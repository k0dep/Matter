using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MtrCore.Common;

namespace MtrCoreTest.Common
{
    [TestClass]
    public class StructureDecompressorTest
    {
        [TestMethod]
        public void Order()
        {
            Assert.AreEqual(0xa, new StructureDecompressor(0x9a).DepackHalf());
            Assert.AreEqual(0x9a, new StructureDecompressor(0x9a).DepackByte());
        }

        [TestMethod]
        public void ShuldThrow()
        {
            var depacker = new StructureDecompressor(0);
            depacker.Depack(32);

            Assert.ThrowsException<ArgumentException>(() =>
            {
                depacker.Depack(3);
            });
        }

        [TestMethod]
        public void Skiping()
        {
            Assert.AreEqual(0x9, new StructureDecompressor(0x9a).SkipHalf().DepackHalf());
        }
    }
}
