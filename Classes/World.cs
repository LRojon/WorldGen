using LibNoise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Numerics;
using System.Resources;
using System.Threading;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using FortuneVoronoi;
using HandyCollections.Extensions;
using VoronoiLib;
using VoronoiLib.Structures;
using Color = System.Drawing.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Pen = System.Drawing.Pen;
using Point = System.Drawing.Point;
using Math = System.Math;
using WorldGen.Classes.Enum;
using Pathfindax.PathfindEngine;
using Pathfindax.Factories;
using Pathfindax.Graph;
using Pathfindax.Algorithms;
using Pathfindax.Utils;
using Pathfindax.Nodes;
using Pathfindax.Collections;
using Pathfindax.Paths;
using RoyT.AStar;

namespace WorldGen.Classes
{
    [Serializable]
    public class World
    {
        // Age du monde

        public List<FortuneSite> sites;
        public List<Region> cities;
        public List<Kingdom> kingdoms;
        private LinkedList<VEdge> vedges;
        private List<Color> colors;
        public Pantheon pantheon;
        private int mapWidth;
        private int mapHeight;
        public bool voronoi;
        public bool affSite;
        public bool croyance;
        public bool templeDungeon;
        private double[,] perlinMap;
        private Bitmap mapPerlin;
        public string name;
        public SourceOfMagic source;

        public World()
        {
        }
        public World(int widthArea, int heightArea)
        {
            regen:

            this.sites = new List<FortuneSite>();
            this.cities = new List<Region>();
            this.kingdoms = new List<Kingdom>();
            this.colors = new List<Color>();
            this.voronoi = false;
            this.affSite = true;
            this.croyance = false;
            this.templeDungeon = true;
            this.pantheon = new Pantheon();

            this.mapHeight = heightArea;
            this.mapWidth = widthArea;
            Random rand = new Random();
            int nbSite = widthArea * heightArea / 1000; // default: /400
            this.perlinMap = new double[widthArea, heightArea];
            //nbSite = 30;

            this.colors.Add(Color.Red);
            this.colors.Add(Color.Black);
            this.colors.Add(Color.Purple);
            this.colors.Add(Color.Gray);
            this.colors.Add(Color.Cyan);

            this.name = NameGenerator.GenWorldName();

            Perlin p = new Perlin
            {
                Persistence = 0.65,
                Frequency = .003333,
                OctaveCount = 8,
                Seed = /*628594615;*/new Random().Next(999999999)
            };

            for (int i = 0; i < widthArea; i++)
            {
                for (int j = 0; j < heightArea; j++)
                {
                    perlinMap[i, j] = p.GetValue(i, j, 0);
                }
            }

            for (int i = 0; i < nbSite; i++)
            {
                double cadre = 50d;
                var x = rand.NextDouble() * widthArea;
                var y = rand.NextDouble() * heightArea;
                var tmp = new Region(x, y, perlinMap);
                if (!(x < this.mapWidth - cadre && x > cadre &&
                    y < this.mapHeight - cadre && y > cadre))
                {
                    tmp.City = false;
                }
                sites.Add(tmp);
                if (tmp.City)
                    cities.Add(tmp);
            }

            if (this.cities.Count == 0)
            {
                this.Empty();
                goto regen;
            }

            this.vedges = FortunesAlgorithm.Run(ref sites, 0, 0, widthArea, heightArea);

            // Génération Pays
            int nbCapital = 5;
            while (nbCapital > 0)
            {
                var tmp = cities.ElementAt(new Random().Next(cities.Count));
                if (!tmp.Capital)
                {
                    var r = new Random().Next(this.colors.Count);
                    tmp.Capital = true;
                    this.kingdoms.Add(new Kingdom(tmp, this.colors.ElementAt(r), this.pantheon));
                    tmp.GenRegion(this.perlinMap, r + this.colors.ElementAt(r).ToArgb());
                    this.colors.RemoveAt(r);
                    nbCapital--;
                }
            }

            List<string> verifKingdomName = new List<string>();
            foreach (Kingdom k in this.kingdoms)
            {
                if (verifKingdomName.Contains(k.Name))
                    k.Name = NameGenerator.GenKingdomName();

                verifKingdomName.Add(k.Name);
                k.Purge();
            }

            int n = 0;
            foreach(Region c in cities)
            {
                if (!c.Capital)
                    c.GenRegion(this.perlinMap, new Random().Next(999999) + n);

                n++;
            }

            //Génération des religions
            var pTmp = this.pantheon.Gods.Where(g => !g.Forgot).ToList();
            foreach (Kingdom k in this.kingdoms.Where(e => e.Type == KingdomType.Théocratie))
            {
                var id = rand.Next(pTmp.Count);

                var r = pTmp.ElementAt(id);

                k.God = r;
                r.Capitale = k.Capital;
                r.Followers.Add(k.Capital);
                k.Capital.God = r;
                k.Capital.GodInfluence = 1;

                pTmp.RemoveAt(id);
            }

            foreach (God g in pTmp)
            {
                var tmp = this.cities.Where(c => c.Kingdom != null ? c.Kingdom.Type != KingdomType.Théocratie : true).ToList();
                var r = tmp.ElementAt(rand.Next(tmp.Count));
                r.GodInfluence = 1;
                r.God = g;
                g.Capitale = r;
                g.Followers.Add(r);
            }

            // foreach region avec 1 de GodInfluence expendre en -0.1 jusqu'a rencontrer une influence equivalente ou atteindre 0.1

            foreach(Region r in this.sites)
            {
                if (r.GodInfluence == 1)
                {
                    var g = this.pantheon.Gods.Where(e => e == r.God).FirstOrDefault();
                    g.GenInfluence(r);
                }
            }

            foreach (FortuneSite site in sites)
            {
                foreach (VEdge vedge in vedges)
                {
                    if (vedge.Left == site || vedge.Right == site)
                        if (!site.Cell.Contains(vedge))
                            site.Cell.Add(vedge);
                }
            }

            var area = new Localization()
            {
                X = this.mapWidth - 20,
                Y = this.mapHeight - 20
            };
            switch(rand.Next(5))
            {
                case 0:
                    this.source = new Don(this.pantheon);
                    break;
                case 1:
                    this.source = new Plan();
                    break;
                case 2:
                    this.source = new Titan(this.pantheon);
                    break;
                case 3:
                    this.source = new Etre(area);
                    break;
                case 4:
                    this.source = new Artefacts(area);
                    break;
                default:
                    break;
            }

            if (this.source is Artefacts)
                foreach (Artefact a in ((Artefacts)this.source).List)
                    a.Position.Underwater = !(this.perlinMap[a.Position.X, a.Position.Y] > 0);
            if (this.source is Etre)
                ((Etre)this.source).Where.Underwater = !(this.perlinMap[((Etre)this.source).Where.X, ((Etre)this.source).Where.Y] > 0);

        }

        public ImageSource GetVoronoiGraph(bool delaunay = false)
        {
            if (mapPerlin == null)
            {
                GenPerlinMap();
            }
            Bitmap bmp = new Bitmap(mapPerlin);


            using (Graphics g = Graphics.FromImage(bmp))
            {
                int seed = -1;

                foreach (Region site in sites)
                {
                    seed++;

                    if (site.Kingdom != null && affSite)
                    {
                        List<VEdge> tmp = new List<VEdge>();
                        int i = 0;
                        tmp.Add(site.Cell.First());
                        site.Cell.Remove(site.Cell.First());
                        while (!site.Cell.IsEmpty() && i < site.Cell.Count)
                        {
                            VEdge ve = site.Cell.ElementAt(i);
                            VEdge t = tmp.Last();
                            if (t.End == ve.Start)
                            {
                                tmp.Add(ve);
                                site.Cell.Remove(ve);
                                i = 0;
                            }
                            else if (t.End == ve.End)
                            {
                                VPoint vp = ve.End;
                                ve.End = ve.Start;
                                ve.Start = vp;

                                tmp.Add(ve);
                                site.Cell.Remove(ve);
                                i = 0;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        site.Cell = tmp;

                        List<Point> points = new List<Point>();
                        foreach (var edge in site.Cell)
                        {
                            points.Add(new Point((int)edge.Start.X, (int)edge.Start.Y));
                            points.Add(new Point((int)edge.End.X, (int)edge.End.Y));
                        }

                        Point[] tab = points.ToArray();
                        Random rand = new Random(seed);
                        Color color = Color.FromArgb(90,
                            site.Kingdom.Color.R,
                            site.Kingdom.Color.G,
                            site.Kingdom.Color.B);


                        g.FillPolygon(new SolidBrush(color), tab);

                    }

                    if (site.God != null && this.croyance)
                    {
                        List<VEdge> tmp = new List<VEdge>();
                        int i = 0;
                        tmp.Add(site.Cell.First());
                        site.Cell.Remove(site.Cell.First());
                        while (!site.Cell.IsEmpty() && i < site.Cell.Count)
                        {
                            VEdge ve = site.Cell.ElementAt(i);
                            VEdge t = tmp.Last();
                            if (t.End == ve.Start)
                            {
                                tmp.Add(ve);
                                site.Cell.Remove(ve);
                                i = 0;
                            }
                            else if (t.End == ve.End)
                            {
                                VPoint vp = ve.End;
                                ve.End = ve.Start;
                                ve.Start = vp;

                                tmp.Add(ve);
                                site.Cell.Remove(ve);
                                i = 0;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        site.Cell = tmp;

                        List<Point> points = new List<Point>();
                        foreach (var edge in site.Cell)
                        {
                            points.Add(new Point((int)edge.Start.X, (int)edge.Start.Y));
                            points.Add(new Point((int)edge.End.X, (int)edge.End.Y));
                        }

                        Point[] tab = points.ToArray();
                        Random rand = new Random(seed);
                        Color color = Color.FromArgb(90,
                            site.God.Color.R,
                            site.God.Color.G,
                            site.God.Color.B);


                        g.FillPolygon(new SolidBrush(color), tab);
                    }


                    if (site.City)
                    {
                        if (!site.Capital)
                            g.FillEllipse(new SolidBrush(Color.Crimson),
                                (float)(site.X - 2), (float)(site.Y - 2),
                                5f, 5f);
                        else
                            g.FillEllipse(new SolidBrush(Color.Yellow),
                                (float)(site.X - 2), (float)(site.Y - 2),
                                5f, 5f);
                    }

                    if (delaunay)
                    {
                        foreach (var neighbor in site.Neighbors)
                        {
                            g.DrawLine(new System.Drawing.Pen(Color.Blue),
                                (float)site.X,
                                (float)site.Y,
                                (float)neighbor.X,
                                (float)neighbor.Y);
                        }
                    }
                }

                if (this.templeDungeon)
                {
                    AffTempleDungeon(g);
                }

                if (voronoi)
                {
                    foreach (VEdge vedge in vedges)
                    {
                        g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black),
                            new System.Drawing.Point((int)vedge.Start.X, (int)vedge.Start.Y),
                            new System.Drawing.Point((int)vedge.End.X, (int)vedge.End.Y));
                    }
                }
            }

            return ImageSourceFromBitmap(bmp);
        }
        public ImageSource GetVoronoiGraph(System.Windows.Point position,bool delaunay = false)
        {

            if (mapPerlin == null)
            {
                GenPerlinMap();
            }
            Bitmap bmp = new Bitmap(mapPerlin);


            using (Graphics g = Graphics.FromImage(bmp))
            {
                int seed = -1;
                Region info = (Region)this.sites.First();
                double distMin = this.mapWidth*2;
                
                foreach (Region site in this.sites)
                {
                    seed++;

                    if (site.Kingdom != null && this.affSite)
                    {
                        List<VEdge> tmp = new List<VEdge>();
                        int i = 0;
                        tmp.Add(site.Cell.First());
                        site.Cell.Remove(site.Cell.First());
                        while (!site.Cell.IsEmpty() && i < site.Cell.Count)
                        {
                            VEdge ve = site.Cell.ElementAt(i);
                            VEdge t = tmp.Last();
                            if (t.End == ve.Start)
                            {
                                tmp.Add(ve);
                                site.Cell.Remove(ve);
                                i = 0;
                            }
                            else if (t.End == ve.End)
                            {
                                VPoint vp = ve.End;
                                ve.End = ve.Start;
                                ve.Start = vp;

                                tmp.Add(ve);
                                site.Cell.Remove(ve);
                                i = 0;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        site.Cell = tmp;

                        List<Point> points = new List<Point>();
                        foreach (var edge in site.Cell)
                        {
                            points.Add(new Point((int)edge.Start.X, (int)edge.Start.Y));
                            points.Add(new Point((int)edge.End.X, (int)edge.End.Y));
                        }

                        Point[] tab = points.ToArray();
                        Random rand = new Random(seed);
                        Color color = Color.FromArgb(90,
                            site.Kingdom.Color.R,
                            site.Kingdom.Color.G,
                            site.Kingdom.Color.B);


                        g.FillPolygon(new SolidBrush(color), tab);

                    }

                    if(site.God != null && this.croyance)
                    {
                        List<VEdge> tmp = new List<VEdge>();
                        int i = 0;
                        tmp.Add(site.Cell.First());
                        site.Cell.Remove(site.Cell.First());
                        while (!site.Cell.IsEmpty() && i < site.Cell.Count)
                        {
                            VEdge ve = site.Cell.ElementAt(i);
                            VEdge t = tmp.Last();
                            if (t.End == ve.Start)
                            {
                                tmp.Add(ve);
                                site.Cell.Remove(ve);
                                i = 0;
                            }
                            else if (t.End == ve.End)
                            {
                                VPoint vp = ve.End;
                                ve.End = ve.Start;
                                ve.Start = vp;

                                tmp.Add(ve);
                                site.Cell.Remove(ve);
                                i = 0;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        site.Cell = tmp;

                        List<Point> points = new List<Point>();
                        foreach (var edge in site.Cell)
                        {
                            points.Add(new Point((int)edge.Start.X, (int)edge.Start.Y));
                            points.Add(new Point((int)edge.End.X, (int)edge.End.Y));
                        }

                        Point[] tab = points.ToArray();
                        Random rand = new Random(seed);
                        Color color = Color.FromArgb(90,
                            site.God.Color.R,
                            site.God.Color.G,
                            site.God.Color.B);


                        g.FillPolygon(new SolidBrush(color), tab);
                    }

                    double dist = Math.Sqrt(Math.Pow(site.X - position.X, 2) + Math.Pow(site.Y - position.Y, 2));
                    if (distMin > dist)
                    {
                        info = site;
                        distMin = dist;
                    }

                    if (site.City)
                    {
                        if (!site.Capital)
                            g.FillEllipse(new SolidBrush(Color.Crimson),
                                (float)(site.X - 2), (float)(site.Y - 2),
                                5f, 5f);
                        else
                            g.FillEllipse(new SolidBrush(Color.Yellow),
                                (float)(site.X - 2), (float)(site.Y - 2),
                                5f, 5f);
                    }

                    if (delaunay)
                    {
                        foreach (var neighbor in site.Neighbors)
                        {
                            g.DrawLine(new System.Drawing.Pen(Color.Blue),
                                (float)site.X,
                                (float)site.Y,
                                (float)neighbor.X,
                                (float)neighbor.Y);
                        }
                    }

                }

                if (info != null && (this.croyance || this.affSite))
                {
                    string str = "";
                    if (this.affSite)
                    {
                        if (info.Kingdom != null)
                            str = info.Kingdom.Name;
                    }
                    else if (this.croyance)
                    {
                        if (info.God != null)
                            str = info.God.ToString();
                    }

                    Font font = new Font(System.Drawing.FontFamily.GenericMonospace, 12f, GraphicsUnit.Pixel);

                    var width = str.Length * 7.5f;

                    g.FillRectangle(new SolidBrush(Color.White),
                        (position.X < this.mapWidth - width ? (float)(position.X) : (float)(position.X - width * 2)),
                        (float)(position.Y - 10), width, font.Size + 2);
                    g.DrawString(str,
                        font,
                        new SolidBrush(Color.Black),
                        (position.X < this.mapWidth - width ? (float)(position.X) : (float)(position.X - width * 2)), (float)(position.Y - 10));

                }

                if (this.templeDungeon)
                {
                    AffTempleDungeon(g, position);
                }

                if (voronoi)
                {
                    foreach (VEdge vedge in vedges)
                    {
                        g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black),
                            new System.Drawing.Point((int)vedge.Start.X, (int)vedge.Start.Y),
                            new System.Drawing.Point((int)vedge.End.X, (int)vedge.End.Y));
                    }
                }
            }

            return ImageSourceFromBitmap(bmp);
        }
        public ImageSource GetKingdomMap(string kingdomName)
        {
            if (mapPerlin == null)
            {
                GenPerlinMap();
            }
            Bitmap bmp = new Bitmap(mapPerlin);

            Kingdom currentK = this.kingdoms.Where(e => e.Name == kingdomName).First();

            using (Graphics g = Graphics.FromImage(bmp))
            {
                List<Point> frontier = new List<Point>();

                foreach (Region site in currentK.regions)
                {
                    List<VEdge> tmp = new List<VEdge>();
                    int i = 0;
                    tmp.Add(site.Cell.First());
                    site.Cell.Remove(site.Cell.First());
                    while (!site.Cell.IsEmpty() && i < site.Cell.Count)
                    {
                        VEdge ve = site.Cell.ElementAt(i);
                        VEdge t = tmp.Last();
                        if (t.End == ve.Start)
                        {
                            tmp.Add(ve);
                            site.Cell.Remove(ve);
                            i = 0;
                        }
                        else if (t.End == ve.End)
                        {
                            VPoint vp = ve.End;
                            ve.End = ve.Start;
                            ve.Start = vp;

                            tmp.Add(ve);
                            site.Cell.Remove(ve);
                            i = 0;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    site.Cell = tmp;

                    List<Point> points = new List<Point>();
                    foreach (var edge in site.Cell)
                    {
                        points.Add(new Point((int)edge.Start.X, (int)edge.Start.Y));
                        points.Add(new Point((int)edge.End.X, (int)edge.End.Y));
                    }

                    Point[] tab = points.ToArray();
                    Color color = Color.FromArgb(150,
                        site.Kingdom.Color.R,
                        site.Kingdom.Color.G,
                        site.Kingdom.Color.B);


                    g.FillPolygon(new SolidBrush(color), tab);

                    if (site.City)
                    {
                        if (!site.Capital)
                            g.FillEllipse(new SolidBrush(Color.White),
                                (float)(site.X - 2), (float)(site.Y - 2),
                                5f, 5f);
                        else
                            g.FillEllipse(new SolidBrush(Color.Yellow),
                                (float)(site.X - 2), (float)(site.Y - 2),
                                5f, 5f);
                    }
                }

                g.FillRectangle(new SolidBrush(Color.FromArgb(50, 255, 255, 255)), 0, 0, this.mapWidth, this.mapHeight);
            }

            return ImageSourceFromBitmap(bmp);
        }
        public ImageSource GetGodMap(string godString)
        {
            God currentG = this.pantheon.Gods.Where(e => e.ToString() == godString).First();
            if (!currentG.Forgot)
            {
                if (mapPerlin == null)
                {
                    GenPerlinMap();
                }
                Bitmap bmp = new Bitmap(mapPerlin);


                using (Graphics g = Graphics.FromImage(bmp))
                {
                    foreach (Region site in currentG.Followers)
                    {
                        List<VEdge> tmp = new List<VEdge>();
                        int i = 0;
                        tmp.Add(site.Cell.First());
                        site.Cell.Remove(site.Cell.First());
                        while (!site.Cell.IsEmpty() && i < site.Cell.Count)
                        {
                            VEdge ve = site.Cell.ElementAt(i);
                            VEdge t = tmp.Last();
                            if (t.End == ve.Start)
                            {
                                tmp.Add(ve);
                                site.Cell.Remove(ve);
                                i = 0;
                            }
                            else if (t.End == ve.End)
                            {
                                VPoint vp = ve.End;
                                ve.End = ve.Start;
                                ve.Start = vp;

                                tmp.Add(ve);
                                site.Cell.Remove(ve);
                                i = 0;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        site.Cell = tmp;

                        List<Point> points = new List<Point>();
                        foreach (var edge in site.Cell)
                        {
                            points.Add(new Point((int)edge.Start.X, (int)edge.Start.Y));
                            points.Add(new Point((int)edge.End.X, (int)edge.End.Y));
                        }

                        Point[] tab = points.ToArray();
                        Color color = Color.FromArgb(150,
                            site.God.Color.R,
                            site.God.Color.G,
                            site.God.Color.B);


                        g.FillPolygon(new SolidBrush(color), tab);

                        if (site.City)
                        {
                            if (site == site.God.Capitale)
                                g.FillEllipse(new SolidBrush(Color.White),
                                    (float)(site.X - 2), (float)(site.Y - 2),
                                    5f, 5f);
                            else
                                g.FillEllipse(new SolidBrush(Color.Yellow),
                                    (float)(site.X - 2), (float)(site.Y - 2),
                                    5f, 5f);
                        }
                    }

                    g.FillRectangle(new SolidBrush(Color.FromArgb(50, 255, 255, 255)), 0, 0, this.mapWidth, this.mapHeight);
                }

                return ImageSourceFromBitmap(bmp);
            }
            else
            {
                return this.ImageSourceFromBitmap((Bitmap)Bitmap.FromFile("Assets/Background/BackgroundTemple.jpg"));
            }
        }

        private ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            ImageSource newSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return newSource;
        }

        private void Empty()
        {
            this.sites = null;
            this.cities = null;
            this.kingdoms = null;
            this.vedges = null;
            this.colors = null;
            this.mapWidth = 0;
            this.mapHeight = 0;
            this.voronoi = false;
            this.affSite = false;
            this.croyance = false;
            this.perlinMap =null;
            this.mapPerlin = null;
            this.name = "";
        }

        private void AffTempleDungeon(Graphics g)
        {
            if (this.source is Artefacts)
            {
                foreach (Artefact a in ((Artefacts)this.source).List)
                {
                    Point[] tmp = {new Point(a.Position.X - 3, a.Position.Y),
                                new Point(a.Position.X, a.Position.Y + 3),
                                new Point(a.Position.X + 3, a.Position.Y),
                                new Point(a.Position.X, a.Position.Y - 3)
                            };
                    g.FillPolygon(new SolidBrush(Color.DarkGray), tmp);
                }
            }
            if (this.source is Etre)
            {
                var a = ((Etre)this.source);
                Point[] tmp = {new Point(a.Where.X - 7, a.Where.Y),
                                new Point(a.Where.X, a.Where.Y + 7),
                                new Point(a.Where.X + 7, a.Where.Y),
                                new Point(a.Where.X, a.Where.Y - 7)
                            };
                g.FillPolygon(new SolidBrush(Color.DarkGray), tmp);
            }
        }
        private void AffTempleDungeon(Graphics g, System.Windows.Point mousePos)
        {
            if (this.source is Artefacts)
            {
                foreach (Artefact a in ((Artefacts)this.source).List)
                {
                    Point[] tmp = {new Point(a.Position.X - 3, a.Position.Y),
                                new Point(a.Position.X, a.Position.Y + 3),
                                new Point(a.Position.X + 3, a.Position.Y),
                                new Point(a.Position.X, a.Position.Y - 3)
                            };
                    g.FillPolygon(new SolidBrush(Color.DarkGray), tmp);
                }
            }
            if (this.source is Etre)
            {
                var a = ((Etre)this.source);
                Point[] tmp = { new Point(a.Where.X - 7 , a.Where.Y),
                                new Point(a.Where.X, a.Where.Y + 7),
                                new Point(a.Where.X + 7, a.Where.Y),
                                new Point(a.Where.X, a.Where.Y - 7)
                            };
                g.FillPolygon(new SolidBrush(Color.DarkGray), tmp);

                if (mousePos.X - 50 >= a.Where.X - 10 && mousePos.X - 50 <= a.Where.X + 10 &&
                    mousePos.Y >= a.Where.Y - 10 && mousePos.Y <= a.Where.Y + 10)
                {
                    var str = a.Where.Name;
                    Font font = new Font(System.Drawing.FontFamily.GenericMonospace, 12f, GraphicsUnit.Pixel);

                    var width = str.Length * 7.5f;
                    g.FillRectangle(new SolidBrush(Color.White),
                        (float)(mousePos.X),
                        (float)(mousePos.Y - 10), width, font.Size + 2);
                    g.DrawString(str,
                        font,
                        new SolidBrush(Color.Black),
                        (float)(mousePos.X), (float)(mousePos.Y - 10));

                }
            }
        }

        private void GenPerlinMap()
        {
            Random r = new Random();
            Image img = Image.FromFile("Assets\\gradient.bmp");
            Bitmap gradient = new Bitmap(img);
            mapPerlin = new Bitmap(mapWidth, mapHeight);
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    double value = perlinMap[i, j];
                    double t = (value + 1 > 2 ? 2 : (value + 1 < 0 ? 0 : value + 1));
                    int lvl = (int)((t / 2) * 255);
                    Color c = gradient.GetPixel(lvl, 0);
                    mapPerlin.SetPixel(i, j, c);
                }
            }
            
            // Génération des routes
            var grid = new Grid(this.mapWidth, this.mapHeight, 1.0f);
            for(int x = 0; x < this.mapWidth; x++)
            {
                for (int y = 0; y < this.mapHeight; y++)
                {
                    if (this.perlinMap[x, y] > 0.05)
                        grid.SetCellCost(new Position(x, y), ((float)this.perlinMap[x, y] * 100) > 1 ? ((float)this.perlinMap[x, y] * 100) : 1f);
                    else
                        grid.BlockCell(new Position(x, y));
                }
            }
            foreach(Region c in this.cities)
            {
                foreach(Region n in c.Neighbors)
                {
                    if(n.City)
                    {
                        Position[] path = grid.GetPath(new Position((int)c.X, (int)c.Y), new Position((int)n.X, (int)n.Y), MovementPatterns.Full, 10000);

                        foreach (Position p in path)
                        {
                            this.mapPerlin.SetPixel(p.X, p.Y, Color.LightGray);// Color.FromArgb(255, 142, 84, 52));
                        }
                    }
                }
            }
        }

        private void GenRiver(int x, int y, int direction = 45)
        {
            GC.Collect();
            if (this.perlinMap[x, y] > 0)
            {
                this.mapPerlin.SetPixel(x, y, Color.Fuchsia); 
                int[] neighbor;
                switch (direction)
                {
                    case 0:
                        neighbor = this.RandTab(new int[] { 0, 0, 0, 0, 1, 2, 3 });
                        break;
                    case 1:
                        neighbor = this.RandTab(new int[] { 0, 1, 1, 1, 1, 2, 3 });
                        break;
                    case 2:
                        neighbor = this.RandTab(new int[] { 0, 1, 2, 2, 2, 2, 3 });
                        break;
                    case 3:
                        neighbor = this.RandTab(new int[] { 0, 1, 2, 3, 3, 3, 3 });
                        break;
                    default:
                        neighbor = this.RandTab(new int[] { 0, 1, 2, 3 });
                        break;
                }
                
                foreach(int i in neighbor)
                {
                    switch(i)
                    {
                        case 0:
                            if (perlinMap[x, y - 1] <= Math.Round(perlinMap[x, y], 8) && this.mapPerlin.GetPixel(x, y - 1) != Color.Fuchsia && this.mapPerlin.GetPixel(x, y - 2) != Color.Fuchsia)
                                GenRiver(x, y - 1, 0);
                            break;
                        case 1:
                            if (perlinMap[x + 1, y] <= Math.Round(perlinMap[x, y], 8) && this.mapPerlin.GetPixel(x + 1, y) != Color.Fuchsia && this.mapPerlin.GetPixel(x + 2, y) != Color.Fuchsia)
                                GenRiver(x + 1, y, 1);
                            break;
                        case 2:
                            if (perlinMap[x, y + 1] <= Math.Round(perlinMap[x, y], 8) && this.mapPerlin.GetPixel(x, y + 1) != Color.Fuchsia && this.mapPerlin.GetPixel(x, y + 2) != Color.Fuchsia)
                                GenRiver(x, y + 1, 2);
                            break;
                        case 3:
                            if (perlinMap[x - 1, y] <= Math.Round(perlinMap[x, y], 8) && this.mapPerlin.GetPixel(x - 1, y) != Color.Fuchsia && this.mapPerlin.GetPixel(x - 2, y) != Color.Fuchsia)
                                GenRiver(x - 1, y, 3);
                            break;
                    }
                }
            }
        }
        private int[] RandTab(int[] tab)
        {
            Random r = new Random();
            int n = 0;
            foreach(int i in tab)
            {
                var id = r.Next(tab.Length);
                var tmp = tab[n];
                tab[n] = tab[id];
                tab[id] = tmp;
                n++;
            }
            return tab;
        }
    }
}

// Voir pour déplacer la carte.
// Voir pour mettre le zoom via un slider