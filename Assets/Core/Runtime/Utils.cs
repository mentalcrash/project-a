using System;
using System.Collections.Generic;
using System.Linq;

namespace MC.Project.Core
{
    public static class Utils
    {
        public static IEnumerable<Type> FindTypes( Type customAttribute, Type baseType )
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach( var assembly in assemblies )
            {
                var types = assembly.GetTypes();
                var matchedTypes = types.Where( t => t.IsDefined( customAttribute, false ) );

                foreach( var type in matchedTypes )
                {
                    var hasGameState = baseType.IsAssignableFrom( type );
                    if( hasGameState )
                    {
                        yield return type;
                    }
                }
            }
        }
    }
}