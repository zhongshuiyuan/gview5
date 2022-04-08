﻿using gView.Framework.Geometry;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gView.Framework.FDB
{
    public interface ISpatialTreeInfo
    {
        string type { get; }
        IEnvelope Bounds { get; }
        double SpatialRatio { get; }
        int MaxFeaturesPerNode { get; }

        Task<List<SpatialIndexNode>> SpatialIndexNodes();
    }
}
