using BibliotekaGra.Gra;
using BibliotekaGra.Postacie;
using BibliotekaGra.Wyprawy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AplikacjaGra
{
    /// <summary>
    /// Logika interakcji dla klasy WalkaView.xaml
    /// </summary>
    public partial class WalkaView : UserControl
    {
        private (bool wygrana, List<RundaWalki> log, string nagrody) wynikWalki;
        public Wyprawa wyprawa { get; set; }
        public Postac postac { get; set; }
        public ObservableCollection<string> TuryWalki { get; set; } = new();
        public string WynikWalki { get; set; } = string.Empty;
        public string Nagrody { get; set; } = string.Empty;

        private bool pominiete = false;
        public WalkaView(Wyprawa w, Postac p)
        {
            InitializeComponent();
            Loaded += WalkaViewerControl_Loaded;
            wyprawa = w;
            postac = p; 
            DataContext = this; 
            wynikWalki = wyprawa.RozegrajWyprawe(postac);
            TuryWalki = new ObservableCollection<string>(
                wynikWalki.log.Select(r => $"{r.Kto} | {r.Akcja} | {r.Opis} | {r.Dmg} | {r.CzyKryt}")
            );
            WynikWalki = wynikWalki.wygrana ? "Wygrana" : "Przegrana";
            Nagrody = wynikWalki.nagrody;
        }

        private async void WalkaViewerControl_Loaded(object sender, RoutedEventArgs e)
        {
            await AnimujTuryAsync();
        }

        private async Task AnimujTuryAsync()
        {
            postac.Zdrowie = postac.MaxZdrowie;
            wyprawa.Potwor.Zdrowie = wyprawa.Potwor.MaxZdrowie;
            Debug.WriteLine("AnimujTuryAsync wywołane!");
            foreach (var tura in TuryWalki)
            {
                if (pominiete) break;

                string[] tur = tura.Split(" | ");
                var tb = new TextBlock
                {
                    Text = $"{tur[0]} | {tur[1]} | {tur[2]}",
                    Margin = new Thickness(0, 5, 0, 5),
                    Foreground = Brushes.White,
                    Opacity = 0
                };
                if (tur[4] == "1")
                    tb.Foreground = Brushes.Orange;
                if (tur[0] == "Gracz" && tur[1]=="Unik")
                    tb.Foreground= Brushes.LawnGreen;
                if (tur[0] == "Gracz" && tur[1] == "Obrazenia")
                    if (postac is Lucznik l)
                        l.OtrzymajDmgBezUnik(wyprawa.Potwor, tur[4] == "1");
                    else
                        postac.OtrzymajDmg(wyprawa.Potwor, tur[4] == "1");
                else if (tur[0] == "Potwor" && tur[1] == "Obrazenia")
                    wyprawa.Potwor.OtrzymajDmg(postac, tur[4] == "1");

                if (tur[0] == "Gracz")
                    GraczTuraContainer.Children.Add(tb);
                else
                    PotworTuraContainer.Children.Add(tb);

                var fade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
                tb.BeginAnimation(OpacityProperty, fade);

                GraczHP.Value = postac.Zdrowie;
                PotworHP.Value = wyprawa.Potwor.Zdrowie;

                await Task.Delay(500);
            }
            if (!pominiete)
                PokazWynik();
        }

        private void PominButton_Click(object sender, RoutedEventArgs e)
        {
            pominiete = true;
            GraczTuraContainer.Children.Clear();
            PotworTuraContainer.Children.Clear();
            postac.Zdrowie = postac.MaxZdrowie;
            wyprawa.Potwor.Zdrowie = wyprawa.Potwor.MaxZdrowie; 
            foreach (var tura in TuryWalki)
            {
                string[] tur = tura.Split(" | ");
                var tb = new TextBlock
                {
                    Text = $"{tur[0]} | {tur[1]} | {tur[2]}",
                    Margin = new Thickness(0, 5, 0, 5),
                    Foreground = Brushes.White,
                };
                if (tur[4]=="1")
                    tb.Foreground = Brushes.Orange;
                if (tur[0] == "Gracz" && tur[1] == "Unik")
                    tb.Foreground = Brushes.LawnGreen;
                if (tur[0] == "Gracz" && tur[1] == "Obrazenia")
                    if (postac is Lucznik l)
                        l.OtrzymajDmgBezUnik(wyprawa.Potwor, tur[4] == "1");
                    else
                        postac.OtrzymajDmg(wyprawa.Potwor, tur[4] == "1");
                else if (tur[0] == "Potwor" && tur[1] == "Obrazenia")
                    wyprawa.Potwor.OtrzymajDmg(postac, tur[4] == "1");

                if (tur[0] == "Gracz")
                    GraczTuraContainer.Children.Add(tb);
                else
                    PotworTuraContainer.Children.Add(tb);
            }
            GraczHP.Value = postac.Zdrowie;
            PotworHP.Value = wyprawa.Potwor.Zdrowie;
            PokazWynik();
        }

        private void PokazWynik()
        {
            GraczHP.Value = postac.Zdrowie;
            PotworHP.Value = wyprawa.Potwor.Zdrowie;
            WynikPanel.Visibility = Visibility.Visible;
            WynikText.Text = WynikWalki;
            NagrodyText.Text = WynikWalki == "Wygrana" ? $"Nagrody: {Nagrody}" : "Brak nagród. Spróbuj jeszcze raz!";
            DalejButton.Visibility = Visibility.Visible;
            PominBtn.Visibility = Visibility.Collapsed;
        }
        private void DalejButton_Click(object sender, RoutedEventArgs e)
        {
            WynikPanel.Visibility = Visibility.Collapsed;
            wyprawa.wywolajEvent();
        }
    }
}
