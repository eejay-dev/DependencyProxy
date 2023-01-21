using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

using Castle.DynamicProxy;

using Microsoft.Extensions.DependencyInjection;

namespace DependencyProxy;
public static class ServiceCollectionExtensions
{
    private static ProxyGenerator ProxyGenerator { get; } = new();

    public static IServiceCollection ProxyServicesWith<TProxy>(
        this IServiceCollection serviceCollection,
        Func<IServiceCollection, IServiceCollection> services,
        Func<ProxyMetadata, ProxyMetadata> proxy)
        where TProxy : IMiddlewareProxy
    {
        var subServiceCollection = services(new ServiceCollection());

        var metadata = proxy(new ProxyMetadata(subServiceCollection, ServiceLifetime.Singleton));

        var analyserCache = new Dictionary<Type, object>();

        object getOrCreate(Type type)
        {
            if (!analyserCache.TryGetValue(type, out var value))
            {
                value = Activator.CreateInstance(type)!;

                analyserCache[type] = value;
            }

            return value;
        }

        var baseAnalyserTypes =
            metadata.AnalyserTypes
            .Where(at => !metadata.IgnoredAnalyserTypes.Contains(at))
            .ToArray();

        foreach(var service in metadata.ServiceMetadata)
        {
            var serviceAnalysers = baseAnalyserTypes.Concat(
                    service.AnalyserTypes
                    .Where(at => !service.IgnoredAnalyserTypes.Contains(at))
                ).ToArray();

            serviceAnalysers.Where(t => t.IsAssignableTo(typeof(IMetadataAnalyser<ServiceMetadata>)))
                .Select(t => (IMetadataAnalyser<ServiceMetadata>)getOrCreate(t))
                .ToList()
                .ForEach(a => a.AnalyseMetadata(service));

            foreach(var method in service.Methods)
            {
                var methodAnalysers = serviceAnalysers.Concat(
                    method.AnalyserTypes
                    .Where(at => !method.IgnoredAnalyserTypes.Contains(at))
                ).ToArray();

                methodAnalysers.Where(t => t.IsAssignableTo(typeof(IMetadataAnalyser<MethodMetadata>)))
                    .Select(t => (IMetadataAnalyser<MethodMetadata>)getOrCreate(t))
                    .ToList()
                    .ForEach(a =>
                    {
                        a.AnalyseMetadata(method);
                    });

                foreach (var parameter in method.ParameterMetadata)
                {
                    var parameterAnalysers = methodAnalysers.Concat(
                        parameter.AnalyserTypes
                        .Where(at => !parameter.IgnoredAnalyserTypes.Contains(at))
                    ).ToArray();

                    methodAnalysers.Where(t => t.IsAssignableTo(typeof(IMetadataAnalyser<ParameterMetadata>)))
                        .Select(t => (IMetadataAnalyser<ParameterMetadata>)getOrCreate(t))
                        .ToList()
                        .ForEach(a => a.AnalyseMetadata(parameter));
                }

                var returnTypeAnalysers = methodAnalysers.Concat(
                    method.ReturnTypeMetadata.AnalyserTypes
                    .Where(at => !method.ReturnTypeMetadata.IgnoredAnalyserTypes.Contains(at))
                ).ToArray();

                methodAnalysers.Where(t => t.IsAssignableTo(typeof(IMetadataAnalyser<ReturnTypeMetadata>)))
                    .Select(t => (IMetadataAnalyser<ReturnTypeMetadata>)getOrCreate(t))
                .ToList()
                    .ForEach(a => a.AnalyseMetadata(method.ReturnTypeMetadata));
            }
        }

        //run base analysers after analysers so they can 
        baseAnalyserTypes.Where(t => t.IsAssignableTo(typeof(IMetadataAnalyser<ProxyMetadata>)))
            .Select(t => (IMetadataAnalyser<ProxyMetadata>)getOrCreate(t))
            .ToList()
            .ForEach(a =>
            {
                a.AnalyseMetadata(metadata);
            });

        foreach (var descriptor in subServiceCollection)
        {
            serviceCollection.Add(TransformDescriptor<TProxy>(descriptor));
        }

        if (!serviceCollection.Any(s => s.ServiceType == typeof(IServiceProvider)))
        {
            serviceCollection.Add(new ServiceDescriptor(typeof(IServiceProvider), sp => sp, ServiceLifetime.Transient));
        }

        if (!serviceCollection.Any(s => s.ServiceType == typeof(TProxy)))
        {
            serviceCollection.Add(
                new ServiceDescriptor(
                    typeof(TProxy), 
                    sp =>
                    {
                        var proxy = ActivatorUtilities.CreateInstance<TProxy>(sp);

                        proxy.ProxyMetadata = metadata;

                        return proxy;
                    }, 
                    metadata.ServiceLifetime)
                );
        }

        return serviceCollection;
    }

    private static ServiceDescriptor TransformDescriptor<TProxy>(ServiceDescriptor service)
        where TProxy : IMiddlewareProxy
    {
        var implementationType = service.ImplementationType ?? service.ServiceType;

        Func<IServiceProvider, object> serviceBuilderFunction =
            implementationType.IsInterface
            ? sp => BuildInterfaceProxy<TProxy>(sp, implementationType)
            : sp => BuildClassProxy<TProxy>(sp, service);

        var methods = implementationType.GetMethods(BindingFlags.Instance | BindingFlags.Public);

        return new ServiceDescriptor(service.ServiceType, serviceBuilderFunction, service.Lifetime);
    }

    private static object BuildInterfaceProxy<TProxy>(IServiceProvider sp, Type interfaceType)
        where TProxy : IMiddlewareProxy
    {
        var proxy = sp.GetRequiredService<TProxy>();

        return ProxyGenerator.CreateInterfaceProxyWithoutTarget(interfaceType, proxy);
    }

    private static object BuildClassProxy<TProxy>(IServiceProvider sp, ServiceDescriptor classDesc)
        where TProxy : IMiddlewareProxy
    {
        var proxy = sp.GetRequiredService<TProxy>();

        var serviceType = classDesc.ServiceType;

        var classType = classDesc.ImplementationType ?? serviceType;

        var instance =
            classDesc.ImplementationInstance
            ?? classDesc.ImplementationFactory?.Invoke(sp)
            ?? ActivatorUtilities.CreateInstance(sp, classType);

        return ProxyGenerator.CreateClassProxyWithTarget(classType, instance, proxy);
    }
}
