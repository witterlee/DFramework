using System;
using System.Collections.Generic;
using System.Reflection;

namespace DFramework.Utilities
{
    public static class TypeUtil
    {
        public static IEnumerable<Type> GetGenericArgumentTypes(Type concreteType, Type genericType)
        {
            foreach (var @interface in concreteType.GetInterfaces())
            {
                if (!@interface.IsGenericType) continue;

                if (@interface.GetGenericTypeDefinition() == genericType)
                {
                    foreach (var argType in @interface.GetGenericArguments())
                    {
                        yield return argType;
                    }
                }
            }
        }
        public static bool IsAttributeDefinedInMethodOrDeclaringClass(MethodInfo method, Type attributeType)
        {
            return method.IsDefined(attributeType, false) || method.DeclaringType.IsDefined(attributeType, false);
        }
    }
}
