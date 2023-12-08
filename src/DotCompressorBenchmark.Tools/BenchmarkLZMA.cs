using System;
using System.IO;
using System.Security.Authentication.ExtendedProtection;
using SevenZip;
using SevenZip.Compression.LZMA;

namespace DotCompressorBenchmark.Tools;

public class BenchmarkLZMA : IBenchmark
{
    public struct CLzmaEncProps
    {
        public int level; /* 0 <= level <= 9 */
    
        public int dictSize; /* (1 << 12) <= dictSize <= (1 << 27) for 32-bit version
                            (1 << 12) <= dictSize <= (3 << 29) for 64-bit version
                            default = (1 << 24) */
    
        public int lc; /* 0 <= lc <= 8, default = 3 */
        public int lp; /* 0 <= lp <= 4, default = 0 */
        public int pb; /* 0 <= pb <= 4, default = 2 */
        public int algo; /* 0 - fast, 1 - normal, default = 1 */
        public int fb; /* 5 <= fb <= 273, default = 32 */
        public int btMode; /* 0 - hashChain Mode, 1 - binTree mode - normal, default = 1 */
        public int numHashBytes; /* 2, 3 or 4, default = 4 */
        public int mc; /* 1 <= mc <= (1 << 30), default = 32 */
        public bool writeEndMark; /* 0 - do not write EOPM, 1 - write EOPM, default = 0 */
        public int numThreads; /* 1 or 2, default = 2 */
    
        public uint reduceSize; /* estimated size of data that will be compressed. default = (UInt64)(Int64)-1.
                              Encoder uses this value to reduce dictionary size */
    
        public CLzmaEncProps()
        {
            level = 5;
            dictSize = mc = 0;
            reduceSize = uint.MaxValue;
            lc = lp = pb = algo = fb = btMode = numHashBytes = numThreads = -1;
            writeEndMark = false;
        }
    }

    public string Name { get; }
    private readonly int _level;

    public BenchmarkLZMA(int level)
    {
        _level = level;
        Name = $"lzma 22.1.1 -{_level}";
    }


    public BenchmarkResult Roundtrip(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return Benchmarks.Roundtrip(Name, filename, srcBytes, dstBytes,
            (s, d) => Compress(s, d, _level), Decompress);
    }

    public static long Compress(byte[] uncompressedBytes, byte[] compressedBytes, int level)
    {
        var props = new CLzmaEncProps();
        props.level = level;
        
        if (0 == props.dictSize) props.dictSize = (level <= 5 ? (1 << (level * 2 + 14)) : (level <= 7 ? (1 << 25) : (1 << 26)));
        if (props.dictSize > props.reduceSize)
        {
            uint reduceSize = (uint)props.reduceSize;
            for (int i = 11; i <= 30; i++)
            {
                if (reduceSize <= (2 << i))
                {
                    props.dictSize = (2 << i);
                    break;
                }
        
                if (reduceSize <= (3 << i))
                {
                    props.dictSize = (3 << i);
                    break;
                }
            }
        }
        
        if (props.lc < 0) props.lc = 3;
        if (props.lp < 0) props.lp = 0;
        if (props.pb < 0) props.pb = 2;
        
        if (props.algo < 0) props.algo = (level < 5 ? 0 : 1);
        if (props.fb < 0) props.fb = (level < 7 ? 32 : 64);
        if (props.btMode < 0) props.btMode = (props.algo == 0 ? 0 : 1);
        if (props.numHashBytes < 0) props.numHashBytes = 4;
        if (props.mc == 0) props.mc = ((16 + (props.fb >> 1)) >> (0 != props.btMode ? 0 : 1));
        if (props.numThreads < 0) props.numThreads = ((0 != props.btMode && 0 != props.algo) ? 2 : 1);

        using MemoryStream inStream = new MemoryStream(uncompressedBytes);
        using MemoryStream outStream = new MemoryStream(compressedBytes);
        var encoder = new Encoder();
        encoder.SetCoderProperties(new CoderPropID[]
        {
            CoderPropID.DictionarySize,
            //CoderPropID.UsedMemorySize,
            //CoderPropID.BlockSize,
            CoderPropID.PosStateBits,
            CoderPropID.LitContextBits,
            CoderPropID.LitPosBits,
            //CoderPropID.NumFastBytes,
            //CoderPropID.MatchFinder,
            //CoderPropID.MatchFinderCycles,
            //CoderPropID.NumPasses,
            CoderPropID.Algorithm,
            //CoderPropID.NumThreads,
            CoderPropID.EndMarker,
        }, new object[]
        {
            props.dictSize,
            //props.reduceSize,
            //props.fb,
            props.pb,
            props.lc,
            props.lp,
            //props.numHashBytes,
            //props.mc,
            props.algo,
            //props.numThreads,
            props.writeEndMark,
        });
        encoder.WriteCoderProperties(outStream);
        outStream.Write(BitConverter.GetBytes((long)uncompressedBytes.Length), 0, 8);
        encoder.Code(inStream, outStream, uncompressedBytes.Length, compressedBytes.Length, null);

        return outStream.Position;
    }

    public static long Decompress(byte[] compressedBytes, long size, byte[] uncompressedBytes)
    {
        using var inStream = new MemoryStream(compressedBytes, 0, (int)size);
        using var outStream = new MemoryStream(uncompressedBytes);

        byte[] properties = new byte[5];
        inStream.Read(properties, 0, 5); // header

        byte[] originalSizeBytes = new byte[8];
        inStream.Read(originalSizeBytes, 0, 8); // size
        long originalSize = BitConverter.ToInt64(originalSizeBytes);

        var decoder = new Decoder();
        decoder.SetDecoderProperties(properties);
        decoder.Code(inStream, outStream, size, uncompressedBytes.Length, null);

        return outStream.Position;
    }
}