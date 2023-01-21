using DependencyProxy.MetadataEngine;
using DependencyProxy.MetadataEngine.Interfaces;

namespace DependencyProxy.Tests.StructuredMetadataEngine;

public class ChildTests : StructuredMetadataEngineTests
{
    protected override IMetadataEngine EngineForMetadata => ChildMetadataEngine;

    [SetUp]
    public void ChildSetup()
    {
        GrandparentMetadataEngine[typeof(int)] = 2;
        ParentMetadataEngine[typeof(int)] = 1;
    }

    [Test]
    public void CanResolveFromParentAndGrandparent()
    {
        Assert.That(EngineForMetadata.FindMetadata<int>().Count(), Is.EqualTo(3));
    }
}
