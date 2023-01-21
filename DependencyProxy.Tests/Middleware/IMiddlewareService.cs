namespace DependencyProxy.Tests.Middleware;

public interface IMiddlewareService
{
    T EchoGeneric<T>(T val);
    Task<T> EchoGenericAsync<T>(T val);
}