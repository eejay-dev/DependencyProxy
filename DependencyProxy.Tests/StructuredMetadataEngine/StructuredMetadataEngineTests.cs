using DependencyProxy.MetadataEngine;
using DependencyProxy.MetadataEngine.Interfaces;
using DependencyProxy.Tests.StructuredMetadataEngine.Engines;

namespace DependencyProxy.Tests.StructuredMetadataEngine;

public abstract class StructuredMetadataEngineTests
{
    protected GrandparentMetadataEngine GrandparentMetadataEngine { get; }

    protected TestParentMetadataEngine ParentMetadataEngine { get; }

    protected ChildMetadataEngine ChildMetadataEngine { get; }

    protected abstract IMetadataEngine EngineForMetadata { get; }

    public StructuredMetadataEngineTests()
    {
        GrandparentMetadataEngine = new();

        ParentMetadataEngine = new(GrandparentMetadataEngine);

        ChildMetadataEngine = new(ParentMetadataEngine);
    }

    [SetUp]
    public void Setup()
    {
        EngineForMetadata[typeof(int)] = 0;
        EngineForMetadata[typeof(string)] = "test";
    }

    [TestCase(0)]
    [TestCase("test")]
    public void FindPrimativeMetadata<TMetadata>(TMetadata result)
    {
        Assert.That(ChildMetadataEngine.FindFirstOrDefaultMetadata<TMetadata>(), Is.EqualTo(result));
    }
}
