﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGen.Classes.Enum
{
    static class NameGenerator
    {
        private static readonly string[] KingdomSyllabe = {
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
            "kian",
            "nix",
            "krob",
            "bola",
            "aba",
            "var",
            "steo",
            "pi",
            "dran",
            "jil",
            "lac",
            "plu",
            "ya",
            "sia",
        };
        private static readonly string[] KingdomName1 =
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
        private static readonly string[] KingdomName2 =
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

        private static readonly string[] CapitalSyllabe =
        {
            "char",
            "set",
            "chu",
            "phu",
            "tsa",
            "vro",
            "vre",
            "vord",
            "ru",
            "ry",
            "ose",
            "aso",
            "sey",
            "son",
            "ler",
            "ov",
            "le",
            "nom",
            "qro",
            "sa",
            "ede",
            "ka",
            "ico",
            "gan",
            "ita",
        };
        private static readonly string[] CapitalTerm =
        {
            "dale",
            "set",
            "chester",
            "tsa",
            "gate",
            "glia",
            "ford",
            "gend",
            "sey",
            "plais",
            "son",
            "dale",
            "gend",
            "bridge",
            "ler",
            "gate",
            "polis",
            "ledo",
            "chester",
            "qro",
            "ford",
            "owin",
            "bridge",
            "ico",
            "polis",
            "gan",
        };

        public static string GenKingdomName(int seed = 0)
        {
            Random r;
            if (seed == 0)
                r = new Random();
            else
                r = new Random(seed);

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

        public static string GenCapitalName(int seed = 0)
        {
            GC.Collect();
            var tmp = "";
            Random r;
            if (seed == 0)
                r = new Random();
            else
                r = new Random(seed);

            if (r.Next(2) == 48)
            {
                var t1 = CapitalSyllabe[r.Next(CapitalSyllabe.Length)];
                var t2 = CapitalSyllabe[new Random(r.Next(999999)).Next(CapitalSyllabe.Length)];
                if (t1 == t2)
                    tmp += t1;
                else
                    tmp += t1 + t2;
            }
            else
            {
                tmp += CapitalSyllabe[r.Next(CapitalSyllabe.Length)];
            }

            tmp += CapitalTerm[r.Next(CapitalTerm.Length)];

            tmp = Convert.ToChar(tmp[0] - 32) + tmp.Substring(1, tmp.Length - 1);
            return tmp;
        }

        public static string GenCityName()
        {
            return "";
        }
    }
}
