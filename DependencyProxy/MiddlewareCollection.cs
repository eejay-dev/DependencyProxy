namespace DependencyProxy;

public class MiddlewareCollection : List<Func<MiddlewareInvocation, Task>>
{

}
