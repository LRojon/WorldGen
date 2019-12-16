using System;
using System.Collections.Generic;
using System.Drawing;
using WorldGen.Classes.Enum;
using VoronoiLib;
using VoronoiLib.Structures;

namespace WorldGen.Classes
{

    public class Kingdom
    {
        public List<Region> regions = new List<Region>();
        private string _name;
        private KingdomType _type;
        private Color _color;
        private string _leader;

        public Kingdom(Region capital, Color color)
        {
            if(!capital.Capital)
                throw new Exception("La region fourni n'est pas une capitale.");

            this.Color = color;
            this.Type = (KingdomType)new Random().Next(4);
            this.Name = "Kingdom";
            this.Leader = "Roi";

            capital.Influence = 1;
            createKingdom(capital);
        }

        public string Name { get => _name; set => _name = value; }
        public Color Color { get => _color; set => _color = value; }
        public KingdomType Type { get => _type; set => _type = value; }
        public string Leader { get => _leader; set => _leader = value; }

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