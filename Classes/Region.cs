using LibNoise;
using System;
using System.Windows;
using VoronoiLib.Structures;

namespace WorldGen.Classes
{
    public class Region : VoronoiLib.Structures.FortuneSite
    {
        private int _height;
        private bool _city;
        private bool _capital;
        private double _influence;
        private Kingdom _kingdom;

        public double Influence
        {
            get => _influence;
            set => _influence = value;
        }
        public int Height
         {
             get => _height;
             set
             { 
                 this._height = value < 0 ? 0 : value > 255 ? 255 : value;
             }
         }
        public bool City { get => _city; set => _city = value; }
        public bool Capital { get => _capital; set => _capital = value; }
        public Kingdom Kingdom { get => _kingdom; set => _kingdom = value; }

        public Region(double x, double y, double[,] perlin) : base(x, y)
        {
            this.Height = (int)(perlin[(int)x, (int)y] * 127) + 127;
            if (this.Height > 135 && this.Height < 200)
                this.City = Convert.ToBoolean(new Random().Next(100) % 3);
            else
                this.City = false;

            this.Capital = false;
            this.Influence = 0;
        }
    }
}