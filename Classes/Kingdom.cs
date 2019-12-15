using System;
using System.Collections.Generic;
using System.Drawing;
using VoronoiLib;
using VoronoiLib.Structures;

namespace WorldGen.Classes
{

    public class Kingdom
    {
        public List<Region> regions = new List<Region>();
        private string name;
        private Color color;

        public Kingdom(Region capital, Color color)
        {
            if(!capital.Capital)
                throw new Exception("La region fourni n'est pas une capitale.");

            this.color = color;
            this.name = "Kingdom";
            capital.Influence = 1;
            createKingdom(capital);
        }

        public string Name { get => name; set => name = value; }
        public Color Color { get => color; set => color = value; }

        private void createKingdom(Region current)
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
                        createKingdom(neighbor);
                    }
                }
            }
        }
    }
}