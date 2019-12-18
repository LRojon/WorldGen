using HTMLConverter;
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
using System.Windows.Markup;
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
        private World _world;

        public World World { get => _world; set => _world = value; }

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
            World = new World((int)this.map.ActualWidth, (int)this.map.ActualHeight);
            this.imageMap.Source = World.GetVoronoiGraph(this.delaunay);
            this.listKingdom.ItemsSource = World.kingdoms;

            this.GoTo(map);
        }

        private void HamburgerMenuItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            World = new World((int)this.map.ActualWidth, (int)this.map.ActualHeight);
            this.imageMap.Source = World.GetVoronoiGraph(this.delaunay);
        }

        private void Exit_Selected(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Vous êtes sur ?", "Quitter ?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void Map_Selected(object sender, RoutedEventArgs e)
        {
            this.GoTo(map);
        }
        private void City_Selected(object sender, RoutedEventArgs e)
        {
            this.GoTo(city);

            var r = new Random().Next(4);
            var b = new BitmapImage(new Uri("Assets/Background/BackgroundCity-" + r + ".jpg", UriKind.Relative));
            backgroundCity.Source = b;
        }
        private void Kingdom_Selected(object sender, RoutedEventArgs e)
        {
            this.GoTo(kingdom);
            listKingdom.SelectedItem = World.kingdoms.First();
            backgroundKingdom.Source = World.GetKingdomMap(listKingdom.SelectedItem.ToString());
            SetKingdom(World.kingdoms.First());
        }

        private void Delaunay_ON_OFF(object sender, RoutedEventArgs e)
        {
            Button btn = (Button) sender;

            delaunay = !delaunay;
            btn.Content = delaunay ? "Delaunay: ON" : "Delaunay: OFF";
            
            if(World != null)
                imageMap.Source = World.GetVoronoiGraph(delaunay);
        }        
        private void Voronoi_ON_OFF(object sender, RoutedEventArgs e)
        {
            Button btn = (Button) sender;

            World.voronoi = !World.voronoi;
            btn.Content = World.voronoi ? "Voronoi: ON" : "Voronoi: OFF";

            if (World != null)
                imageMap.Source = World.GetVoronoiGraph(delaunay);
        }
        private void Sites_ON_OFF(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            World.affSite = !World.affSite;
            btn.Content = World.affSite ? "Royaume: ON" : "Royaume: OFF";

            if (World != null)
                imageMap.Source = World.GetVoronoiGraph(delaunay);
        }
        private void Influence_ON_OFF(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            World.influence = !World.influence;
            btn.Content = World.influence ? "Influence: ON" : "Influence: OFF";

            if (World != null)
                imageMap.Source = World.GetVoronoiGraph(delaunay);
        }
        private void NewWorld(object sender, RoutedEventArgs e)
        {
            World = new World((int)this.map.ActualWidth, (int)this.map.ActualHeight);
            this.imageMap.Source = World.GetVoronoiGraph(this.delaunay);
        }

        private void ImageMap_ToolTip(object sender, MouseEventArgs e)
        {
            this.imageMap.Source = World.GetVoronoiGraph(e.GetPosition(this), delaunay);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void ImageMap_MouseLeave(object sender, MouseEventArgs e)
        {
            imageMap.Source = World.GetVoronoiGraph(delaunay);
        }
    
        private void GoTo(Grid g)
        {
            foreach (var elem in app.Children)
            {
                if (elem is Grid tmp)
                {
                    tmp.Visibility = Visibility.Hidden;
                }
            }
            g.Visibility = Visibility.Visible;
        }

        private void Kingdom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Kingdom selected = World.kingdoms.Where(k => k.Name == listKingdom.SelectedItem.ToString()).FirstOrDefault();
            backgroundKingdom.Source = World.GetKingdomMap(selected.ToString());
            SetKingdom(selected);
        }
    
        private void SetKingdom(Kingdom kingdom)
        {
            capitalName.Content = "Capitale: " + kingdom.Capital.Name; 
            var xaml = HtmlToXamlConverter.ConvertHtmlToXaml(kingdom.GetInfo(), true);
            var flowDocument = XamlReader.Parse(xaml) as FlowDocument;
            affKingdom.Document = flowDocument;
        }
    }
}
