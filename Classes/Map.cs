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

namespace WorldGen.Classes
{
    public class Map
    {
        public List<FortuneSite> sites = new List<FortuneSite>();
        private LinkedList<VEdge> vedges;
        private int mapWidth;
        private int mapHeight;
        public bool voronoi = true;
        public bool affSite = true;
        private double[,] perlinMap;
        private Bitmap map;
        private Bitmap mapPerlin;
        private int zoom;
        private float ratio;

        public Map()
        {
        }
        public Map(int widthArea, int heightArea)
        {
            mapHeight = heightArea;
            mapWidth = widthArea;
            ratio = (int)(mapWidth / mapHeight);
            Random rand = new Random();
            int nbSite = widthArea * heightArea / 400;
            perlinMap = new double[widthArea, heightArea];
            //nbSite = 30;
            
            Perlin p = new Perlin();
            p.Persistence = 0.65;
            p.Frequency = .003333;
            p.OctaveCount = 8;
            p.Seed = 628594615;//new Random().Next(5000);

            for (int i = 0; i < widthArea; i++)
            {
                for (int j = 0; j < heightArea; j++)
                {
                    var tmp = p.GetValue(i, j, 0);
                    perlinMap[i, j] = p.GetValue(i, j, 0);
                }
            }
            
            for (int i = 0; i < nbSite; i++)
            {
                sites.Add(new Region(rand.NextDouble()*widthArea, rand.NextDouble()*heightArea, p));
            }
            this.vedges = FortunesAlgorithm.Run(ref sites, 0, 0, widthArea, heightArea);

            foreach (FortuneSite site in sites)
            {
                foreach (VEdge vedge in vedges)
                {
                    if(vedge.Left == site || vedge.Right == site)
                        if(!site.Cell.Contains(vedge))
                            site.Cell.Add(vedge);
                }
            }
        }
        
        public ImageSource getVoronoiGraph(bool delaunay = false)
        {
            Bitmap bmp = new Bitmap(this.mapWidth, this.mapHeight);
            Image img = Image.FromFile("C:\\Users\\loicr\\Desktop\\Projet\\Perso\\WorldGen\\Assets\\gradient.bmp");
            Bitmap gradient = new Bitmap(img);

            /*if (mapPerlin == null)
            {
                for (int i = 0; i < mapWidth; i++)
                {
                    for (int j = 0; j < mapHeight; j++)
                    {
                        double value = perlinMap[i, j];
                        double y = (value + 1 > 2 ? 2 : (value + 1 < 0 ? 0 : value + 1));
                        int lvl = (int) ((y / 2) * 255);
                        Color c = gradient.GetPixel(lvl, 0);
                        bmp.SetPixel(i, j, c);
                    }
                }
                this.mapPerlin = bmp;
            }
            else
            {
                bmp = this.mapPerlin;
            }*/


            using (Graphics g = Graphics.FromImage(bmp))
            {
                int seed = -1;

                
                
                foreach (Region site in sites)
                {
                    seed++;
                    
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
                        points.Add(new Point((int) edge.Start.X, (int) edge.Start.Y));
                        points.Add(new Point((int) edge.End.X, (int) edge.End.Y));
                    }

                    Point[] tab = points.ToArray();
                    Random rand = new Random(seed);
                    Color color = gradient.GetPixel(site.Height, 0);
                    
                    g.FillPolygon(new SolidBrush(color), tab);

                    if(affSite)
                        g.FillEllipse(new SolidBrush(Color.Crimson), 
                            (float)(site.X-2), (float)(site.Y-2),
                            5f, 5f);
                    
                    if (delaunay)
                    {
                        foreach (var neighbor in site.Neighbors)
                        {
                            g.DrawLine(new System.Drawing.Pen(Color.Blue),
                                (float) site.X,
                                (float) site.Y,
                                (float) neighbor.X,
                                (float) neighbor.Y);
                        }
                    }
                }

                if (voronoi)
                {
                    foreach (VEdge vedge in vedges)
                    {
                        g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black),
                            new System.Drawing.Point((int) vedge.Start.X, (int) vedge.Start.Y),
                            new System.Drawing.Point((int) vedge.End.X, (int) vedge.End.Y));
                    }
                }
            }

            this.map = bmp;
            return ImageSourceFromBitmap(bmp);
        }
        
        private ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            ImageSource newSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return newSource;
        }
    }
}

// Voir pour déplacer la carte.
// Voir pour mettre le zoom via un slider
/*
 * Sur un evenement mouse left button hold/cilck, déplacer le x,y en fonction du delta de la souris
 * ou le width, height si x,y == 0,0
 */
