using HTMLConverter;
using System;
using System.Globalization;
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
        private ImageSource backPantheon;

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
            pantheonContent.ItemsSource = World.pantheon.Gods;
            this.backPantheon = backgroundPantheon.Source;

            var tmp = World.cities.OrderBy(c => c.Kingdom != null ? c.Kingdom.Name : "zzzzzzzz").ThenByDescending(c => c.Capital);
            this.listCity.ItemsSource = tmp;
            this.listCity.SelectedItem = tmp.First();

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
        private void Pantheon_Selected(object sender, RoutedEventArgs e)
        {
            this.GoTo(pantheon);
            pantheonContent.SelectedItem = World.pantheon.Gods.First();
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
        private void Croyance_ON_OFF(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            World.croyance = !World.croyance;
            btn.Content = World.croyance ? "Croyance: ON" : "Croyance: OFF";

            if (World != null)
                imageMap.Source = World.GetVoronoiGraph(delaunay);
        }
        private void NewWorld(object sender, RoutedEventArgs e)
        {
            World = new World((int)this.map.ActualWidth, (int)this.map.ActualHeight);
            this.imageMap.Source = World.GetVoronoiGraph(this.delaunay);
            this.listKingdom.ItemsSource = World.kingdoms;
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
            affKingdom.Height = 40 + kingdom.Distribution.Count * 40;
            capitalName.Content = "Capitale: " + kingdom.Capital.Name;
            var xaml = HtmlToXamlConverter.ConvertHtmlToXaml(kingdom.GetInfo(), true);
            var flowDocument = XamlReader.Parse(xaml) as FlowDocument;
            affKingdom.Document = flowDocument;
            kingdomType.Content = kingdom.Type + (kingdom.Type == Classes.Enum.KingdomType.Théocratie ? ": " + kingdom.God.ToString() : "" );
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (subMenuMap.Visibility == Visibility.Visible)
                subMenuMap.Visibility = Visibility.Hidden;
            else
                subMenuMap.Visibility = Visibility.Visible;
        }

        private void PantheonContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            God selected = World.pantheon.Gods.Where(g => g.ToString() == pantheonContent.SelectedItem.ToString()).FirstOrDefault();
            if (!selected.Forgot)
                backgroundPantheon.Source = World.GetGodMap(selected.ToString());
            else
                backgroundPantheon.Source = this.backPantheon;
            SetGod(selected);
        }

        private void SetGod(God god)
        {
            nbRegion.Content = god.Followers.Count.ToString("N0", CultureInfo.GetCultureInfo("ru-RU")) + " regions sous influence";
            nbFollower.Content = god.Followers.Sum(r => r.Citizen).ToString("N0", CultureInfo.GetCultureInfo("ru-RU")) + " adepte";
        }

        private void City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Region selected = World.cities.Where(c => c.ToString() == listCity.SelectedItem.ToString()).FirstOrDefault();
            //backgroundKingdom.Source = World.GetKingdomMap(selected.ToString());
            SetCity(selected);
        }

        private void SetCity(Region r)
        {
            if(r.Capital)
            {
                this.isNotCapitale.Visibility = Visibility.Hidden;
                this.isCapitale.Visibility = Visibility.Visible;
            }
            else
            {
                this.isCapitale.Visibility = Visibility.Hidden;
                this.isNotCapitale.Visibility = Visibility.Visible;
            }

            this.cityKingdom.Content = (r.Kingdom != null ? r.Kingdom.Name : "");
            this.cityGod.Content = (r.God != null ? r.God.ToString() : "");
            this.cityType.Content = r.Size;

            var xaml = HtmlToXamlConverter.ConvertHtmlToXaml(r.GetHTMLInfo(), true);
            var flowDocument = XamlReader.Parse(xaml) as FlowDocument;
            cityInfo.Document = flowDocument;
        }
    }
}
