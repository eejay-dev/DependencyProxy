using System.Linq.Expressions;
using System.Reflection;

namespace DependencyProxy;

public static class TaskMethodExtensions
{
    private static async Task<TResult?> GetTaskOfT<TResult>(Task init, TResult? value)
    {
        await init;

        return value;
    }

    private static MethodInfo GetTaskOfTMethod =
        typeof(TaskMethodExtensions)
        .GetMethod(nameof(GetTaskOfT), BindingFlags.Static | BindingFlags.NonPublic)!;

    public static Dictionary<Type, MethodInfo> GetTaskOfTCache { get; } = new();

    public static object? GetTaskOfTReflection(Type type, Task init, object? result)
    {
        if (!GetTaskOfTCache.TryGetValue(type, out var method))
        {
            method = GetTaskOfTMethod.MakeGenericMethod(type);

            GetTaskOfTCache[type] = method;
        }

        return method.Invoke(null, new object?[] { init, result });
    }

}
