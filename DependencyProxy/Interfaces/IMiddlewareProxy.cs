using Castle.DynamicProxy;

using Microsoft.Extensions.DependencyInjection;

namespace DependencyProxy;

public interface IMiddlewareProxy : IInterceptor
{
    ProxyMetadata? ProxyMetadata { get; set; }
}
