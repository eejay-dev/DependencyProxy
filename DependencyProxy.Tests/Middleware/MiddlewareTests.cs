using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using DependencyProxy.Analysers;

using Microsoft.Extensions.DependencyInjection;

namespace DependencyProxy.Tests.Middleware;
public class MiddlewareTests
{
    [SetUp]
    public void Setup()
    {

    }

    [TestCase(1, 2)]
    [TestCase("a", "b")]
    public void EchoDifferentValue<T>(T provided, T overrided)
    {
        var sc = new ServiceCollection();

        sc.ProxyServicesWith<DefaultMiddlewareProxy>(
            services => services
                .AddSingleton<IMiddlewareService, MiddlewareService>()
            ,
            proxy => proxy
                .SetMetadata(overrided)
                .Intercept(async invocation =>
                {
                    invocation.Arguments[0] = proxy[typeof(T)] ?? invocation.Arguments[0];

                    await invocation.Execute();
                    
                    await invocation.Next();
                })
        );

        Assert.That(sc.BuildServiceProvider().GetRequiredService<IMiddlewareService>().EchoGeneric(provided), Is.EqualTo(overrided));
    }

    [TestCase(1, 2)]
    [TestCase("a", "b")]
    public async Task EchoDifferentValueAsync<T>(T provided, T overrided)
    {
        var sc = new ServiceCollection();

        sc.ProxyServicesWith<DefaultMiddlewareProxy>(
            services => services
                .AddSingleton<IMiddlewareService, MiddlewareService>()
            ,
            proxy => proxy
                .SetMetadata(overrided)
                .Intercept(async invocation =>
                {
                    invocation.Arguments[0] = proxy[typeof(T)] ?? invocation.Arguments[0];

                    await invocation.Execute();

                    await invocation.Next();
                })
                .Service<IMiddlewareService>(service => service
                    .Method<IMiddlewareService>(s => s.EchoGenericAsync<object>, method => method
                        .Parameter(0, parameter => parameter)
                    )
                )
        );

        Assert.That(await sc.BuildServiceProvider().GetRequiredService<IMiddlewareService>().EchoGenericAsync(provided), Is.EqualTo(overrided));
    }
}