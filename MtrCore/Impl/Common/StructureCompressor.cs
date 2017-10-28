
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MtrCore.Common
{
    public class StructureCompressor
    {
        public List<PackingPart> PartsPacking { get; set; }

        public StructureCompressor()
        {
            PartsPacking = new List<PackingPart>();
        }

        public StructureCompressor Pack(int bitCount, int data)
        {
            PartsPacking.Add(new PackingPart(bitCount, data));
            return this;
        }

        public StructureCompressor PackHalf(int data)
        {
            return Pack(4, data);
        }

        public StructureCompressor PackByte(int data)
        {
            return Pack(8, data);
        }

        public int Build(int startValue = 0)
        {
            if(PartsPacking.Sum(t => t.BitCount) > 32)
                throw new InvalidDataException("packed structure owerflow int32");

            var result = startValue;
            foreach (var packingPart in PartsPacking)
            {
                result = result << packingPart.BitCount;
                result |= packingPart.GetPureData();
            }
            return result;
        }

        public class PackingPart
        {
            public int BitCount { get; set; }
            public int RawData { get; set; }

            public PackingPart(int bitCount, int rawData)
            {
                BitCount = bitCount;
                RawData = rawData;
            }

            public int GetMask()
            {
                int mask = 1;
                for (int i = 0; i < BitCount - 1; i++)
                {
                    mask = mask << 1 | 1;
                }
                return mask;
            }

            public int GetPureData()
            {
                return RawData & GetMask();
            }
        }
    }    
}
