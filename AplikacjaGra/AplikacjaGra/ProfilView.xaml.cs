using BibliotekaGra.Gra;
using BibliotekaGra.Postacie;
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
using System.Xml;

namespace AplikacjaGra
{
    /// <summary>
    /// Logika interakcji dla klasy ProfilView.xaml
    /// </summary>
    public partial class ProfilView : UserControl
    {
        private GraView gra { get; set; }
        private Postac postac { get; set; }
        public ProfilView(GraView g)
        {
            InitializeComponent();
            gra = g;
            postac = gra.gra.Gracz;
            DataContext = postac;
            WczytajPlecak();
            WczytajZalozone();
            OdswiezHUD();
            if (postac.Zloto < postac.BazoweStatystyki.ZrecznoscCena)
                ZrecznoscBtn.IsEnabled = false;
            if (postac.Zloto < postac.BazoweStatystyki.SilaCena)
                SilaBtn.IsEnabled = false;
            if (postac.Zloto < postac.BazoweStatystyki.InteligencjaCena)
                InteligencjaBtn.IsEnabled = false;
            if (postac.Zloto < postac.BazoweStatystyki.WytrzymaloscCena)
                WytrzymaloscBtn.IsEnabled = false;
            if (postac.Zloto < postac.BazoweStatystyki.SzczescieCena)
                SzczescieBtn.IsEnabled = false;
        }
        private void OdswiezHUD()
        {
            XPText.Text = $"{postac.XP}/{postac.PotrzebneXPDoPoziomu}";
        }

        private void Zrecznosc_Click(object sender, RoutedEventArgs e)
        {
            postac.UlepszStatystyke("zrecznosc");
            gra.PokazProfil();
        }
        private void Sila_Click(object sender, RoutedEventArgs e)
        {
            postac.UlepszStatystyke("sila");
            gra.PokazProfil();
        }
        private void Inteligencja_Click(object sender, RoutedEventArgs e)
        {
            postac.UlepszStatystyke("inteligencja");
            gra.PokazProfil();
        }
        private void Wytrzymalosc_Click(object sender, RoutedEventArgs e)
        {
            postac.UlepszStatystyke("wytrzymalosc");
            gra.PokazProfil();
        }
        private void Szczescie_Click(object sender, RoutedEventArgs e)
        {
            postac.UlepszStatystyke("szczescie");
            gra.PokazProfil();
        }
        public void OdswiezPlecEq()
        {
            gra.PokazProfil();
        }
        public void WczytajPlecak()
        {
            Plecak.Content = new PlecakView(postac,2,5,this);
        }
        public void WczytajZalozone()
        {
            Ekwipunek.Content = new EkwipunekView(postac, this);
        }
    }
}
