namespace DependencyProxy;

public class DefaultMethodFilter : IMethodFilter
{
    private bool AllowStatic { get; }

    public DefaultMethodFilter(bool allowStatic = false)
    {
        AllowStatic = allowStatic;
    }

    public bool AllowMethod(MethodMetadata methodMetadata)
        => methodMetadata.MethodInfo.DeclaringType != typeof(object) &&
           (AllowStatic || methodMetadata.MethodInfo.IsStatic == false);
}
