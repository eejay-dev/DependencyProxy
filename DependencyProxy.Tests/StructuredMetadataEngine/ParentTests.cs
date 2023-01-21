using System.Reflection;

using DependencyProxy.MetadataEngine;
using DependencyProxy.MetadataEngine.Interfaces;

namespace DependencyProxy.Tests.StructuredMetadataEngine;

public class ParentTests : StructuredMetadataEngineTests
{
    protected override IMetadataEngine EngineForMetadata => ParentMetadataEngine;
}