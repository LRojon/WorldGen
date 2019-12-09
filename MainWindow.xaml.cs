using System;
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
        private Map graph;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void HamburgerMenuItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            graph = new Map((int)this.map.ActualWidth, (int)this.map.ActualHeight);
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
            if (delaunay)
                btn.Content = "ON";
            else
                btn.Content = "OFF";
            
            if(graph != null)
                imageMap.Source = graph.getVoronoiGraph(delaunay);
        }
    }
}
