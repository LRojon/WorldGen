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
        private God _god;

        public Don(Pantheon pantheon)
        {
            this.God = pantheon.Gods.OrderByDescending(g => g.Followers.Sum(r => r.Citizen)).First();
            this.Description = "Lors de la création du monde " + this.God.ToString() + " donna au mortel la possibilité de faire de la magie en rassemblant le mana ambient.";
        }

        public Don(string description) : base(description)
        { }

        internal God God { get => _god; set => _god = value; }
    }

    public class Plan : SourceOfMagic
    {
        private bool _tunnelVisible;
        private bool _hostile;
        private Creature _creature;

        public bool Visible { get => _tunnelVisible; set => _tunnelVisible = value; }
        public bool Hostile { get => _hostile; set => _hostile = value; }
        public Creature Creature { get => _creature; set => _creature = value; }

        public Plan(string description, bool visible, bool hostile, Creature creature) : base(description)
        {
            this.Visible = visible;
            this.Hostile = hostile;
            this.Creature = creature;
        }

        public Plan()
        {
            var r = new Random();
            this.Creature = r.Next(100) % 2 == 0 ? Creature.Null : (Creature)r.Next(7);
            this.Visible = Convert.ToBoolean(r.Next(2));
            this.Hostile = Convert.ToBoolean(r.Next(2));
            this.Description = "Le mana provient d'un trou amenant à un autre plan";

            switch (this.Creature)
            {
                case Creature.Ange:
                    this.Description += ", d'où est tombé  l'ange. ";
                    break;
                case Creature.Null:
                    this.Description += ". ";
                    break;
                default:
                    this.Description += ", d'où est tombé le " + this.Creature + ". ";
                    break;
            }
            if (this.Hostile)
                this.Description += "Un plan dangereux et hostile au plan des mortels. ";
            else
                this.Description += r.Next(100) % 2 == 0 ? "Un plan inhabité voir désolé. " : "Un plan où vivent des peuples paisible et pacifiste. ";

            if (this.Visible)
                this.Description += "L'entrer de se plan est connu " +
                    (r.Next(100) % 2 == 0 ? "de tous " : "des érudits et des mages ") +
                    "on l'appel " +
                    (r.Next(100) % 2 == 0 ? "le Soleil. " : "la Lune. ") +
                    (this.Creature != Creature.Null ? "Plusieurs groupes (notament les mages) vénère " +
                    (this.Creature == Creature.Ange ? "l'Ange" : "le " + this.Creature) +
                    " autant que les dieux." : "");
            else
                this.Description += "Malhereusement, l'entrer de se trou n'a toujours pas été découvert.";
        }
    }
    public enum Creature
    {
        Dragon,
        Titan,
        Démon,
        Ange,
        Kraken,
        Phénix,
        Tarasque,
        Null
    }

    public class Lien : SourceOfMagic
    {
        public Lien()
        {
        }

        public Lien(string description) : base(description)
        { }
    }

    public class Titan : SourceOfMagic
    {
        public Titan() { }

        public Titan(string description) : base (description) { }
    }

    public class Etre : SourceOfMagic
    {
        private HistoricalPNJ _him;
        private State _state;
        private Localization _where;

        public Etre(Localization area)
        {
            var r = new Random();
            this.Him = new HistoricalPNJ()
            {
                Name = NameGenerator.GenGodName(),
                Race = (Race)r.Next(11),
                Sexe = Convert.ToBoolean(r.Next(2))
            };
            this.State = (State)(r.Next(4));
            this.Where = new Localization()
            {
                X = r.Next(area.X) + 10,
                Y = r.Next(area.Y) + 10
            };

            List<char> voy = new List<char>(new char[] { 'A', 'E', 'I', 'O', 'U', 'Y' });
            switch (this.State)
            {
                case State.Endormie:
                case State.Eveillé:
                    this.Where.Name = voy.Contains(this.Him.Name[0]) ? "Temple d'" + this.Him.Name : "Temple de " + this.Him.Name;
                    this.Description = "Toute la magie du monde provient de " + this.Him.Name + ", un être quasi-divin. Aillant trop de puissance en lui elle déborde sur le monde, en se transformant en mana.";
                    break;
                case State.Mort:
                    this.Where.Name = voy.Contains(this.Him.Name[0]) ? "Tombe d'" + this.Him.Name : "Tombe de " + this.Him.Name;
                    this.Description = this.Him.Name + ", un être quasi-divin, voulu mettre un terme au reigne des dieux et se révolta. Malheureusement il en mouru, mais en disparaissent son énergie enveloppa le monde de mana, donnant la possibilité au mortel de continuer son oeuvre.";
                    break;
                case State.Scellé:
                    this.Where.Name = "Sanctuaire de l'oublie";
                    this.Description = this.Him.Name + ", un être quasi-divin, voulu mettre un terme au reigne des dieux et se révolta. Pour le punir, les dieux le déposséda de ses pouvoirs et le donna au mortel. Quant à lui, il fût scellé au milieu d'une prison piégé";
                    break;
            }
        }

        public Etre(string description, HistoricalPNJ him, State state, Localization where) : base (description)
        {
            this.Him = him;
            this.State = state;
            this.Where = where;
        }

        public HistoricalPNJ Him { get => _him; set => _him = value; }
        public State State { get => _state; set => _state = value; }
        public Localization Where { get => _where; set => _where = value; }
    }

    public enum State
    {
        Eveillé,
        Endormie,
        Scellé,
        Mort
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
                        X = r.Next(cadre.X) + 10,
                        Y = r.Next(cadre.Y) + 10,
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
