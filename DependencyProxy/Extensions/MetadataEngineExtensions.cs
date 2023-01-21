using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using DependencyProxy.Interfaces;
using DependencyProxy.MetadataEngine;
using DependencyProxy.MetadataEngine.Interfaces;

namespace DependencyProxy;

public static class MetadataEngineExtensions
{
    public static TEngine Intercept<TEngine>(this TEngine engine, Func<MiddlewareInvocation, Task> middleware)
        where TEngine : IMetadataEngine
    {
        engine.GetMetadataOrSet(new MiddlewareCollection())
            .Add(middleware);

        return engine;
    }
}
