using DependencyProxy.MetadataEngine;
using DependencyProxy.Tests.StructuredMetadataEngine.Engines;

namespace DependencyProxy.Tests.StructuredMetadataEngine;

public class TestParentMetadataEngine : StructuredMetadataEngine<GrandparentMetadataEngine>
{
    public override GrandparentMetadataEngine ParentMetadataEngine { get; }

    public ICollection<ChildMetadataEngine> Children { get; set; } = new List<ChildMetadataEngine>();

    public TestParentMetadataEngine(GrandparentMetadataEngine parentMetadataEngine)
    {
        ParentMetadataEngine = parentMetadataEngine;
    }
}