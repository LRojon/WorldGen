﻿using System;
using System.Collections.Generic;

namespace VoronoiLib.Structures
{
    [Serializable]
    public class FortuneSite
    {
        public double X { get; }
        public double Y { get; }

        public List<VEdge> Cell { get; set; }

        public List<FortuneSite> Neighbors { get; private set; }

        public FortuneSite() { }
        public FortuneSite(double x, double y)
        {
            X = x;
            Y = y;
            Cell = new List<VEdge>();
            Neighbors = new List<FortuneSite>();
        }
    }
}