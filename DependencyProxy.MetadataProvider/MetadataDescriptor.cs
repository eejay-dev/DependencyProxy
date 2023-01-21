using DependencyProxy.MetadataProvider.Interfaces;

namespace DependencyProxy.MetadataProvider;

public class MetadataDescriptor<TMetadata>
{
    public Type MetadataType { get; }
    public TMetadata? MetadataInstance { get; }
    public Func<IMetadataProvider<TMetadata>, TMetadata>? MetadataFactory { get; }

    public MetadataDescriptor(Type metadataType)
    {
        MetadataType = metadataType;
    }

    public MetadataDescriptor(Type metadataType, TMetadata metadataInstance)
    {
        MetadataType = metadataType;
        MetadataInstance = metadataInstance;
    }

    public MetadataDescriptor(Type metadataType, Func<IMetadataProvider<TMetadata>, TMetadata> metadataFactory)
    {
        MetadataType = metadataType;
        MetadataFactory = metadataFactory;
    }
}