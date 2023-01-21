using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DependencyProxy.Interfaces;
using DependencyProxy.MetadataEngine;
using DependencyProxy.MetadataEngine.Interfaces;

namespace DependencyProxy;
public interface IMetadataAnalyser<TMetadata> : IBaseMetadataAnalyser
    where TMetadata : IMetadataEngine
{
    /// <summary>
    /// Analyse (and modify) proxy metadata after creating the proxy with
    /// <see cref="ServiceCollectionExtensions.ProxyServicesWith{TProxy}" />
    /// </summary>
    /// <param name="metadata"></param>
    void AnalyseMetadata(TMetadata metadata);
}
public interface IBaseMetadataAnalyser
{

}