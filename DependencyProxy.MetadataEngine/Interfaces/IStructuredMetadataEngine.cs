using DependencyProxy.MetadataEngine.Interfaces;

namespace DependencyProxy.MetadataEngine;

public interface IStructuredMetadataEngine : IMetadataEngine
{
    IMetadataEngine? ParentMetadataEngine { get; }
}