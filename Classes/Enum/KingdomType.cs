using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGen.Classes.Enum
{
    public enum KingdomType
    {
        Royaume,        // Dirigeant: Roi,                  Nom: Royaume de ----
        Empire,         // Dirigeant: Empereur,             Nom: L'empire <Nom de la race>
        Théocratie,     // Dirigeant: Grand pêtre/prophéte, Nom: L'ordre de <Nom de dieu> OU Nom aléatoire
        Polysynodie     // Dirigeant: Le conseil,           Nom: Nom aléatoire
    }
}
