using DependencyProxy.Interfaces;
using DependencyProxy.MetadataEngine;

namespace DependencyProxy;

public class ReturnTypeMetadata : StructuredMetadataEngine<MethodMetadata>
{
    public override MethodMetadata ParentMetadataEngine { get; }
    public Type ReturnType { get; }

    public ReturnTypeMetadata(MethodMetadata parent, Type returnType)
    {
        ParentMetadataEngine = parent;
        ReturnType = returnType;
    }

    internal List<Type> AnalyserTypes = new();

    public ReturnTypeMetadata HasAnalyser<TAnalyser>()
        where TAnalyser : IBaseMetadataAnalyser, IReturnTypeMetadataTarget, new()
    {
        AnalyserTypes.Add(typeof(TAnalyser));

        return this;
    }

    internal List<Type> IgnoredAnalyserTypes = new();

    public ReturnTypeMetadata IgnoreAnalyser<TAnalyser>()
        where TAnalyser : IBaseMetadataAnalyser, IReturnTypeMetadataTarget, new()
    {
        IgnoredAnalyserTypes.Add(typeof(TAnalyser));

        return this;
    }
}