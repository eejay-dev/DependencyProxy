using DependencyProxy.MetadataEngine.Interfaces;

namespace DependencyProxy;

public static class MetadataEngineExtensions
{
    public static TEngine SetMetadata<TEngine, TMetadata>(this TEngine engine)
        where TEngine : IMetadataEngine
        where TMetadata : new()
    {
        engine[typeof(TMetadata)] = new TMetadata();

        return engine;
    }

    public static TEngine SetMetadata<TEngine, TMetadata>(this TEngine engine, TMetadata metadata)
        where TEngine : IMetadataEngine
    {
        engine[typeof(TMetadata)] = metadata;

        return engine;
    }

    public static TMetadata? GetMetadataOrDefault<TMetadata>(this IMetadataEngine engine)
        => (TMetadata?)engine[typeof(TMetadata)];

    public static TMetadata GetMetadataOrSet<TMetadata>(this IMetadataEngine engine)
        where TMetadata : new() => GetMetadataOrSet(engine, new TMetadata());

    public static TMetadata GetMetadataOrSet<TMetadata>(this IMetadataEngine engine, TMetadata metadata)
    {
        return (TMetadata)(engine[typeof(TMetadata)] ??= metadata)!;
    }
}