using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using DependencyProxy.Analysers;
using DependencyProxy.Interfaces;
using DependencyProxy.MetadataEngine;
using DependencyProxy.MetadataEngine.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace DependencyProxy;

public class ProxyMetadata : BaseMetadataEngine
{
    public ICollection<ServiceMetadata> ServiceMetadata { get; }

    public ServiceLifetime ServiceLifetime { get; private set; }

    public ProxyMetadata(IEnumerable<ServiceDescriptor> services, ServiceLifetime serviceLifetime)
    {
        ServiceMetadata = services.Select(s => new ServiceMetadata(this, s)).ToList();
        ServiceLifetime = serviceLifetime;
    }

    public ProxyMetadata Service<TService>(Func<ServiceMetadata, ServiceMetadata> serviceSetupFunc)
    {
        var serviceMetadata = 
            ServiceMetadata.FirstOrDefault(sm => sm.ServiceDescriptor.ServiceType == typeof(TService)) 
            ?? throw new ArgumentException("", nameof(TService));

        serviceMetadata = serviceSetupFunc(serviceMetadata);

        return this;
    }

    private Dictionary<Type, ServiceMetadata> ImplementationMetadata { get; } = new();

    public ServiceMetadata? GetServiceMetadataForImplementation(Type implementationType)
    {
        if (!ImplementationMetadata.TryGetValue(implementationType, out var metadata) || metadata == null)
        {
            metadata = ServiceMetadata
                .FirstOrDefault(sm => (sm.ServiceDescriptor.ImplementationType ?? sm.ServiceDescriptor.ServiceType) == implementationType);

            ImplementationMetadata[implementationType] = metadata;
        }

        return metadata;
    }

    public ProxyMetadata HasLifetime(ServiceLifetime lifetime)
    {
        ServiceLifetime = lifetime;

        return this;
    }

    internal List<Type> AnalyserTypes = new()
    {
        typeof(NonVirtualMethodAnalyser),
        typeof(ServiceLifetimeAnalyser)
    };

    public ProxyMetadata HasAnalyser<TAnalyser>()
        where TAnalyser : IBaseMetadataAnalyser, IProxyMetadataTarget, new()
    {
        AnalyserTypes.Add(typeof(TAnalyser));

        return this;
    }

    internal List<Type> IgnoredAnalyserTypes = new();

    public ProxyMetadata IgnoreAnalyser<TAnalyser>()
        where TAnalyser : IBaseMetadataAnalyser, IProxyMetadataTarget, new()
    {
        IgnoredAnalyserTypes.Add(typeof(TAnalyser));

        return this;
    }
}
