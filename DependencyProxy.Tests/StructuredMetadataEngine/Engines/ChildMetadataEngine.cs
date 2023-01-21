using DependencyProxy.MetadataEngine;

namespace DependencyProxy.Tests.StructuredMetadataEngine.Engines;

public class ChildMetadataEngine : StructuredMetadataEngine<TestParentMetadataEngine>
{
    public override TestParentMetadataEngine ParentMetadataEngine { get; }

    public ChildMetadataEngine(TestParentMetadataEngine parentMetadataEngine)
    {
        ParentMetadataEngine = parentMetadataEngine;
    }
}