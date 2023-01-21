using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DependencyProxy.MetadataEngine.Interfaces;

namespace DependencyProxy.MetadataEngine;

public abstract class StructuredMetadataEngine<TParentMetadataEngine> : BaseMetadataEngine, IStructuredMetadataEngine
    where TParentMetadataEngine : IMetadataEngine
{
    public abstract TParentMetadataEngine ParentMetadataEngine { get; }

    IMetadataEngine IStructuredMetadataEngine.ParentMetadataEngine => ParentMetadataEngine;
}
