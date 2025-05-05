using BibliotekaGra.Gra;
using BibliotekaGra.Postacie;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AplikacjaGra
{
    /// <summary>
    /// Logika interakcji dla klasy MenadzerPostaciView.xaml
    /// </summary>
    public partial class MenadzerPostaciView : UserControl
    {
        private MenadżerPostaci menadzerPostaci { get; set; }
        public MenadzerPostaciView(MenadżerPostaci men)
        {
            InitializeComponent();
            menadzerPostaci = men;
            DataContext = menadzerPostaci;
            OdswiezListePostaci();
        }
        private void OdswiezListePostaci()
        {
            ListaPostaci.ItemsSource = null;
            ListaPostaci.ItemsSource = menadzerPostaci.Gry.Select(g => g.Gracz);
        }

        private void NowaPostac_Click(object sender, RoutedEventArgs e)
        {
            var creator = new StworzPostacWindow();
            if (creator.ShowDialog() == true && creator.CzyZatwierdzono)
            {
                try
                {
                    var nowaPostac = menadzerPostaci.StworzPostac(creator.ImiePostaci, creator.KlasaPostaci);
                    menadzerPostaci.DodajPostac(nowaPostac);
                    OdswiezListePostaci();
                }catch (PostacJuzIstniejeException)
                {
                    MessageBox.Show($"Postać o imieniu '{creator.ImiePostaci}' już istnieje! Wymyśl coś bardziej oryginalnego 😏",
                                    "Duplikat", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private void UsunPostacZListy_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is Postac postac)
            {
                if (MessageBox.Show($"Na pewno usunąć {postac.Imie}?", "Potwierdzenie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    menadzerPostaci.UsunGre(menadzerPostaci.Gry.Find(g => g.Gracz == postac));
                    OdswiezListePostaci();
                }
            }
        }

        private void Graj_Click(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Border)?.Tag is Postac postac)
            {
                menadzerPostaci.AktualnaGra = menadzerPostaci.Gry.Find(g => g.Gracz == postac);
                if (menadzerPostaci.AktualnaGra.Sklep != null && menadzerPostaci.AktualnaGra.Gracz != null)
                {
                    menadzerPostaci.AktualnaGra.Sklep.UstawPostac(menadzerPostaci.AktualnaGra.Gracz);
                    menadzerPostaci.AktualnaGra.Sklep.NaprawAsortyment();
                }
                GraView gra = new GraView(menadzerPostaci.AktualnaGra);
                gra.PokazProfil();
                MenadzerPostaciContent.Content = gra;
            }
        }
    }
}
