namespace MatterCore
{
    public interface IDataTransformer
    {
        int TransformBin(DataTransformBinFunction function, int a, int b);
        int TransformUni(DataTransformUniFunction function, int input);
    }

    public enum DataTransformUniFunction
    {
        Neg,
    }

    public enum DataTransformBinFunction
    {
        Add,
        Sub,
        Div,
        Mul
    }

}