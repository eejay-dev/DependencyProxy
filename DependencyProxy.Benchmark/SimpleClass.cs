// See https://aka.ms/new-console-template for more information

public class SimpleClass
{
    public virtual T GenericEcho<T>(T value)
    {
        return value;
    }

    public virtual async Task<T> GenericEchoAsync<T>(T value)
    {
        return value;
    }

    public virtual void Void()
    {

    }

    public virtual async Task VoidAsync()
    {

    }

    public virtual void Slow()
    {
        Thread.Sleep(1);
    }

    public virtual async Task SlowAsync()
    {
        await Task.Delay(1);
    }
}
