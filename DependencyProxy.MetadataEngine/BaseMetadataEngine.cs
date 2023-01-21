using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using DependencyProxy.MetadataEngine.Interfaces;

namespace DependencyProxy.MetadataEngine;

public class BaseMetadataEngine : IMetadataEngine
{
    public virtual Dictionary<Type, object?> Metadata { get;  } = new();

    public object? this[Type key] 
    {
        get => Metadata.TryGetValue(key, out object? value) ? value : null;
        set => Metadata[key] = value;
    }
}