using DependencyProxy.MetadataEngine;

namespace DependencyProxy.Tests.StructuredMetadataEngine.Engines;

public class GrandparentMetadataEngine : BaseMetadataEngine
{
    public ICollection<TestParentMetadataEngine> Children { get; set; } = new List<TestParentMetadataEngine>();
}

