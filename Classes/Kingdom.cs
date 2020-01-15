using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using WorldGen.Classes.Enum;
using VoronoiLib;
using VoronoiLib.Structures;
using System.Windows;
using System.Globalization;

namespace WorldGen.Classes
{
    /*
     * Race dominante
     */

    [Serializable]
    public class Kingdom
    {
        public List<Region> regions = new List<Region>();
        private string _name;
        private KingdomType _type;
        private Color _color;
        private Region _capital;
        private string _leader; // Deviendra HistoricalPNJ
        private RaceDominante _race;
        private int _money;
        private int _citizen;
        private Dictionary<RaceDominante, int> _distribution = new Dictionary<RaceDominante, int>();
        private God _god;
        private Dictionary<Ressource, int> _richesse = new Dictionary<Ressource, int>();
        private Currency _currency;

        public Kingdom() { }
        public Kingdom(Region capital, Color color, Pantheon pantheon)
        {
            if(!capital.Capital)
                throw new Exception("La region fourni n'est pas une capitale.");

            Random r = new Random();

            this.Color = color;
            this.Type = (KingdomType)(r.Next(4));
            this.Name = NameGenerator.GenKingdomName(r.Next(99999) + color.R + color.B + color.G);
            this.Leader = "Roi";
            this.Race = (RaceDominante)r.Next(5);
            this.Currency = new Currency(this);

            if (this.Type == KingdomType.Théocratie)
                this.God = pantheon.Gods.ElementAt(r.Next(pantheon.Gods.Count));

            this.Capital = capital;
            capital.Influence = 1;
            CreateKingdom(capital);
        }

        public string Name { get => _name; set => _name = value; }
        public Color Color { get => _color; set => _color = value; }
        public KingdomType Type { get => _type; set => _type = value; }
        public string Leader { get => _leader; set => _leader = value; }
        public Region Capital { get => _capital; set => _capital = value; }
        public RaceDominante Race { get => _race; set => _race = value; }
        public int Money { get => _money; set => _money = value; }
        public int Citizen { get => _citizen; set => _citizen = value; }
        public Dictionary<RaceDominante, int> Distribution { get => _distribution; set => _distribution = value; }
        internal God God { get => _god; set => _god = value; }
        public Dictionary<Ressource, int> Richesse { get => _richesse; set => _richesse = value; }
        internal Currency Currency { get => _currency; set => _currency = value; }

        private void CreateKingdom(Region current)
        {
            if (current.Influence >= 0.1)
            {
                current.Kingdom = this;
                this.regions.Add(current);
                foreach (Region neighbor in current.Neighbors)
                {
                    if (neighbor.Influence < current.Influence - 0.1)
                    {
                        neighbor.Influence = current.Influence - 0.1;
                        CreateKingdom(neighbor);
                    }
                }
            }
        }

        public void Purge()
        {
            List<Region> tmp = new List<Region>();
            foreach(Region r in this.regions)
            {
                if(!tmp.Contains(r) && r.Kingdom == this)
                {
                    tmp.Add(r);
                }
            }
            this.regions = tmp;
        }


        public string GetDemoInfo()
        {
            string tmp ="Population: " + this.Citizen.ToString("N0", CultureInfo.CreateSpecificCulture("ru-RU")) + "<br>";
            foreach(KeyValuePair<RaceDominante, int> kvp in this.Distribution)
            {
                string race = "";
                switch(kvp.Key)
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
                tmp += kvp.Value.ToString("N0", CultureInfo.CreateSpecificCulture("ru-RU")) + " " + race + "<br>";
            }
            
            return tmp;
        }
        
        public string GetEcoInfo()
        {
            string tmp = "Argent: " + this.Money.ToString("N0", CultureInfo.CreateSpecificCulture("ru-RU")) + " " + this.Currency.Po.Name + "<br>";
            tmp += "Ressource principale du pays: " + this.Richesse.OrderByDescending(kvp => kvp.Value).First().Key + "<br>";
            foreach(KeyValuePair<Ressource,int> kvp in this.Richesse.OrderByDescending(kvp => kvp.Value))
            {
                tmp += kvp.Key.ToString() + ": " + kvp.Value + " ville<br>";
            }

            return tmp;
        }
        public override string ToString()
        {
            return this.Name;
        }
    }
}