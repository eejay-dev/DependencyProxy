// See https://aka.ms/new-console-template for more information

using Castle.DynamicProxy;

using DependencyProxy;

public class BasicProxy : DefaultMiddlewareProxy
{
    public BasicProxy(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override void Intercept(IInvocation invocation)
    {
        var a= invocation.Method.IsConstructedGenericMethod;

        invocation.Proceed();
    }
}