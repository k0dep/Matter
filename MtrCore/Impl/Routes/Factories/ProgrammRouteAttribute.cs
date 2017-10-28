
using System;
using MatterCore.Routes;

namespace MtrCore.Routes.Factories
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class ProgrammRouteAttribute : Attribute
    {
        public ProgramRoutesId Id;

        public ProgrammRouteAttribute(ProgramRoutesId id)
        {
            Id = id;
        }
    }
}
