using System;
using System.Linq;
using System.Security.Cryptography;
using MatterCore.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MtrCoreTest.Common
{
    [TestClass]
    public class StructureComposerTest
    {
        [TestMethod]
        public void ShouldNormalOneDecompose()
        {
            var result = new StructureComposer().Decompose("11111111", 0x12345678);

            Assert.IsTrue(new byte[]{ 1, 2, 3, 4, 5, 6, 7, 8}.SequenceEqual(result));
        }

        [TestMethod]
        public void ShouldNormalOneCompose()
        {
            var result = new StructureComposer().Compose("11111111", 1, 2, 3, 4, 5, 6, 7, 8);

            Assert.AreEqual(0x12345678, result);
        }

        [TestMethod]
        public void ShouldNormalMultiCompose()
        {
            var result = new StructureComposer().Compose("12122", 1, 0x23, 4, 0x56, 0x78);

            Assert.AreEqual(0x12345678, result);
        }

        [TestMethod]
        public void ShouldNormalMultiDecompose()
        {
            var result = new StructureComposer().Decompose("12122", 0x12345678);

            Assert.IsTrue(result.SequenceEqual(new byte[] { 1, 0x23, 4, 0x56, 0x78 }));
        }

        [TestMethod]
        public void ShouldThrow()
        {
            Assert.ThrowsException<ArgumentException>(
                () => new StructureComposer().Decompose("1adc", 0x11112211)
            );

            Assert.ThrowsException<ArgumentException>(
                () => new StructureComposer().Decompose("12122123", 0x11112211)
            );
        }
    }
}
