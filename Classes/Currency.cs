using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using WorldGen.Classes.Enum;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace WorldGen.Classes
{
    class Currency
    {
        private Coin _po;
        private Coin _pa;
        private Coin _pc;

        public Coin Po { get => _po; set => _po = value; }
        public Coin Pa { get => _pa; set => _pa = value; }
        public Coin Pc { get => _pc; set => _pc = value; }
    
        public Currency(Kingdom kingdom)
        {
            this.Po = new Coin(Color.Gold, kingdom);
            this.Pa = new Coin(Color.Silver, kingdom);
            this.Pc = new Coin(Color.FromArgb(72, 45, 20), kingdom);
        }

        public ImageSource GetCurrencyImage()
        {
            var bmp = new Bitmap(655, 250);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                switch (this.Po.Shape)
                {
                    case CurrencyShape.Diamond:
                        PointF[] tmp;
                        if (this.Po.IsPerforated)
                        {
                            tmp = new PointF[]
                            {
                                new PointF(9f, 125f),
                                new PointF(109f, 225f),
                                new PointF(209f, 125f),
                                new PointF(109f, 25f),
                                new PointF(9f, 125f),

                                new PointF(59f, 125f),
                                new PointF(109f, 175f),
                                new PointF(159f, 125f),
                                new PointF(109f, 75f),
                                new PointF(59f, 125f),
                            };
                        }
                        else
                        {
                            tmp = new PointF[]
                            {
                                new PointF(9f, 125f),
                                new PointF(109f, 225f),
                                new PointF(209f, 125f),
                                new PointF(109f, 25f),
                            };
                        }
                        g.FillPolygon(new SolidBrush(this.Po.Color), tmp);
                        break;
                    case CurrencyShape.Round:
                        g.FillEllipse(new SolidBrush(this.Po.Color), 9, 25, 209, 225);
                        if (this.Po.IsPerforated)
                            g.FillEllipse(new System.Drawing.SolidBrush(this.Po.Color), 59, 75, 159, 175);
                        break;
                    case CurrencyShape.Square:
                        g.FillRectangle(new SolidBrush(this.Po.Color), 9, 25, 209, 225);
                        break;
                }
            }


            return ImageSourceFromBitmap(bmp);
        }

        private ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            ImageSource newSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return newSource;
        }

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
            else if(k.Type == KingdomType.Royaume)
            {
                if (this.Color == System.Drawing.Color.Gold)
                    this.Name = NameGenerator.GenCoinName()[0] + " royal";
            }
            else
            {
                var tmp = NameGenerator.GenCoinName();
                this.Name = tmp[0] + tmp[1];
            }
        }
    }

    enum CurrencyShape
    {
        Square,
        Round,
        Diamond,
        Octogone
    }
}
