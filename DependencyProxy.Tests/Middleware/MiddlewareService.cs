namespace DependencyProxy.Tests.Middleware;

public class MiddlewareService : IMiddlewareService
{
    public virtual T EchoGeneric<T>(T val) => val;

    public async virtual Task<T> EchoGenericAsync<T>(T val) => val;
}