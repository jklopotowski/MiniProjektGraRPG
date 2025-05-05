using AplikacjaGra;
using BibliotekaGra.Ekwipunek;
using BibliotekaGra.Gra;
using BibliotekaGra.Postacie;
using BibliotekaGra.Sklep;
using BibliotekaGra.Wyjatki;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AplikacjaGra
{
    /// <summary>
    /// Logika interakcji dla klasy SklepView.xaml
    /// </summary>
    public partial class SklepView : UserControl
    {
        private DispatcherTimer timer;
        private Gra gra { get; set; }
        private Sklep sklep { get; set; }
        public SklepView(Gra g)
        {
            InitializeComponent(); 
            gra = g;
            sklep = gra.Sklep;
            DataContext = sklep;
            WczytajPlecak();
            WczytajZalozone();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            Timer_Tick(null, null);
            generujAsortyment();
        }
        private void Odswiez_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool sukces = false;

                if (sklep.CzyDarmoweOdswiezenie)
                {
                    sukces = sklep.OdnowAsortyment();
                    if (sukces)
                        MessageBox.Show("Darmowe odświeżenie sklepu zakończone sukcesem!");
                }
                else
                {
                    var wynik = MessageBox.Show(
                        $"Odświeżenie kosztuje 100 złota. Chcesz kontynuować?",
                        "Płatne odświeżenie", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (wynik == MessageBoxResult.Yes)
                    {
                        sukces = sklep.OdnowAsortyment(true);
                        if (sukces)
                            MessageBox.Show($"Sklep został odświeżony.");
                    }
                }

                if (sukces)
                    Content = new SklepView(gra);
            }
            catch (ZaMaloZlotaException ex)
            {
                var czasOd = DateTime.Now - sklep.OstatnieDarmoweOdnowienie;
                var doDarmowego = TimeSpan.FromMinutes(60) - czasOd;
                MessageBox.Show($"{ex.Message}\nDarmowe odświeżenie będzie za: {doDarmowego.Minutes:D2}m {doDarmowego.Seconds:D2}s", "Brak kasy", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public void generujAsortyment()
        {
            Itemy.Children.Clear();

            for (int i = 0; i < sklep.MaksRozmiar; i++)
            {
                var przedmiot = sklep.Asortyment[i];
                var border = new Border
                {
                    Width = 100,
                    Height = 100,
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.Black,
                    Background = Brushes.LightGray,
                    Margin = new Thickness(5)
                };

                var grid = new Grid();

                Image obrazek = new Image
                {
                    Width = 60,
                    Height = 60,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(10),
                    Stretch = Stretch.UniformToFill
                };

                if (przedmiot != null)
                {
                    var binding = new Binding("SciezkaDoObrazka")
                    {
                        Source = przedmiot,
                        Converter = (IValueConverter)FindResource("SciezkaDoObrazkaConverter")
                    };
                    BindingOperations.SetBinding(obrazek, Image.SourceProperty, binding);
                    border.ToolTip = przedmiot.ToString(); 
                }
                else
                {
                    obrazek.Source = new BitmapImage(new Uri("/Obrazki/pusty_slot.png", UriKind.Relative));
                    border.ToolTip = $"Pusty slot";
                }

                grid.Children.Add(obrazek);

                var btn = new Button
                {
                    Content = $"Kup ({przedmiot.Wartosc})",
                    Width = 60,
                    Height=20,
                    Margin = new Thickness(5),
                    Tag = i,
                    IsEnabled = przedmiot != null && gra.Gracz.Zloto >= przedmiot.Wartosc && gra.Gracz.Ekwipunek.Plecak.CzyJestMiejsce,
                    VerticalAlignment=VerticalAlignment.Bottom
                };
                btn.Click += Kup_Click;
                grid.Children.Add(btn);

                border.Child = grid;

                grid.MouseLeftButtonDown += (s, e) =>
                {
                    if (e.ClickCount == 2 && btn.IsEnabled)
                        Kup_Click(btn, null);
                };

                Itemy.Children.Add(border);
            }
        }
        private void Kup_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag.ToString(), out int index))
            {
                sklep.KupPrzedmiot(index);
                sklep.Asortyment[index] = GeneratorPrzedmiotow.GenerujPrzedmiotPoziom(gra.Gracz);
                Content = new SklepView(gra);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var teraz = DateTime.Now;
            var czasOdOstatniego = teraz - sklep.OstatnieDarmoweOdnowienie;
            var czasDoDarmowego = TimeSpan.FromMinutes(60) - czasOdOstatniego;

            if (czasDoDarmowego.TotalSeconds <= 0)
            {
                OdliczanieLabel.Text = "Darmowe odświeżenie dostępne!";
            }
            else
            {
                OdliczanieLabel.Text = $"Darmowe odświeżenie za: {czasDoDarmowego.Minutes:D2}m {czasDoDarmowego.Seconds:D2}s";
            }
        }
        public void OdswiezPlecEq()
        {
            Content = new SklepView(gra);
        }
        public void WczytajPlecak()
        {
            Plecak.Content = new PlecakView(gra.Gracz, 2, 5, this);
        }
        public void WczytajZalozone()
        {
            Ekwipunek.Content = new EkwipunekView(gra.Gracz, this);
        }
    }
}
