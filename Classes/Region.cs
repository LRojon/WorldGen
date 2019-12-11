using LibNoise;
using System;
using System.Windows;
using VoronoiLib.Structures;

namespace WorldGen.Classes
{
    public class Region : VoronoiLib.Structures.FortuneSite
    {
        private int _height;

        public int Height
                 {
                     get => _height;
                     set
                     { 
                         this._height = value < 0 ? 0 : value > 255 ? 255 : value;
                     }
                 }

        public Region(double x, double y, Perlin perlin) : base(x, y)
        {
            this.Height = (int)(perlin.GetValue(x, y, 0) * 127) + 127;
        }
    }
}