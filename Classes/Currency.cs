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
            var r = new Random();
            this.Po = new Coin(Color.Gold, kingdom, new Random(r.Next(999999)));
            this.Pa = new Coin(Color.Silver, kingdom, new Random(r.Next(999999)));
            this.Pc = new Coin(Color.FromArgb(72, 45, 20), kingdom, new Random(r.Next(999999)));
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
                        if (this.Po.IsPerforated)
                            g.DrawEllipse(new System.Drawing.Pen(this.Po.Color, 50), 39, 55, 139, 155);
                        else
                            g.FillEllipse(new SolidBrush(this.Po.Color), 9, 25, 209, 225);
                        break;
                    case CurrencyShape.Square:
                        if (this.Po.IsPerforated)
                        {
                            tmp = new PointF[]
                            {
                                new PointF(9, 25),
                                new PointF(209, 25),
                                new PointF(209, 225),
                                new PointF(9, 225),
                                new PointF(9, 25),

                                new PointF(59, 75),
                                new PointF(159, 75),
                                new PointF(159, 175),
                                new PointF(59, 175),
                                new PointF(59, 75),
                            };
                            g.FillPolygon(new SolidBrush(this.Po.Color), tmp);
                        }
                        else
                        {
                            g.FillRectangle(new SolidBrush(this.Po.Color), 9, 25, 209, 225);
                        }
                        break;
                }
                switch (this.Pa.Shape)
                {
                    case CurrencyShape.Diamond:
                        PointF[] tmp;
                        if (this.Pa.IsPerforated)
                        {
                            tmp = new PointF[]
                            {
                                new PointF(9f + 218, 125f),
                                new PointF(109f + 218, 225f),
                                new PointF(209f + 218, 125f),
                                new PointF(109f + 218, 25f),
                                new PointF(9f + 218, 125f),

                                new PointF(59f + 218, 125f),
                                new PointF(109f + 218, 175f),
                                new PointF(159f + 218, 125f),
                                new PointF(109f + 218, 75f),
                                new PointF(59f + 218, 125f),
                            };
                        }
                        else
                        {
                            tmp = new PointF[]
                            {
                                new PointF(9f + 218, 125f),
                                new PointF(109f + 218, 225f),
                                new PointF(209f + 218, 125f),
                                new PointF(109f + 218, 25f),
                            };
                        }
                        g.FillPolygon(new SolidBrush(this.Pa.Color), tmp);
                        break;
                    case CurrencyShape.Round:
                        if (this.Pa.IsPerforated)
                            g.DrawEllipse(new System.Drawing.Pen(this.Pa.Color, 50), 39 + 218, 55, 139, 155);
                        else
                            g.FillEllipse(new SolidBrush(this.Pa.Color), 9 + 218, 25, 209, 225);
                        break;
                    case CurrencyShape.Square:
                        if (this.Pa.IsPerforated)
                        {
                            tmp = new PointF[]
                            {
                                new PointF(9 + 218, 25),
                                new PointF(209 + 218, 25),
                                new PointF(209 + 218, 225),
                                new PointF(9 + 218, 225),
                                new PointF(9 + 218, 25),

                                new PointF(59 + 218, 75),
                                new PointF(159 + 218, 75),
                                new PointF(159 + 218, 175),
                                new PointF(59 + 218, 175),
                                new PointF(59 + 218, 75),
                            };
                            g.FillPolygon(new SolidBrush(this.Pa.Color), tmp);
                        }
                        else
                        {
                            g.FillRectangle(new SolidBrush(this.Pa.Color), 9 + 218, 25, 209, 225);
                        }
                        break;
                }
                switch (this.Pc.Shape)
                {
                    case CurrencyShape.Diamond:
                        PointF[] tmp;
                        if (this.Pc.IsPerforated)
                        {
                            tmp = new PointF[]
                            {
                                new PointF(9f + 436, 125f),
                                new PointF(109f + 436, 225f),
                                new PointF(209f + 436, 125f),
                                new PointF(109f + 436, 25f),
                                new PointF(9f + 436, 125f),

                                new PointF(59f + 436, 125f),
                                new PointF(109f + 436, 175f),
                                new PointF(159f + 436, 125f),
                                new PointF(109f + 436, 75f),
                                new PointF(59f + 436, 125f),
                            };
                        }
                        else
                        {
                            tmp = new PointF[]
                            {
                                new PointF(9f + 436, 125f),
                                new PointF(109f + 436, 225f),
                                new PointF(209f + 436, 125f),
                                new PointF(109f + 436, 25f),
                            };
                        }
                        g.FillPolygon(new SolidBrush(this.Pc.Color), tmp);
                        break;
                    case CurrencyShape.Round:
                        if (this.Pc.IsPerforated)
                            g.DrawEllipse(new System.Drawing.Pen(this.Pc.Color, 50), 39 + 436, 55, 139, 155);
                        else
                            g.FillEllipse(new SolidBrush(this.Pc.Color), 9 + 436, 25, 209, 225);
                        break;
                    case CurrencyShape.Square:
                        if (this.Pc.IsPerforated)
                        {
                            tmp = new PointF[]
                            {
                                new PointF(9 + 436, 25),
                                new PointF(209 + 436, 25),
                                new PointF(209 + 436, 225),
                                new PointF(9 + 436, 225),
                                new PointF(9 + 436, 25),

                                new PointF(59 + 436, 75),
                                new PointF(159 + 436, 75),
                                new PointF(159 + 436, 175),
                                new PointF(59 + 436, 175),
                                new PointF(59 + 436, 75),
                            };
                            g.FillPolygon(new SolidBrush(this.Pc.Color), tmp);
                        }
                        else
                        {
                            g.FillRectangle(new SolidBrush(this.Pc.Color), 9 + 436, 25, 209, 225);
                        }
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

        public Coin(Color color, Kingdom k, Random r)
        {
            this.IsPerforated = Convert.ToBoolean(r.Next(2));
            this.Color = color;
            this.Shape = (CurrencyShape)r.Next(3);

            if(k.Type == KingdomType.Empire)
            {
                if (this.Color == Color.Gold)
                {
                    this.Name = NameGenerator.GenCoinName(r.Next(999999))[0] + " impérial";
                }
                else
                {
                    var tmp = NameGenerator.GenCoinName(r.Next(999999));
                    this.Name = tmp[0] + tmp[1];
                }
            }
            else if(k.Type == KingdomType.Royaume)
            {
                if (this.Color == Color.Gold)
                {
                    this.Name = NameGenerator.GenCoinName(r.Next(999999))[0] + " royal";
                }
                else
                {
                    var tmp = NameGenerator.GenCoinName(r.Next(999999));
                    this.Name = tmp[0] + tmp[1];
                }
            }
            else
            {
                var tmp = NameGenerator.GenCoinName(r.Next(999999));
                this.Name = tmp[0] + tmp[1];
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
