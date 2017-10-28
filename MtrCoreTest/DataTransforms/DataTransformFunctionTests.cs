using MatterCore;
using MatterCore.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MtrCoreTest.DataTransforms
{
    [TestClass]
    public class DataTransformFunctionTests
    {
        public DataTransformer DataTransformInstnce { get; set; }

        public DataTransformFunctionTests()
        {
            DataTransformInstnce = new DataTransformer();
        }

        [TestMethod]
        public void Adding()
        {
            Assert.AreEqual(10, DataTransformInstnce.TransformBin(DataTransformBinFunction.Add, 4, 6));
        }

        [TestMethod]
        public void Substracting()
        {
            Assert.AreEqual(-2, DataTransformInstnce.TransformBin(DataTransformBinFunction.Sub, 4, 6));
        }

        [TestMethod]
        public void Multiply()
        {
            Assert.AreEqual(24, DataTransformInstnce.TransformBin(DataTransformBinFunction.Mul, 4, 6));
        }

        [TestMethod]
        public void Divide()
        {
            Assert.AreEqual(3, DataTransformInstnce.TransformBin(DataTransformBinFunction.Div, 6, 2));
        }

        [TestMethod]
        public void Negative()
        {
            Assert.AreEqual(-1, DataTransformInstnce.TransformUni(DataTransformUniFunction.Neg, 1));
        }
    }
}
