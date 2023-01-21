// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;

using DependencyProxy;

using Microsoft.Extensions.DependencyInjection;

//[EtwProfiler(performExtraBenchmarksRun: false)]
[MemoryDiagnoser]
public class DefaultProxyBenchmark : SimpleClassBenchmarks
{

    public DefaultProxyBenchmark()
    {
        var sc = new ServiceCollection();

        sc.ProxyServicesWith<DefaultMiddlewareProxy>(
            services => services
                .AddSingleton<SimpleClass>(), 
            proxy => proxy
                .Intercept(async invocation =>
                {
                    invocation.ReturnValue = "nottest";

                    await invocation.Execute();
                })
            );

        InterceptedClass = sc.BuildServiceProvider().GetRequiredService<SimpleClass>();

        sc = new ServiceCollection();

        sc.ProxyServicesWith<BasicProxy>(
            services => services
                .AddSingleton<SimpleClass>(),
            proxy => proxy);

        BaseClass = sc.BuildServiceProvider().GetRequiredService<SimpleClass>();
    }
}
