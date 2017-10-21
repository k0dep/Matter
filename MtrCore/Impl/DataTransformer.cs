using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MatterCore.Impl
{
    public delegate int DataTransformBinDelegate(int a, int b);
    public delegate int DataTransformUniDelegate(int input);

    public class DataTransformer : IDataTransformer
    {
        public Dictionary<DataTransformBinFunction, DataTransformBinDelegate> DataTransformBinFunctions { get; set; }
        public Dictionary<DataTransformUniFunction, DataTransformUniDelegate> DataTransformUniFunctions { get; set; }

        public DataTransformer()
        {
            DataTransformBinFunctions = new Dictionary<DataTransformBinFunction, DataTransformBinDelegate>();
            DataTransformUniFunctions = new Dictionary<DataTransformUniFunction, DataTransformUniDelegate>();

            GetAllMethodsWithAttribute<AutoDataTransformBinAttribute>(this.GetType())
                .ForEach(method =>
                {
                    var invocableLink =
                        (DataTransformBinDelegate)method.CreateDelegate(typeof(DataTransformBinDelegate), this);

                    DataTransformBinFunctions.Add(
                        method.GetCustomAttribute<AutoDataTransformBinAttribute>().BinFunction,
                        invocableLink
                    );
                });


            GetAllMethodsWithAttribute<AutoDataTransformUniAttribute>(this.GetType())
                .ForEach(method =>
                {
                    var invocableLink =
                        (DataTransformUniDelegate)method.CreateDelegate(typeof(DataTransformUniDelegate), this);

                    DataTransformUniFunctions.Add(
                        method.GetCustomAttribute<AutoDataTransformUniAttribute>().UniFunction,
                        invocableLink
                    );
                });
        }

        public List<MethodInfo> GetAllMethodsWithAttribute<TAttr>(Type type) where TAttr : Attribute
        {
            var methodsResult = new List<MethodInfo>();
            foreach (var methodInfo in type.GetMethods())
            {
                if(methodInfo.GetCustomAttributes<TAttr>().Any())
                    methodsResult.Add(methodInfo);
            }
            if(type.BaseType != null)
                methodsResult.AddRange(GetAllMethodsWithAttribute<TAttr>(type.BaseType));

            return methodsResult;
        }

        public int TransformBin(DataTransformBinFunction function, int a, int b) =>
            DataTransformBinFunctions[function](a, b);

        public int TransformUni(DataTransformUniFunction function, int input) =>
            DataTransformUniFunctions[function](input);


        [AutoDataTransformBin(DataTransformBinFunction.Add)]
        public int Add(int a, int b)
            => a + b;

        [AutoDataTransformBin(DataTransformBinFunction.Sub)]
        public int Sub(int a, int b)
            => a - b;

        [AutoDataTransformBin(DataTransformBinFunction.Mul)]
        public int Mul(int a, int b)
            => a * b;

        [AutoDataTransformBin(DataTransformBinFunction.Div)]
        public int Div(int a, int b)
            => a / b;

        [AutoDataTransformUni(DataTransformUniFunction.Neg)]
        public int Negative(int input)
            => -input;


        #region Attributes

        [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
        sealed class AutoDataTransformBinAttribute : Attribute
        {
            public DataTransformBinFunction BinFunction { get; set; }

            public AutoDataTransformBinAttribute(DataTransformBinFunction binFunction)
            {
                BinFunction = binFunction;
            }
        }

        [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
        sealed class AutoDataTransformUniAttribute : Attribute
        {
            public DataTransformUniFunction UniFunction { get; set; }

            public AutoDataTransformUniAttribute(DataTransformUniFunction uniFunction)
            {
                UniFunction = uniFunction;
            }
        }

        #endregion
    }
}
