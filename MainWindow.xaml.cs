﻿using HTMLConverter;
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
using Newtonsoft.Json;
using Microsoft.Win32;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

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
            /*World = new World((int)this.map.ActualWidth, (int)this.map.ActualHeight);
            this.imageMap.Source = World.GetVoronoiGraph(this.delaunay);

            this.listKingdom.ItemsSource = World.kingdoms;
            this.listKingdom.SelectedItem = World.kingdoms.First();
            pantheonContent.ItemsSource = World.pantheon.Gods;
            this.backPantheon = backgroundPantheon.Source;

            var tmp = World.cities.OrderBy(c => c.Kingdom != null ? c.Kingdom.Name : "zzzzzzzz").ThenByDescending(c => c.Capital);
            this.listCity.ItemsSource = tmp;
            this.listCity.SelectedItem = tmp.First();*/

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
            if (this.World != null)
            {
                this.GoTo(city);

                var r = new Random().Next(4);
                var b = new BitmapImage(new Uri("Assets/Background/BackgroundCity-" + r + ".jpg", UriKind.Relative));
                backgroundCity.Source = b;
            }
        }
        private void Kingdom_Selected(object sender, RoutedEventArgs e)
        {
            if (this.World != null)
            {
                this.GoTo(kingdom);
                listKingdom.SelectedItem = World.kingdoms.First();
                backgroundKingdom.Source = World.GetKingdomMap(listKingdom.SelectedItem.ToString());
                SetKingdom(World.kingdoms.First());
            }
        }
        private void Pantheon_Selected(object sender, RoutedEventArgs e)
        {
            if (this.World != null)
            {
                this.GoTo(pantheon);
                pantheonContent.SelectedItem = World.pantheon.Gods.First();
            }
        }

        private void Delaunay_ON_OFF(object sender, RoutedEventArgs e)
        {
            if (this.World != null)
            {
                Button btn = (Button)sender;

                delaunay = !delaunay;
                btn.Content = delaunay ? "Delaunay: ON" : "Delaunay: OFF";

                if (World != null)
                    imageMap.Source = World.GetVoronoiGraph(delaunay);
            }
        }        
        private void Voronoi_ON_OFF(object sender, RoutedEventArgs e)
        {
            if (this.World != null)
            {
                Button btn = (Button)sender;

                World.voronoi = !World.voronoi;
                btn.Content = World.voronoi ? "Voronoi: ON" : "Voronoi: OFF";

                if (World != null)
                    imageMap.Source = World.GetVoronoiGraph(delaunay);
            }
        }
        private void Sites_ON_OFF(object sender, RoutedEventArgs e)
        {
            if (this.World != null)
            {
                Button btn = (Button)sender;

                World.affSite = !World.affSite;
                btn.Content = World.affSite ? "Royaume: ON" : "Royaume: OFF";

                if (World != null)
                    imageMap.Source = World.GetVoronoiGraph(delaunay);
            }
        }
        private void Croyance_ON_OFF(object sender, RoutedEventArgs e)
        {
            if (this.World != null)
            {
                Button btn = (Button)sender;

                World.croyance = !World.croyance;
                btn.Content = World.croyance ? "Croyance: ON" : "Croyance: OFF";

                if (World != null)
                    imageMap.Source = World.GetVoronoiGraph(delaunay);
            }
        }
        
        private void NewWorld(object sender, RoutedEventArgs e)
        {
            this.NW.Visibility = Visibility.Hidden;
            World = new World((int)this.map.ActualWidth, (int)this.map.ActualHeight);
            this.imageMap.Source = World.GetVoronoiGraph(this.delaunay);

            this.listKingdom.SelectedItem = World.kingdoms.First();
            this.listKingdom.ItemsSource = World.kingdoms;
            this.pantheonContent.SelectedItem = World.pantheon.Gods.First();
            this.pantheonContent.ItemsSource = World.pantheon.Gods;
            this.backPantheon = backgroundPantheon.Source;

            var tmp = World.cities.OrderBy(c => c.Kingdom != null ? c.Kingdom.Name : "zzzzzzzz").ThenByDescending(c => c.Capital);
            this.listCity.SelectedItem = tmp.First();
            this.listCity.ItemsSource = tmp;

            this.GoTo(map);
        }
        private void ImageMap_ToolTip(object sender, MouseEventArgs e)
        {
            if (this.World != null)
            {
                this.imageMap.Source = World.GetVoronoiGraph(e.GetPosition(this), delaunay);
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void ImageMap_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.World != null)
            {
                imageMap.Source = World.GetVoronoiGraph(delaunay);
            }
        }
    
        private void GoTo(Grid g)
        {
            foreach (var elem in app.Children)
            {
                if (elem is Grid tmp && ((Grid)elem).Name != "menu")
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
            kingdomType.Content = kingdom.Type + (kingdom.Type == Classes.Enum.KingdomType.Théocratie ? ": " + kingdom.God.ToString() : "");
            capitalName.Content = "Capitale: " + kingdom.Capital.Name;

            demoInfo.Height = 40 + kingdom.Distribution.Count * 40;
            var xaml = HtmlToXamlConverter.ConvertHtmlToXaml(kingdom.GetDemoInfo(), true);
            var flowDocument = XamlReader.Parse(xaml) as FlowDocument;
            demoInfo.Document = flowDocument;

            ecoInfo.Height = 40 + kingdom.Richesse.Count * 40;
            xaml = HtmlToXamlConverter.ConvertHtmlToXaml(kingdom.GetEcoInfo(), true);
            flowDocument = XamlReader.Parse(xaml) as FlowDocument;
            ecoInfo.Document = flowDocument;
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (this.World != null)
            {
                if (subMenuMap.Visibility == Visibility.Visible)
                    subMenuMap.Visibility = Visibility.Hidden;
                else
                    subMenuMap.Visibility = Visibility.Visible;
            }
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
            cityRessource.Content = "Ressource principale: " + r.Ressource.ToString();
        }
    
        private void SaveWorld(object sender, RoutedEventArgs e)
        {
            if (this.World != null)
            {
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    Title = "Save your world...",
                    FileName = World.name + ".wg",
                    Filter = "World file (*.wg)|*.wg",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    AddExtension = true,
                    OverwritePrompt = true
                };
                sfd.FileOk += Sfd_FileOk;

                sfd.ShowDialog();
            }
        }

        private void Sfd_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveFileDialog sfd = (SaveFileDialog)sender;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, World);
            stream.Close();
        }

        private void LoadWorld(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Load your world...",
                FileName = World.name + ".wg",
                Filter = "World file (*.wg)|*.wg",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            };
            ofd.FileOk += Ofd_FileOk; ;

            ofd.ShowDialog();
        }

        private void Ofd_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpenFileDialog ofd = (OpenFileDialog)sender;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            this.World = (World)formatter.Deserialize(stream);
            stream.Close();

            this.imageMap.Source = World.GetVoronoiGraph(this.delaunay);

            this.listKingdom.SelectedItem = World.kingdoms.First();
            this.listKingdom.ItemsSource = World.kingdoms;
            this.pantheonContent.SelectedItem = World.pantheon.Gods.First();
            this.pantheonContent.ItemsSource = World.pantheon.Gods;
            this.backPantheon = backgroundPantheon.Source;

            var tmp = World.cities.OrderBy(c => c.Kingdom != null ? c.Kingdom.Name : "zzzzzzzz").ThenByDescending(c => c.Capital);
            this.listCity.SelectedItem = tmp.First();
            this.listCity.ItemsSource = tmp;

            this.GoTo(map);
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string[] path = (string[])e.Data.GetData(DataFormats.FileDrop);

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path[0], FileMode.Open, FileAccess.Read, FileShare.Read);
            this.World = (World)formatter.Deserialize(stream);
            stream.Close();

            this.imageMap.Source = World.GetVoronoiGraph(this.delaunay);

            this.listKingdom.SelectedItem = World.kingdoms.First();
            this.listKingdom.ItemsSource = World.kingdoms;
            this.pantheonContent.SelectedItem = World.pantheon.Gods.First();
            this.pantheonContent.ItemsSource = World.pantheon.Gods;
            this.backPantheon = backgroundPantheon.Source;

            var tmp = World.cities.OrderBy(c => c.Kingdom != null ? c.Kingdom.Name : "zzzzzzzz").ThenByDescending(c => c.Capital);
            this.listCity.SelectedItem = tmp.First();
            this.listCity.ItemsSource = tmp;

            this.NW.Visibility = Visibility.Hidden;
            this.GoTo(map);
        }
    }
}
