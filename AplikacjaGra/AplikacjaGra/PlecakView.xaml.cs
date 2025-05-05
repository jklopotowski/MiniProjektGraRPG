using BibliotekaGra.Ekwipunek;
using BibliotekaGra.Gra;
using BibliotekaGra.Postacie;
using BibliotekaGra.Sklep;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace AplikacjaGra
{

    /// <summary>
    /// Logika interakcji dla klasy PlecakView.xaml
    /// </summary>
    public partial class PlecakView : UserControl
    {
        private Postac postac { get; set; }
        private Plecak plecak { get; set; }
        private UserControl view { get; set; }
        public PlecakView(Postac p, int rows, int cols, UserControl v)
        {
            InitializeComponent();
            postac = p;
            plecak = postac.Ekwipunek.Plecak;
            DataContext = plecak;
            Itemy.Rows = rows;
            Itemy.Columns = cols;
            view = v;
            generujPlecak();
        }

        public void generujPlecak()
        {
            Itemy.Children.Clear();

            for (int i = 0; i < plecak.MaksRozmiar; i++)
            {
                int lokalnyIndex = i;
                var border = new Border
                {
                    Width = 64,
                    Height = 64,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(5),
                    Background = Brushes.LightGray
                };

                var grid = new Grid();

                Image obrazek = new Image
                {
                    Width = 60,
                    Height = 60,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Stretch = Stretch.UniformToFill
                };

                if (plecak.Sloty[i] != null)
                {
                    var binding = new Binding("SciezkaDoObrazka")
                    {
                        Source = plecak.Sloty[i],
                        Converter = (IValueConverter)FindResource("SciezkaDoObrazkaConverter")
                    };

                    BindingOperations.SetBinding(obrazek, Image.SourceProperty, binding);
                    border.ToolTip = plecak.Sloty[i].ToString();

                    obrazek.MouseLeftButtonUp += (s, e) => { ZakladajLubZamien(lokalnyIndex); };
                }
                else
                {
                    obrazek.Source = new BitmapImage(new Uri("/Obrazki/pusty_slot.png", UriKind.Relative));
                    border.ToolTip = "Pusty slot";
                }

                grid.Children.Add(obrazek);

                if (plecak.Sloty[i] != null)
                {
                    var btn = new Button
                    {
                        Content = "$",
                        Width = 20,
                        Height = 20,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Top,
                        FontSize = 10,
                        Tag = lokalnyIndex,
                        Padding = new Thickness(0)
                    };
                    btn.Click += Sprzedaj_Click;
                    btn.Click += (s, e) =>
                        Debug.WriteLine($"Klik na slot {lokalnyIndex}");

                    grid.Children.Add(btn);
                }

                border.Child = grid;
                Itemy.Children.Add(border);
            }
        }
        private void ZakladajLubZamien(int index)
        {
            if (index < 0 || index >= plecak.MaksRozmiar)
            {
                MessageBox.Show("Nieprawidłowy index – kliknąłeś matrix.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var przedmiot = plecak.Sloty[index];
            if (przedmiot == null) return;

            postac.Ekwipunek.ZalozPrzedmiot(przedmiot, index);
            if (view is ProfilView profil)
            {
                profil.OdswiezPlecEq();
            }
            else if (view is SklepView sklep)
            {
                sklep.OdswiezPlecEq();
            }
        }
        private void Sprzedaj_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag.ToString(), out int index))
            {
                postac.SprzedajPrzedmiot(index);
                if (view is ProfilView profil)
                {
                    profil.OdswiezPlecEq();
                }
                else if (view is SklepView sklep)
                {
                    sklep.OdswiezPlecEq();
                }
            }
        }
    }
}
