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
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using FortuneVoronoi;
using HandyCollections.Extensions;
using VoronoiLib;
using VoronoiLib.Structures;
using Color = System.Drawing.Color;
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

        public Map()
        {
        }
        public Map(int widthArea, int heightArea)
        {
            mapHeight = heightArea;
            mapWidth = widthArea;
            Random rand = new Random();
            int nbSite = widthArea * heightArea / 400;
            //nbSite = 30;
            List<Vector2> list = new List<Vector2>();

            for (int i = 0; i < nbSite; i++)
            {
                sites.Add(new FortuneSite(rand.NextDouble()*widthArea, rand.NextDouble()*heightArea));
                list.Add(new Vector2((float)rand.NextDouble()*widthArea, (float)rand.NextDouble()*heightArea));
            }
            this.vedges = FortunesAlgorithm.Run(ref sites, 0, 0, widthArea, heightArea);
            /*if(!this.vedges.IsEmpty())
                MessageBox.Show("Nombre de site pour ("+ widthArea + "x" + heightArea + "): " + nbSite);*/
        }

        public ImageSource getVoronoiGraph(bool delaunay = false)
        {
            Bitmap bmp = new Bitmap(this.mapWidth, this.mapHeight);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                foreach (var site in sites)
                {
                    foreach (VEdge vedge in vedges)
                    {
                        if(vedge.Left == site || vedge.Right == site)
                            if(!site.Cell.Contains(vedge))
                                site.Cell.Add(vedge);
                    }
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
                    Random rand = new Random();
                    Color color = Color.FromArgb(255,
                        rand.Next(255),
                        rand.Next(255),
                        rand.Next(255));
                    g.FillPolygon(new SolidBrush(Color.Red), tab);

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
