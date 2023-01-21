namespace DependencyProxy.MetadataProvider.Interfaces;

public interface IMetadataBuilder<TSelf, TMetadataBase> : IList<MetadataDescriptor<TMetadataBase>>
    where TSelf : IMetadataBuilder<TSelf, TMetadataBase>
{
    TSelf HasMetadata(MetadataDescriptor<TMetadataBase> metadataDescriptor);

    TSelf HasMetadata<TMetadata>();

    TSelf HasMetadata<TMetadata>(TMetadata metadata);

    TSelf HasMetadata<TMetadata>(Func<IMetadataProvider<TMetadataBase>, TMetadata> metadataFactory);

    IMetadataProvider<TMetadataBase> BuildMetadataProvider();
}
