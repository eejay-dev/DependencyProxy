using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DependencyProxy.MetadataEngine.Interfaces;

namespace DependencyProxy.Interfaces;
public interface IProxyMetadataTarget { }

public interface IServiceMetadataTarget : IProxyMetadataTarget { }

public interface IMethodMetadataTarget : IServiceMetadataTarget { }

public interface IParameterMetadataTarget : IMethodMetadataTarget { }

public interface IReturnTypeMetadataTarget : IMethodMetadataTarget { }