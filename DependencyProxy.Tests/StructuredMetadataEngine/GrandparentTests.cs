using DependencyProxy.MetadataEngine.Interfaces;

namespace DependencyProxy.Tests.StructuredMetadataEngine;

public class GrandparentTests : StructuredMetadataEngineTests
{
    protected override IMetadataEngine EngineForMetadata => GrandparentMetadataEngine;
}