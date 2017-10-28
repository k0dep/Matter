using System;
using System.Configuration;
using System.IO;
using MatterCore.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MtrCore.Common;

namespace MtrCoreTest.Common
{
    [TestClass]
    public class StructureCompressorTest
    {
        [TestMethod]
        public void Order()
        {
            var actual = new StructureCompressor()
                .Pack(4, 0)
                .Pack(4, 1)
                .Pack(4, 2)
                .Pack(4, 3)
                .Pack(4, 4)
                .Pack(4, 5)
                .Pack(4, 6)
                .Pack(4, 7)
                .Build();

            Assert.AreEqual(0x01234567, actual);
        }

        [TestMethod]
        public void ShouldOwerflow()
        {
            Assert.ThrowsException<InvalidDataException>
            (() => new StructureCompressor()
                .Pack(20, 0)
                .Pack(30, 0)
                .Build()
                );
        }

        [TestMethod]
        public void DefaultFill()
        {
            var actual = new StructureCompressor()
                .Pack(4, 0xf)
                .Build(0xA);

            Assert.AreEqual(0xAF, actual);
        }
    }
}
