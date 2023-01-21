using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

using Castle.DynamicProxy;
using DependencyProxy.MetadataEngine;

using Microsoft.Extensions.DependencyInjection;

namespace DependencyProxy;

public class DefaultMiddlewareProxy : IMiddlewareProxy
{
    private ProxyMetadata proxyMetadata { get; set; }

    public ProxyMetadata ProxyMetadata { get => proxyMetadata; set => proxyMetadata = value; }

    public IServiceProvider ServiceProvider { get; }

    public DefaultMiddlewareProxy(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public static Dictionary<MethodInfo, MethodInfo> GenericMethodInfoCache { get; } = new();

    public virtual void Intercept(IInvocation invocation)
    {
        var serviceMetadata =
            ProxyMetadata.GetServiceMetadataForImplementation(invocation.Method.DeclaringType!)
            ?? throw new Exception("Could not find serviceMetadata from interecpted method");

        var actualMethod = invocation.Method;
        
        var method = actualMethod;

        if (serviceMetadata.ServiceDescriptor.ServiceType.IsInterface)
        {
            method = actualMethod.GetInterfaceMethod(serviceMetadata.ServiceDescriptor.ServiceType);
        }

        if (method.IsConstructedGenericMethod)
        {
            if (!GenericMethodInfoCache.TryGetValue(method, out var matchedMethod))
            {
                matchedMethod = method.GetGenericMethodDefinition();
        
                GenericMethodInfoCache[method] = matchedMethod;
            }
        
            method = matchedMethod;
        }
        
        var methodMetadata =
            serviceMetadata.Methods.FirstOrDefault(m => m.MethodInfo.MetadataToken == method.MetadataToken)
            ?? throw new Exception("Could not find intercepted method metadata");
        
        var middlewares =
            methodMetadata.FindMetadataWithLevels<MiddlewareCollection>()
            .OrderByDescending(ml => ml.Level)
            .SelectMany(level => level.Metadata)
            .GetEnumerator();
        
        var middlewareTask = Task.CompletedTask;
        
        if (middlewares.MoveNext())
        {
            middlewareTask = middlewares.Current(new MiddlewareInvocation(
                invocation,
                ServiceProvider,
                methodMetadata,
                () => middlewares.MoveNext() ? middlewares.Current : null)
            );
        }

        var isMethodVoidTask = actualMethod.ReturnType == typeof(Task);
        
        var doesMethodReturnTask = isMethodVoidTask || actualMethod.ReturnType.IsAssignableTo(typeof(Task));
        
        if (doesMethodReturnTask)
        {
            if (!isMethodVoidTask)
            {
                var providedReturnType = invocation.ReturnValue?.GetType();
        
                var actualReturnType = actualMethod.ReturnType.GenericTypeArguments[0];

                invocation.ReturnValue = TaskMethodExtensions.GetTaskOfTReflection(actualReturnType, middlewareTask, invocation.ReturnValue);
            }
            else
            {
                invocation.ReturnValue = middlewareTask;
            }
        }
        else
        {
            middlewareTask.Wait();
        }
    }
}
