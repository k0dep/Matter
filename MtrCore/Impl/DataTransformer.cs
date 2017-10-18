using System.Collections.Generic;

namespace MatterCore.Impl
{
    public delegate int DataTransformBinDelegate(int a, int b);
    public delegate int DataTransformUniDelegate(int input);

    public class DataTransformer : IDataTransformer
    {
        public Dictionary<DataTransformBinFunction, DataTransformBinDelegate> DataTransformBinFunctions { get; set; }
        public Dictionary<DataTransformUniFunction, DataTransformUniDelegate> DataTransformUniFunctions { get; set; }

        public int TransformBin(DataTransformBinFunction function, int a, int b) =>
            DataTransformBinFunctions[function](a, b);

        public int TransformUni(DataTransformUniFunction function, int input) =>
            DataTransformUniFunctions[function](input);
    }
}
