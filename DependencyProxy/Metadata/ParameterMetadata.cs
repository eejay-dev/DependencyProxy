using System.Reflection;

using DependencyProxy.Interfaces;
using DependencyProxy.MetadataEngine;

namespace DependencyProxy;

public class ParameterMetadata : StructuredMetadataEngine<MethodMetadata>, IParameterMetadataTarget
{
    public override MethodMetadata ParentMetadataEngine { get; }

    public ParameterInfo ParameterInfo { get; }

    public ParameterMetadata(MethodMetadata methodMetadata, ParameterInfo parameterInfo)
    {
        ParentMetadataEngine = methodMetadata;
        ParameterInfo= parameterInfo;
    }

    internal List<Type> AnalyserTypes = new();

    public ParameterMetadata HasAnalyser<TAnalyser>()
        where TAnalyser : IBaseMetadataAnalyser, IParameterMetadataTarget, new()
    {
        AnalyserTypes.Add(typeof(TAnalyser));

        return this;
    }

    internal List<Type> IgnoredAnalyserTypes = new();

    public ParameterMetadata IgnoreAnalyser<TAnalyser>()
        where TAnalyser : IBaseMetadataAnalyser, IParameterMetadataTarget, new()
    {
        IgnoredAnalyserTypes.Add(typeof(TAnalyser));

        return this;
    }
}
