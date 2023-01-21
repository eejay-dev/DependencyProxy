using DependencyProxy.MetadataEngine.Interfaces;

namespace DependencyProxy.MetadataEngine.Models;

public class MetadataWithLevel<TMetadata>
{
    public TMetadata Metadata { get; }

    public int Level { get; }

    public IMetadataEngine Engine { get; }

    public MetadataWithLevel(TMetadata metadata, int level, IMetadataEngine engine)
    {
        Metadata = metadata;
        Level = level;
        Engine = engine;
    }
}