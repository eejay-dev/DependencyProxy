using System.Reflection;

namespace DependencyProxy;

public static class TypeExtensions
{
    public static MethodInfo GetImplementedMethod(this MethodInfo interfaceMethod, Type targetType)
    {
        if (!(interfaceMethod.DeclaringType?.IsInterface ?? true))
        {
            return interfaceMethod;
        }

        var map = targetType.GetInterfaceMap(interfaceMethod.DeclaringType);
        var index = Array.IndexOf(map.InterfaceMethods, interfaceMethod);
        if (index < 0) throw new Exception();

        return map.TargetMethods[index];
    }

    public static MethodInfo GetInterfaceMethod(this MethodInfo implementedMethod, Type interfaceType)
    {
        var map = implementedMethod.DeclaringType!.GetInterfaceMap(interfaceType);
        var index = Array.IndexOf(map.TargetMethods.Select(m => m.MetadataToken).ToArray(), implementedMethod.MetadataToken);
        if (index < 0) throw new Exception();

        return map.InterfaceMethods[index];

    }
}
