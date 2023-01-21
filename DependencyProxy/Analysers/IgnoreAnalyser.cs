using DependencyProxy.MetadataEngine.Interfaces;

namespace DependencyProxy;

public class IgnoreAnalyser<TAnalyser, TMetadata> : IIgnoreAnalyser
    where TAnalyser : IMetadataAnalyser<TMetadata>
    where TMetadata : IMetadataEngine
{
    public Type AnalyserType => typeof(TAnalyser);
}

public interface IIgnoreAnalyser
{
    Type AnalyserType { get; }
}
