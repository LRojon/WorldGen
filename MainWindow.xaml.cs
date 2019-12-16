﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorldGen.Classes;

namespace WorldGen
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool delaunay = false;
        private World graph;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
            graph = new World((int)this.map.ActualWidth, (int)this.map.ActualHeight);
            this.imageMap.Source = graph.getVoronoiGraph(this.delaunay);
        }

        private void HamburgerMenuItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            graph = new World((int)this.map.ActualWidth, (int)this.map.ActualHeight);
            this.imageMap.Source = graph.getVoronoiGraph(this.delaunay);
        }

        private void Exit_Selected(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Vous êtes sur ?", "Quitter ?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void map_Selected(object sender, RoutedEventArgs e)
        {
            city.Visibility = Visibility.Hidden;
            map.Visibility = Visibility.Visible;
        }
        private void city_Selected(object sender, RoutedEventArgs e)
        {
            city.Visibility = Visibility.Visible;
            map.Visibility = Visibility.Hidden;
        }

        private void delaunay_ON_OFF(object sender, RoutedEventArgs e)
        {
            Button btn = (Button) sender;

            delaunay = !delaunay;
            btn.Content = delaunay ? "Delaunay: ON" : "Delaunay: OFF";
            
            if(graph != null)
                imageMap.Source = graph.getVoronoiGraph(delaunay);
        }        
        private void voronoi_ON_OFF(object sender, RoutedEventArgs e)
        {
            Button btn = (Button) sender;

            graph.voronoi = !graph.voronoi;
            btn.Content = graph.voronoi ? "Voronoi: ON" : "Voronoi: OFF";

            if (graph != null)
                imageMap.Source = graph.getVoronoiGraph(delaunay);
        }
        private void sites_ON_OFF(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            graph.affSite = !graph.affSite;
            btn.Content = graph.affSite ? "Royaume: ON" : "Royaume: OFF";

            if (graph != null)
                imageMap.Source = graph.getVoronoiGraph(delaunay);
        }
        private void influence_ON_OFF(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            graph.influence = !graph.influence;
            btn.Content = graph.influence ? "Influence: ON" : "Influence: OFF";

            if (graph != null)
                imageMap.Source = graph.getVoronoiGraph(delaunay);
        }

        private void imageMap_ToolTip(object sender, MouseEventArgs e)
        {
            this.imageMap.Source = graph.getVoronoiGraph(e.GetPosition(this), delaunay);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void imageMap_MouseLeave(object sender, MouseEventArgs e)
        {
            imageMap.Source = graph.getVoronoiGraph(delaunay);
        }
    }
}
