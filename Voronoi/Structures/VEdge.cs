﻿using System;

namespace VoronoiLib.Structures
{
    [Serializable]
    public class VEdge
    {
        public VPoint Start { get; set; }
        public VPoint End { get; set; }
        public FortuneSite Left { get; }
        public FortuneSite Right { get; }
        internal double SlopeRise { get; }
        internal double SlopeRun { get; }
        internal double? Slope { get; }
        internal double? Intercept { get; }

        public VEdge Neighbor { get; internal set; }

        internal VEdge() { }
        internal VEdge(VPoint start, FortuneSite left, FortuneSite right)
        {
            Start = start;
            Left = left;
            Right = right;
            
            //for bounding box edges
            if (left == null || right == null)
                return;

            //from negative reciprocal of slope of line from left to right
            //ala m = (left.y -right.y / left.x - right.x)
            SlopeRise = left.X - right.X;
            SlopeRun = -(left.Y - right.Y);
            Intercept = null;

            if (SlopeRise.ApproxEqual(0) || SlopeRun.ApproxEqual(0)) return;
            Slope = SlopeRise/SlopeRun;
            Intercept = start.Y - Slope*start.X;
        }
    }
}