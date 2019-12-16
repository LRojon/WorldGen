using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGen.Classes.Enum
{
    static class KingdomNameGenerator
    {
        private static string[] KingdomSyllabe = {
            "sha",
            "int",
            "is",
            "xep",
            "an",
            "bui",
            "wer",
            "hui",
            "khou",
            "renth",
            "bek",
            "usa",
            "lui",
            "seon",
            "vru",
            "kian","nix",
            "krob","bola",
            "aba","var",
            "steo","pi","dran",
            "jil","lac",
            "plu","ya","sia",
        };
        private static string[] KingdomName1 =
        {
            "Le pays ",
            "Les terres ",
            "La nation ",
            "Le sanctuaire ",
            "Le domaine ",
            "Le dominion ",
            "L'étendue ",
            "Le nexus ",
            "La province ",
            "Les territoires ",
        };
        private static string[] KingdomName2 =
        {
            "brisé",
            "cendreux",
            "crépusculaire",
            "lumineux",
            "azure",
            "d'automne",
            "de l'été",
            "du printemps",
            "de l'hiver",
            "forgé",
            "brumeux",
            "bouillant",
            "perfide",
            "sauvage",
            "mourant",
            "du délire",
            "endormi",
            "calme",
            "invisible",
            "antique"
        };

        public static string genKingdomName()
        {
            var r = new Random();
            string tmp = "";
            if (r.Next(3) == 0)
            {
                int n = 2;
                for (int i = 0; i < n; i++)
                {
                    r = new Random(r.Next(999999999) + i);
                    var res = KingdomSyllabe[r.Next(KingdomSyllabe.Length)];
                    if (i > 0 && res[0] == tmp[tmp.Length - 1])
                        i++;
                    else
                        tmp += res;
                }
                tmp = Convert.ToChar(tmp[0] - 32) + tmp.Substring(1, tmp.Length - 1);
            }
            else
            {
                tmp += KingdomName1[r.Next(KingdomName1.Length)] + KingdomName2[new Random(r.Next(999999)).Next(KingdomName2.Length)];
            }
            return tmp;
        }
    }
}
