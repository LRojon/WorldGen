using LibNoise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Globalization;
using VoronoiLib.Structures;
using WorldGen.Classes.Enum;

namespace WorldGen.Classes
{
    [Serializable]
    public class Region : VoronoiLib.Structures.FortuneSite
    {
        // Zone pour ressource principale
        private int _height;
        private bool _city;
        private bool _capital;
        private double _influence;
        private double _godInfluence;
        private Kingdom _kingdom;
        private God _god;
        private string _name;
        private CitySize _size;
        private int _money;
        private int _citizen;
        private Dictionary<RaceDominante, double> _distribution;
        private Ressource _ressource;

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
        public double GodInfluence { get => _godInfluence; set => _godInfluence = value; }
        internal God God { get => _god; set => _god = value; }
        public Ressource Ressource { get => _ressource; set => _ressource = value; }

        public Region() { }
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

        public void GenRegion(double[,] heightMap, int seed = 0)
        {
            if (this.City)
            {
                Random r;
                if (seed == 0)
                    r = new Random();
                else
                    r = new Random(seed);

                int randS = (int)(this.Height * 10.78) + r.Next(99999);
                if (this.Capital)
                {
                    this.Name = NameGenerator.GenCapitalName(randS);
                    this.Size = CitySize.Capitale;
                }
                else
                {
                    this.Size = (CitySize)r.Next(5);
                    this.Name = NameGenerator.GenCityName(this.Size, randS);
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

                // Gen Ressource
                Dictionary<double, int> compte = new Dictionary<double, int>();
                for (int i = (int)(this.X - 25); i < this.X + 25; i++)
                {
                    for (int j = (int)(this.Y - 25); j < this.Y + 25; j++)
                    {
                        double k = System.Math.Round(heightMap[i, j], 2);
                        if (compte.ContainsKey(k))
                            compte[k]++;
                        else
                            compte.Add(k, 1);
                    }
                }

                double h = compte.OrderByDescending(kvp => kvp.Value).First().Key;
                if (h >= .3)
                    this.Ressource = Ressource.Minerai;
                else if (h <= .05)
                    this.Ressource = Ressource.Poisson;
                else
                    this.Ressource = (r.Next(2) == 0 ? Ressource.Céréale : Ressource.Bétail);

                // Fill Kingdom
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

                            this.Kingdom.Distribution[kvp.Key] = nb;
                        }
                        else
                        {
                            this.Distribution.TryGetValue(kvp.Key, out double val);

                            this.Kingdom.Distribution.Add(kvp.Key, (int)(this.Citizen * val));
                        }
                    }

                    if(!this.Kingdom.Richesse.ContainsKey(this.Ressource))
                    {
                        this.Kingdom.Richesse.Add(this.Ressource, 1);
                    }
                    else
                    {
                        this.Kingdom.Richesse[this.Ressource]++;
                    }
                }
            }
        }

        public string GetHTMLInfo()
        {
            if(this.City)
            {
                string tmp = "Argent: " + this.Money.ToString("N0", new CultureInfo("ru-RU")) + " PO<br>";
                tmp += "Population: " + this.Citizen + " Habitant<br>";

                foreach(KeyValuePair<RaceDominante, double> kvp in this.Distribution)
                {
                    string race = "";
                    switch (kvp.Key)
                    {
                        case RaceDominante.Elfe_sylvain:
                            race = "Elfe Sylvain";
                            break;
                        case RaceDominante.Gnome:
                            race = "Gnome";
                            break;
                        case RaceDominante.Haut_elfe:
                            race = "Haut Elfe";
                            break;
                        case RaceDominante.Humain:
                            race = "Humain";
                            break;
                        case RaceDominante.Nain:
                            race = "Nain";
                            break;
                        case RaceDominante.Autres:
                            race = "Autres";
                            break;
                    }
                    tmp += (int)(kvp.Value * this.Citizen) + " " + race + "<br>";
                }

                return tmp;
            }

            return "";
        }

        public override string ToString()
        {
            if (this.City)
                return this.Name + (this.Kingdom != null ? " - " + this.Kingdom.Name : "");
            else
                return "Not a city";
        }
    }
}