﻿using LibNoise;
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

namespace WorldGen.Classes
{
    public class Map
    {
        public List<FortuneSite> sites = new List<FortuneSite>();
        public List<Region> cities = new List<Region>();
        private LinkedList<VEdge> vedges;
        private List<Color> colors = new List<Color>();
        private int mapWidth;
        private int mapHeight;
        public bool voronoi = true;
        public bool affSite = false;
        public bool influence = false;
        private double[,] perlinMap;
        private Bitmap map;
        private Bitmap mapPerlin;
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
            int nbSite = widthArea * heightArea / 1000; // default: /400
            perlinMap = new double[widthArea, heightArea];
            //nbSite = 30;

            this.colors.Add(Color.Red);
            this.colors.Add(Color.Black);
            this.colors.Add(Color.Purple);
            this.colors.Add(Color.Gray);
            this.colors.Add(Color.Cyan);

            Perlin p = new Perlin();
            p.Persistence = 0.65;
            p.Frequency = .003333;
            p.OctaveCount = 8;
            p.Seed = /*628594615;*/new Random().Next(999999999);

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
                var tmp = new Region(rand.NextDouble() * widthArea, rand.NextDouble() * heightArea, perlinMap);
                sites.Add(tmp);
                if (tmp.City)
                    cities.Add(tmp);
            }
            this.vedges = FortunesAlgorithm.Run(ref sites, 0, 0, widthArea, heightArea);

            if (cities.Count > 0)
            {
                int nbCapital = 5;
                while (nbCapital > 0)
                {
                    var tmp = cities.ElementAt(new Random().Next(cities.Count));
                    if (!tmp.Capital)
                    {
                        var r = new Random().Next(this.colors.Count);
                        tmp.Capital = true;
                        new Kingdom(tmp, this.colors.ElementAt(r));
                        this.colors.RemoveAt(r);
                        nbCapital--;
                    }
                }
            }
            
            // Génération de la liste des villes, et de la capital

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
            Image img = Image.FromFile("Assets\\gradient.bmp");
            Bitmap gradient = new Bitmap(img);

            if (mapPerlin == null)
            {
                mapPerlin = new Bitmap(mapWidth, mapHeight);
                for (int i = 0; i < mapWidth; i++)
                {
                    for (int j = 0; j < mapHeight; j++)
                    {
                        double value = perlinMap[i, j];
                        double y = (value + 1 > 2 ? 2 : (value + 1 < 0 ? 0 : value + 1));
                        int lvl = (int) ((y / 2) * 255);
                        Color c = gradient.GetPixel(lvl, 0);
                        mapPerlin.SetPixel(i, j, c);
                    }
                }
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
                    if (influence)
                    {
                        if(site.Influence > 0.1)
                            g.DrawString(site.Influence.ToString(), new Font(System.Drawing.FontFamily.GenericSansSerif, 5f), new SolidBrush(Color.Blue), (float)site.X, (float)site.Y);
                    }

                    if (site.City)
                    {
                        if(!site.Capital)
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
/*
 * Pour avoir les frontiers des pays, tester sur toutes les arrete des cellules. 
 * Et supprimer celles qui sont en double laisse les arretes unique.
 */