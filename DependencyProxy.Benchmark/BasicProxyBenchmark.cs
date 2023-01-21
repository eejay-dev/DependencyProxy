// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;

using DependencyProxy;

using Microsoft.Extensions.DependencyInjection;

[MemoryDiagnoser]
public class BasicProxyBenchmark : SimpleClassBenchmarks
{
    public BasicProxyBenchmark() : base()
    {
        var sc = new ServiceCollection();

        sc.ProxyServicesWith<BasicProxy>(
            services => services
                .AddSingleton<SimpleClass>(), 
            proxy => proxy);

        InterceptedClass = sc.BuildServiceProvider().GetRequiredService<SimpleClass>();

        BaseClass = new();
    }
}
