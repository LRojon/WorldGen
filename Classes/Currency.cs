using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using WorldGen.Classes.Enum;

namespace WorldGen.Classes
{
    class Currency
    {

    }

    class Coin
    {
        private bool _isPerforated;
        private CurrencyShape _shape;
        private Color _color;
        private string _name;

        public bool IsPerforated { get => _isPerforated; set => _isPerforated = value; }
        public Color Color { get => _color; set => _color = value; }
        public string Name { get => _name; set => _name = value; }
        public CurrencyShape Shape { get => _shape; set => _shape = value; }

        public Coin(Color color, Kingdom k)
        {
            var r = new Random();
            this.IsPerforated = Convert.ToBoolean(r.Next(2));
            this.Color = color;
            this.Shape = (CurrencyShape)r.Next(3);

            if(k.Type == KingdomType.Empire)
            {
                if (this.Color == System.Drawing.Color.Gold)
                    this.Name = NameGenerator.GenCoinName()[0] + " impériaux";
            }
        }
    }

    enum CurrencyShape
    {
        Square,
        Round,
        Diamond,
    }
}
