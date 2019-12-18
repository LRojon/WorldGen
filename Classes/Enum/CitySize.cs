using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGen.Classes.Enum
{
    public enum CitySize
    {
        Hameau,         // Le plus petit souvent absent des cartes
        Village,        // Assez grand pour être sur la carte
        Bourg,          // Village moyen
        Ville,          // Lieux de passage fréquent
        Cité,           // Lieux les plus connue du pays
        Capitale        // Réservé au ville-Capitale
    }
}
