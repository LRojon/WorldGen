using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FortuneVoronoi;
using VoronoiLib;
using VoronoiLib.Structures;

namespace WorldGen.Classes
{
    public class Map
    {
        public List<FortuneSite> sites = new List<FortuneSite>();

        public Map()
        {
        }
        public Map(int widthArea, int heightArea)
        {
            int nbSite = widthArea * heightArea / 400;
            MessageBox.Show("Nombre de site pour ("+ widthArea + "x" + heightArea + "): " + nbSite);
        }
    }
}
