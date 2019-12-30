using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGen.Classes.Enum;

namespace WorldGen.Classes
{
    public abstract class SourceOfMagic
    {
        private string _description;


        public SourceOfMagic() { }
        public SourceOfMagic(string description)
        {
            this.Description = description;
        }

        public string Description { get => _description; set => _description = value; }
    }

    public class Don : SourceOfMagic
    {
        public Don()
        {
        }

        public Don(string description) : base(description)
        { }
    }

    public class Mana : SourceOfMagic
    {
        private string _quantite;
        private bool _recepHumain;

        public string Quantite { get => _quantite; set => _quantite = value; }
        public bool RecepHumain { get => _recepHumain; set => _recepHumain = value; }

        public Mana(string description, string quantite, bool recepHumain) : base(description)
        {
            this.Quantite = quantite;
            this.RecepHumain = recepHumain;
        }

        public Mana()
        {
        }
    }

    public class Plan : SourceOfMagic
    {
        private bool _tunnel;
        private bool _hostile;

        public bool Tunnel { get => _tunnel; set => _tunnel = value; }
        public bool Hostile { get => _hostile; set => _hostile = value; }

        public Plan(string description, bool tunnel, bool hostile) : base(description)
        {
            this.Tunnel = tunnel;
            this.Hostile = hostile;
        }

        public Plan()
        {
        }
    }

    public class Lien : SourceOfMagic
    {
        public Lien()
        {
        }

        public Lien(string description) : base(description)
        { }
    }

    public class Etre : SourceOfMagic
    {
        private HistoricalPNJ _him;
        private string _state;

        public Etre()
        {
        }

        public Etre(string description, HistoricalPNJ him, string state) : base (description)
        {
            this.Him = him;
            this.State = state;
        }

        public HistoricalPNJ Him { get => _him; set => _him = value; }
        public string State { get => _state; set => _state = value; }
    }

    public class Artefacts : SourceOfMagic
    {
        private List<Artefact> _artefacts;

        public Artefacts(Localization cadre)
        {
            this.List = new List<Artefact>();
            var r = new Random();
            int nb = r.Next(3, 9);
            for(int i = 0; i < nb; i++)
            {
                string[] tmpName;
                do
                {
                    tmpName = NameGenerator.GenArtefactName();
                } while (this.List.Where(a => a.Name == tmpName[0]).Count() != 0);
                var tmp = new Artefact()
                {
                    Name = tmpName[0],
                    Position = new Localization()
                    {
                        X = r.Next(cadre.X) + 50,
                        Y = r.Next(cadre.Y) + 50,
                        Name = (r.Next() % 2 == 0 ? "Le sanctuaire" : "Le temple") + tmpName[1],
                    }
                };
                this.List.Add(tmp);
            }

            this.Description = "La magie est gardé dans ce monde tant que les " + this.List.Count + " artéfacts reste dans leur sanctuaire.";
        }

        public Artefacts(string description, List<Artefact> artefacts) : base(description)
        {
            this.List = artefacts;
        }

        internal List<Artefact> List { get => _artefacts; set => _artefacts = value; }
    }

    public class Artefact
    {
        private Localization _localization;
        private string _name;

        public Artefact()
        {
        }

        public override string ToString()
        {
            return this.Name;
        }

        public Localization Position { get => _localization; set => _localization = value; }
        public string Name { get => _name; set => _name = value; }
    }

    public class Localization
    {
        private int _x;
        private int _y;
        private string _name;
        private bool _underwater;

        public Localization()
        {
            this.X = 0;
            this.Y = 0;
        }

        public Localization(int x, int y, string name)
        {
            this.X = x;
            this.Y = y;
            this.Name = name;
        }

        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        public string Name { get => _name; set => _name = value; }
        public bool Underwater { get => _underwater; set => _underwater = value; }
    }
}
