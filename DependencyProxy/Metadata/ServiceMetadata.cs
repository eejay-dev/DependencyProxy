using System;
using System.Linq.Expressions;
using System.Reflection;

using DependencyProxy.Interfaces;
using DependencyProxy.MetadataEngine;

using Microsoft.Extensions.DependencyInjection;

namespace DependencyProxy;

public class ServiceMetadata : StructuredMetadataEngine<ProxyMetadata>
{
    public override ProxyMetadata ParentMetadataEngine { get; }

    public ServiceDescriptor ServiceDescriptor { get; }

    protected ICollection<MethodMetadata> AllMethods { get; }

    public IEnumerable<MethodMetadata> Methods
    {
        get
        {
            var methodFilter = this.FindFirstOrDefaultMetadata<IMethodFilter>() ?? IMethodFilter.Default;

            return AllMethods.Where(methodFilter.AllowMethod);
        }
    }

    public ServiceMetadata(ProxyMetadata parent, ServiceDescriptor serviceDescriptor)
    {
        ParentMetadataEngine = parent;
        ServiceDescriptor = serviceDescriptor;

        AllMethods =
            serviceDescriptor.ServiceType
            .GetMethods()
            .Select(m => new MethodMetadata(this, m))
            .ToList();
    }

    public ServiceMetadata Method(MethodInfo method, Func<MethodMetadata, MethodMetadata> func)
    {
        var methodMetadata =
            Methods.FirstOrDefault(m => m.MethodInfo.HasSameMetadataDefinitionAs(method))
            ?? throw new Exception("Could not find MethodMetadata matching method");

        methodMetadata = func(methodMetadata);

        return this;
    }

    public ServiceMetadata Method<TService>(Expression<Func<TService, Delegate>> methodExpression, Func<MethodMetadata, MethodMetadata> methodFunc)
    {
        var operand = (methodExpression.Body as UnaryExpression)?.Operand;

        var obj = (operand as MethodCallExpression)?.Object;

        var value = (obj as ConstantExpression)?.Value as MethodInfo;

        if (value == null)
        {
            throw new Exception("Could not find method from expression");
        }

        if (value.IsConstructedGenericMethod)
        {
            value = value.GetGenericMethodDefinition();
        }

        return Method(value, methodFunc);
    }

    internal List<Type> AnalyserTypes = new();

    public ServiceMetadata HasAnalyser<TAnalyser>()
        where TAnalyser : IBaseMetadataAnalyser, IServiceMetadataTarget, new()
    {
        AnalyserTypes.Add(typeof(TAnalyser));

        return this;
    }

    internal List<Type> IgnoredAnalyserTypes = new();

    public ServiceMetadata IgnoreAnalyser<TAnalyser>()
        where TAnalyser : IBaseMetadataAnalyser, IServiceMetadataTarget, new()
    {
        IgnoredAnalyserTypes.Add(typeof(TAnalyser));

        return this;
    }
}