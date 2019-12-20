using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGen.Classes.Enum;

namespace WorldGen.Classes
{
    public class Pantheon
    {
        private readonly List<God> _gods = new List<God>();
        private readonly List<string> _titles = new List<string>(new string[]{
                "des âmes",
                "des plaisir",
                "des animaux",
                "de l'au-delà",
                "des enfers",
                "du ciel",
                "du temps",
                "du passage",
                "du feu",
                "de l'eau",
                "de l'air",
                "de la terre",
                "des éléments",
                "de la guerre",
                "de l'agriculture",
                "de la nuit",
                "de la paix",
                "de la vengeance",
                "de la ruse",
                "de la destruction",
                "des foyers",
                "de la musique",
                "de la récolte",
                "de la lumière",
                "de la loi",
                "des tempêtes",
                "des esprits",
                "de la haine",
                "de la mort",
                "de la famine",
                "de la maladie",
                "de la connaissance"
            });

        public Pantheon(int seed = 0)
        {
            Random r;
            if (seed == 0)
                r = new Random();
            else
                r = new Random(seed);
            int nbGod = r.Next(4, 16);

            for(int i = 0; i < nbGod; i++)
            {
                int id = r.Next(this.Titles.Count);
                var tmp = new God(r.Next(999999) + i)
                {
                    Title = this.Titles.ElementAt(id)
                };
                this.Titles.RemoveAt(id);
                this.Gods.Add(tmp);
            }
            this._gods = this.Gods.OrderBy(e => e.Forgot).ToList();
        }

        public string GetHTMLInfo()
        {
            string tmp = "";
            foreach(God g in this.Gods)
            {
                tmp += g.ToString() + "<br>";
            }
            return tmp;
        }

        private List<string> Titles { get => _titles; }
        internal List<God> Gods { get => _gods; }
    }

    class God
    {
        private string _name;
        private string _title;
        private bool _forgot;
        private bool _sexe;
        private List<Region> _followers = new List<Region>();
        private Region capitale;
        private Color _color;

        public God(int seed = 0)
        {
            Random r;
            if(seed == 0)
                r = new Random();
            else
                r = new Random(seed);

            this.Name = NameGenerator.GenGodName(seed);
            this.Forgot = !Convert.ToBoolean(r.Next(10));
            this.Sexe = Convert.ToBoolean(r.Next(2));
            this.Color = Color.FromArgb(
                r.Next(256),
                r.Next(256),
                r.Next(256));
        }

        // foreach region avec 1 de GodInfluence expendre en -0.1 jusqu'a rencontrer une influence equivalente ou atteindre 0.1
        public void GenInfluence(Region r)
        {
            double seuil;
            if (r.Kingdom != null && r.Kingdom.Type == KingdomType.Théocratie)
                seuil = 0.05;
            else
                seuil = 0.1;

            if (r.GodInfluence >= seuil)
            {
                if(r.God != null)
                    r.God.Followers.Remove(r);

                r.God = this;
                this.Followers.Add(r);
                foreach (Region neighbor in r.Neighbors)
                {
                    if (neighbor.GodInfluence < r.GodInfluence - seuil)
                    {
                        neighbor.GodInfluence = r.GodInfluence - seuil;
                        GenInfluence(neighbor);
                    }
                }
            }
        }

        public override string ToString()
        {
            return this.Name + ", " + (this.Sexe ?  (this.Forgot ? "Dieu oublié " : "Dieu ") : (this.Forgot ? "Déesse oubliée " : "Déesse ")) + this.Title;
        }

        public string Name { get => _name; set => _name = value; }
        public string Title { get => _title; set => _title = value; }
        public bool Forgot { get => _forgot; set => _forgot = value; }
        public bool Sexe { get => _sexe; set => _sexe = value; }
        public List<Region> Followers { get => _followers; set => _followers = value; }
        public Region Capitale { get => capitale; set => capitale = value; }
        public Color Color { get => _color; set => _color = value; }
    }
}
