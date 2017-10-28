
using System;
using System.Collections.Generic;
using System.Reflection;
using MatterCore;
using MatterCore.Routes;
using MtrCore.Impl.Routes.ProgramRoutes;

namespace MtrCore.Routes.Factories
{

    public class ProgramRouteAutoFactory : IRouteFactory
    {
        public IRoute Create(IPacketFactory packetFactory)
        {
            var programRoute = new ProgrammRoute();

            var types = SelectTypes();

            foreach (var type in types)
            {
                var programRouteId = type.GetCustomAttribute<ProgrammRouteAttribute>().Id;

                if (typeof(ProgramDuplicationRoute).IsAssignableFrom(type))
                {
                    programRoute.RouteProgrammsBases.Add(programRouteId, new ProgramDuplicationRoute(packetFactory));
                    continue;
                }

                programRoute.RouteProgrammsBases.Add(programRouteId, (IProgrammRouteMarker)Activator.CreateInstance(type, new object[] { }));
            }

            return programRoute;
        }

        private static List<Type> SelectTypes()
        {
            var asemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = new List<Type>();
            foreach (var assembly in asemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsAssignableFrom(typeof(IProgrammRouteMarker)) && type.GetCustomAttribute<ProgrammRouteAttribute>() != null)
                        types.Add(type);
                }
            }

            return types;
        }


    }
}
