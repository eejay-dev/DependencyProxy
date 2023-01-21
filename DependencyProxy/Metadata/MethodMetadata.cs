using System.Reflection;

using DependencyProxy.Interfaces;
using DependencyProxy.MetadataEngine;

namespace DependencyProxy;

public class MethodMetadata : StructuredMetadataEngine<ServiceMetadata>
{
    public override ServiceMetadata ParentMetadataEngine { get; }

    /// <summary>
    /// Implementation Types MethodInfo (except when only an interface is provided)
    /// </summary>
    public MethodInfo MethodInfo { get; }

    public ICollection<ParameterMetadata> ParameterMetadata { get; }

    public ReturnTypeMetadata ReturnTypeMetadata { get; }

    public MethodMetadata(ServiceMetadata parent, MethodInfo methodInfo)
    {
        ParentMetadataEngine = parent;
        MethodInfo = methodInfo;

        ParameterMetadata = methodInfo.GetParameters()
            .Select(p => new ParameterMetadata(this, p))
            .ToList();

        ReturnTypeMetadata = new ReturnTypeMetadata(this, methodInfo.ReturnType);
    }

    public MethodMetadata Parameter(int position, Func<ParameterMetadata, ParameterMetadata> parameterFunc)
    {
        var parameterMetadata =
            ParameterMetadata.FirstOrDefault(p => p.ParameterInfo.Position == position) 
            ?? throw new Exception("No argument at provided position");

        parameterMetadata = parameterFunc(parameterMetadata);

        return this;
    }

    public MethodMetadata Parameter(string name, Func<ParameterMetadata, ParameterMetadata> parameterFunc)
    {
        var parameterMetadata =
            ParameterMetadata.FirstOrDefault(p => p.ParameterInfo.Name == name)
            ?? throw new Exception("No argument with provided name");

        parameterMetadata = parameterFunc(parameterMetadata);

        return this;
    }

    internal List<Type> AnalyserTypes = new();

    public MethodMetadata HasAnalyser<TAnalyser>()
        where TAnalyser : IBaseMetadataAnalyser, IMethodMetadataTarget, new()
    {
        AnalyserTypes.Add(typeof(TAnalyser));

        return this;
    }

    internal List<Type> IgnoredAnalyserTypes = new();

    public MethodMetadata IgnoreAnalyser<TAnalyser>()
        where TAnalyser : IBaseMetadataAnalyser, IMethodMetadataTarget, new()
    {
        IgnoredAnalyserTypes.Add(typeof(TAnalyser));

        return this;
    }
}