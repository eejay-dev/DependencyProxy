using Microsoft.Extensions.DependencyInjection;

namespace DependencyProxy.MetadataProvider.Interfaces;

public interface IMetadataProvider<TMetadataBase>
{
    IDictionary<Type, TMetadataBase> Metadata { get; set; }

    IServiceCollection
}
