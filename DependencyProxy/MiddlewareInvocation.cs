using System.Reflection;

using Castle.DynamicProxy;

namespace DependencyProxy;

public class MiddlewareInvocation
{
    protected IInvocation Invocation { get; }
    public IServiceProvider ServiceProvider { get; }
    public MethodMetadata MethodMetadata { get; }

    public object[] Arguments => Invocation.Arguments;

    public Type[] GenericArguments => Invocation.GenericArguments;

    public object InvocationTarget => Invocation.InvocationTarget;

    public MethodInfo Method => Invocation.Method;

    public MethodInfo MethodInvocationTarget => Invocation.MethodInvocationTarget;

    public object Proxy => Invocation.Proxy;

    /// <summary>
    /// Should always be taskless
    /// </summary>
    public object? ReturnValue
    {
        get => Invocation.ReturnValue;
        set => Invocation.ReturnValue = value;
    }

    public Type TargetType => Invocation.TargetType;

    public bool HasProceeded { get; private set; } = false;

    public Func<Func<MiddlewareInvocation, Task>?> GetNextAction { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="invocation">param1</param>
    /// <param name="serviceProvider">param2</param>
    /// <param name="getNextAction">param3</param>
    public MiddlewareInvocation(IInvocation invocation, IServiceProvider serviceProvider, MethodMetadata methodMetadata, Func<Func<MiddlewareInvocation, Task>?> getNextAction)
    {
        Invocation = invocation;
        ServiceProvider = serviceProvider;
        MethodMetadata = methodMetadata;
        GetNextAction = getNextAction;
    }

    public async Task Next() => await (GetNextAction()?.Invoke(this) ?? Task.CompletedTask);

    public async Task Execute()
    {
        Invocation.Proceed();

        var returnType = ReturnValue?.GetType();

        if (returnType == typeof(Task))
        {
            ReturnValue = null;
        }
        else if (returnType?.IsAssignableTo(typeof(Task)) ?? false)
        {
            await ((ReturnValue as Task) ?? Task.CompletedTask);

            if (returnType.GenericTypeArguments.Any() == false ||
                returnType.GenericTypeArguments.Any(ta => ta.FullName == "System.Threading.Tasks.VoidTaskResult"))
            {
                ReturnValue = null;
            }

            ReturnValue = (ReturnValue as dynamic)?.Result;
        }
    }
}