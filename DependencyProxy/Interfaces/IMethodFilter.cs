namespace DependencyProxy;

public interface IMethodFilter
{
    bool AllowMethod(MethodMetadata methodMetadata);

    static IMethodFilter Default { get; } = new DefaultMethodFilter();
}
