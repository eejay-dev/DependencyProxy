// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;

public class SimpleClassBenchmarks
{
    public SimpleClass InterceptedClass { get; set; }

    public SimpleClass BaseClass { get; set; }

    [Benchmark]
    public void InterceptedEcho()
    {
        InterceptedClass.GenericEcho("test");
    }

    [Benchmark]
    public void BaseEcho()
    {
        BaseClass.GenericEcho("test");
    }

    [Benchmark]
    public async Task InterceptedEchoAsync()
    {
        await InterceptedClass.GenericEchoAsync("test");
    }

    [Benchmark]
    public async Task BaseEchoAsync()
    {
        await BaseClass.GenericEchoAsync("test");
    }

    [Benchmark]
    public void InterceptedMulti()
    {
        InterceptedClass.GenericEcho("test");
        InterceptedClass.GenericEcho("test");
        InterceptedClass.Void();
        InterceptedClass.Void();
    }

    [Benchmark]
    public void BaseMulti()
    {
        BaseClass.GenericEcho("test");
        BaseClass.GenericEcho("test");
        BaseClass.Void();
        BaseClass.Void();
    }

    [Benchmark]
    public async Task InterceptedMultiAsync()
    {
        await InterceptedClass.GenericEchoAsync("test");
        await InterceptedClass.GenericEchoAsync("test");
        await InterceptedClass.VoidAsync();
        await InterceptedClass.VoidAsync();
    }

    [Benchmark]
    public async Task BaseMultiAsync()
    {
        await BaseClass.GenericEchoAsync("test");
        await BaseClass.GenericEchoAsync("test");
        await BaseClass.VoidAsync();
        await BaseClass.VoidAsync();
    }

#if FALSE

    [Benchmark]
    public void InterceptedSlow()
    {
        InterceptedClass.Slow();
    }

    [Benchmark]
    public void BaseSlow()
    {
        BaseClass.Slow();
    }

    [Benchmark]
    public async Task InterceptedSlowAsync()
    {
        await InterceptedClass.SlowAsync();
    }

    [Benchmark]
    public async Task BaseSlowAsync()
    {
        await BaseClass.SlowAsync();
    }

    [Benchmark]
    public void InterceptedVoid()
    {
        InterceptedClass.Void();
    }

    [Benchmark]
    public void BaseVoid()
    {
        BaseClass.Void();
    }

    [Benchmark]
    public async Task InterceptedVoidAsync()
    {
        await InterceptedClass.VoidAsync();
    }

    [Benchmark]
    public async Task BaseVoidAsync()
    {
        await BaseClass.VoidAsync();
    }
#endif
}
