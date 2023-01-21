namespace DependencyProxy.MetadataEngine.Interfaces;

public interface IMetadataEngine
{
    Dictionary<Type, object?> Metadata { get; }

    object? this[Type key] { get; set; }
}
