using System;

namespace MtrCore.Common
{
    public class StructureDecompressor
    {
        public int Data { get; set; }

        private int currentUsedBits;

        public StructureDecompressor(int data)
        {
            currentUsedBits = 0;
            Data = data;
        }


        public int Depack(int bitcount)
        {
            if((currentUsedBits + bitcount) > 32)
                throw new ArgumentException("owerflow roll data");

            var result = Data & GetMask(bitcount);

            currentUsedBits += bitcount;
            Data = Data >> bitcount;

            return result;
        }

        public int DepackHalf() => Depack(4);

        public int DepackByte() => Depack(8);


        public StructureDecompressor Skip(int bitcount)
        {
            Depack(bitcount);
            return this;
        }

        public StructureDecompressor SkipHalf() => Skip(4);

        public StructureDecompressor SkipByte() => Skip(8);

        public int GetMask(int BitCount)
        {
            int mask = 1;
            for (int i = 0; i < BitCount - 1; i++)
            {
                mask = mask << 1 | 1;
            }
            return mask;
        }
    }
}
