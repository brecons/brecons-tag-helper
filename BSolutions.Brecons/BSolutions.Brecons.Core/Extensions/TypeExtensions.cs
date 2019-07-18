using System;
using System.Linq;
using System.Reflection;

namespace BSolutions.Brecons.Core.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets a public property from a Type, even if it is an interface
        /// </summary>
        /// <remarks>Calling GetProperty on an interface returns only the properties defined in the interface
        /// but not the ones inherited from the interfaces it implements. This solution was posted in S.O:
        /// https://stackoverflow.com/questions/358835/getproperties-to-return-all-properties-for-an-interface-inheritance-hierarchy
        /// </remarks>
        /// <param name="type">The type</param>
        /// <param name="propertyName">The property Name</param>
        /// <returns>The property info for the requested property</returns>
        public static PropertyInfo GetPublicProperty(this Type type, string propertyName)
        {
            if (!type.GetTypeInfo().IsInterface)
            {
                return type.GetProperty(propertyName);
            }

            return (new Type[] { type })
                   .Concat(type.GetInterfaces())
                   .Select(i => i.GetProperty(propertyName)).Distinct().Where(pi => pi != null).SingleOrDefault();
        }
    }
}
