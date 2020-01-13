using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGen.Classes.Enum;

namespace WorldGen.Classes
{
    public class HistoricalPNJ
    {
        private string _name;
        private Race _race;
        private bool _sexe;

        public string Name { get => _name; set => _name = value; }
        public Race Race { get => _race; set => _race = value; }
        public bool Sexe { get => _sexe; set => _sexe = value; }
    }
}
