using LibNoise;
using System;
using System.Collections.Generic;
using System.Windows;
using VoronoiLib.Structures;
using WorldGen.Classes.Enum;

namespace WorldGen.Classes
{
    public class Region : VoronoiLib.Structures.FortuneSite
    {
        // Zone pour ressource principale
        private int _height;
        private bool _city;
        private bool _capital;
        private double _influence;
        private Kingdom _kingdom;
        private string _name;
        private CitySize _size;
        private int _money;
        private int _citizen;
        private Dictionary<RaceDominante, double> _distribution;

        public double Influence { get => _influence; set => _influence = value; }
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
        public string Name { get => _name; set => _name = value; }
        public CitySize Size { get => _size; set => _size = value; }
        public int Money { get => _money; set => _money = value; }
        public int Citizen { get => _citizen; set => _citizen = value; }
        public Dictionary<RaceDominante, double> Distribution { get => _distribution; set => _distribution = value; }

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

        public void GenRegion()
        {
            if (this.City)
            {
                Random r = new Random();
                if (this.Capital)
                {
                    int seed = (int)(this.Height * 10.78) + r.Next(99999);
                    this.Name = NameGenerator.GenCapitalName(seed);
                    this.Size = CitySize.Capitale;
                }
                else
                {
                    this.Name = NameGenerator.GenCityName();
                    this.Size = (CitySize)r.Next(5);
                }

                double percentP; double percentS; double percentT; double percentA;
                switch(this.Size)
                {
                    case CitySize.Hameau:
                        this.Citizen = r.Next(50, 451);
                        this.Money = r.Next(75, 551) * this.Citizen;
                        percentP = .96; percentS = .02; percentT = .01; percentA = .01;
                        break;
                    case CitySize.Village:
                        this.Citizen = r.Next(450, 1001);
                        this.Money = r.Next(75, 551) * this.Citizen;
                        percentP = .78; percentS = .09; percentT = .05; percentA = .08;
                        break;
                    case CitySize.Bourg:
                        this.Citizen = r.Next(1000, 2501);
                        this.Money = r.Next(330, 951) * this.Citizen;
                        percentP = .78; percentS = .09; percentT = .05; percentA = .08;
                        break;
                    case CitySize.Ville:
                        this.Citizen = r.Next(2500, 5001);
                        this.Money = r.Next(330, 951) * this.Citizen;
                        percentP = .78; percentS = .09; percentT = .05; percentA = .08;
                        break;
                    case CitySize.Cité:
                        this.Citizen = r.Next(10000, 25001);
                        this.Money = r.Next(800, 1500) * this.Citizen;
                        percentP = .36; percentS = .20; percentT = .18; percentA = .26;
                        break;
                    case CitySize.Capitale:
                        this.Citizen = r.Next(25000, 85001);
                        this.Money = r.Next(800, 1500) * this.Citizen;
                        percentP = .36; percentS = .20; percentT = .18; percentA = .26;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                this.Distribution = new Dictionary<RaceDominante, double>();
                RaceDominante test;
                if (this.Kingdom == null)
                    test = (RaceDominante)r.Next(5);
                else
                    test = this.Kingdom.Race;
                switch (test)
                {
                    case RaceDominante.Humain:
                        var s = (RaceDominante)r.Next(1, 5);
                        RaceDominante t;
                        do { t = (RaceDominante)r.Next(1, 5); } while (s == t);
                        this.Distribution.Add(RaceDominante.Humain, percentP);
                        this.Distribution.Add(s, percentS);
                        this.Distribution.Add(t, percentT);
                        this.Distribution.Add(RaceDominante.Autres, percentA);
                        break;
                    case RaceDominante.Elfe_sylvain:
                        this.Distribution.Add(RaceDominante.Elfe_sylvain, percentP);
                        this.Distribution.Add(RaceDominante.Humain, percentS);
                        this.Distribution.Add(RaceDominante.Haut_elfe, percentT);
                        this.Distribution.Add(RaceDominante.Autres, percentA);
                        break;
                    case RaceDominante.Nain:
                        this.Distribution.Add(RaceDominante.Nain, percentP);
                        this.Distribution.Add(RaceDominante.Gnome, percentS);
                        this.Distribution.Add(RaceDominante.Humain, percentT);
                        this.Distribution.Add(RaceDominante.Autres, percentA);
                        break;
                    case RaceDominante.Gnome:
                        this.Distribution.Add(RaceDominante.Gnome, percentP);
                        this.Distribution.Add(RaceDominante.Nain, percentS);
                        this.Distribution.Add(RaceDominante.Haut_elfe, percentT);
                        this.Distribution.Add(RaceDominante.Autres, percentA);
                        break;
                    case RaceDominante.Haut_elfe:
                        this.Distribution.Add(RaceDominante.Haut_elfe, percentP);
                        this.Distribution.Add(RaceDominante.Elfe_sylvain, percentS);
                        this.Distribution.Add(RaceDominante.Humain, percentT);
                        this.Distribution.Add(RaceDominante.Autres, percentA);
                        break;

                }

                if (this.Kingdom != null)
                {
                    this.Kingdom.Money += this.Money;
                    this.Kingdom.Citizen += this.Citizen;
                    foreach (KeyValuePair<RaceDominante, double> kvp in this.Distribution)
                    {
                        if (this.Kingdom.Distribution.ContainsKey(kvp.Key))
                        {
                            this.Kingdom.Distribution.TryGetValue(kvp.Key, out int nb);
                            this.Distribution.TryGetValue(kvp.Key, out double val);

                            nb += (int)(this.Citizen * val);

                            this.Kingdom.Distribution.Remove(kvp.Key);
                            this.Kingdom.Distribution.Add(kvp.Key, nb);
                        }
                        else
                        {
                            this.Distribution.TryGetValue(kvp.Key, out double val);

                            this.Kingdom.Distribution.Add(kvp.Key, (int)(this.Citizen * val));
                        }
                    }
                }
            }
        }
    }
}