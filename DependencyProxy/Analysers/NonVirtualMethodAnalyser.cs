using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using DependencyProxy.Interfaces;
using DependencyProxy.MetadataEngine;
using DependencyProxy.MetadataEngine.Interfaces;

namespace DependencyProxy.Analysers;
public class NonVirtualMethodAnalyser : IMetadataAnalyser<MethodMetadata>, IMetadataAnalyser<ProxyMetadata>, IMethodMetadataTarget
{
    private List<MethodInfo> FailedMethods { get; set; } = new();

    public void AnalyseMetadata(MethodMetadata metadata)
    {
        if (!metadata.MethodInfo.DeclaringType.IsInterface && (metadata.MethodInfo.IsFinal || !metadata.MethodInfo.IsVirtual))
        {
            FailedMethods.Add(metadata.MethodInfo);
        }
    }

    public void AnalyseMetadata(ProxyMetadata metadata)
    {
        if (FailedMethods.Any())
        {
            var errorString = string.Join("\r\n", FailedMethods.Select(fm => $"Non virtual method detected: {fm.DeclaringType.Name}.{fm.Name}"));

            throw new Exception(errorString);
        }
    }
}

public class ServiceLifetimeAnalyser : IMetadataAnalyser<ServiceMetadata>, IServiceMetadataTarget
{
    public void AnalyseMetadata(ServiceMetadata metadata)
    {
        if (metadata.ParentMetadataEngine.ServiceLifetime > metadata.ServiceDescriptor.Lifetime)
        {
            throw new Exception("Can not have service with a longer lifetime than it's proxy");
        }
    }
}
