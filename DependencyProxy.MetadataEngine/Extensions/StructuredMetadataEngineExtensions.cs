using DependencyProxy.MetadataEngine;
using DependencyProxy.MetadataEngine.Interfaces;
using DependencyProxy.MetadataEngine.Models;

namespace DependencyProxy;

public static class StructuredMetadataEngineExtensions
{
    public static IEnumerable<MetadataWithLevel<TMetadata>> FindMetadataWithLevels<TMetadata>(this IMetadataEngine engine, int currentLevel = 0)
    {
        var matchingMetadata = engine[typeof(TMetadata)];

        if (matchingMetadata != null)
        {
            yield return new MetadataWithLevel<TMetadata>((TMetadata?)matchingMetadata, currentLevel, engine);
        }

        if (engine is IStructuredMetadataEngine structuredEngine)
        {
            foreach(var metadataLevel in 
                structuredEngine.ParentMetadataEngine?.FindMetadataWithLevels<TMetadata>(currentLevel++) ?? Array.Empty<MetadataWithLevel<TMetadata>>())
            {
                yield return metadataLevel;
            }
        }
    }

    public static IEnumerable<MetadataWithLevel<object>> FindMetadataWithLevels(this IMetadataEngine engine, Func<KeyValuePair<Type, object?>, bool> filter, int currentLevel = 0)
    {
        foreach(var metadata in engine.Metadata.Where(filter))
        {
            yield return new MetadataWithLevel<object>(metadata.Value, currentLevel, engine);
        }

        if (engine is IStructuredMetadataEngine structuredEngine)
        {
            foreach (var metadataLevel in
                structuredEngine.ParentMetadataEngine?.FindMetadataWithLevels(filter, currentLevel++) ?? Array.Empty<MetadataWithLevel<object>>())
            {
                yield return metadataLevel;
            }
        }
    }

    public static IEnumerable<TMetadata> FindMetadata<TMetadata>(this IMetadataEngine engine)
    {
        return FindMetadataWithLevels<TMetadata>(engine).Select(m => (TMetadata)m.Metadata);
    }

    public static TMetadata FindFirstMetadata<TMetadata>(this IMetadataEngine engine)
    {
        return FindMetadataWithLevels<TMetadata>(engine).Select(m => (TMetadata)m.Metadata).First();
    }

    public static TMetadata? FindFirstOrDefaultMetadata<TMetadata>(this IMetadataEngine engine)
    {
        return FindMetadataWithLevels<TMetadata>(engine).Select(m => (TMetadata)m.Metadata).FirstOrDefault();
    }
}
