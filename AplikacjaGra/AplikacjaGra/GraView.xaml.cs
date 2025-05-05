using BibliotekaGra.Gra;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using System.Windows.Shapes;
using BibliotekaGra.Sklep;

namespace AplikacjaGra
{
    public partial class GraView : UserControl
    {
        private DispatcherTimer MenuTimer;
        public Gra gra { get; set; }
        public GraView(Gra gr)
        {
            InitializeComponent();
            gra = gr;
            DataContext = gra;
            gra.Gracz.AktualizujEnergie();
            OdswiezHUD();
            OdnawiajEnergie();
            MenuTimer = new DispatcherTimer();
            MenuTimer.Interval = TimeSpan.FromSeconds(1);
            MenuTimer.Tick += (s, e) => OdnawiajEnergie();
            MenuTimer.Tick += (s, e) => OdswiezHUD();
            MenuTimer.Start();
        }
        private void OdnawiajEnergie()
        {
            if (gra.Gracz.Energia < gra.Gracz.MaksEnergia)
            {
                var czasOdAktualizacji = DateTime.Now - gra.Gracz.OstatniaAktualizacjaEnergii;
                if (czasOdAktualizacji.Minutes>=1)
                {
                    gra.Gracz.AktualizujEnergie();
                }
                else { 
                int sekundyDoPelnejMinuty = 60 - (int)czasOdAktualizacji.TotalSeconds % 60;
                EnergiaOdnowiSieZa.Text = $"Odnowienie za {sekundyDoPelnejMinuty}s";
                }
            }
            else
            {
                EnergiaOdnowiSieZa.Text = "";
            }
        }
        private void OdswiezHUD()
        {
            EnergiaProgress.Value = gra.Gracz.Energia;
            EnergiaText.Text = $"{gra.Gracz.Energia}/{gra.Gracz.MaksEnergia}";
            XPText.Text = $"{gra.Gracz.XP}/{gra.Gracz.PotrzebneXPDoPoziomu}";
            XPProgress.Value = gra.Gracz.XP;
            ZlotoText.Text = $"{gra.Gracz.Zloto}";
            LvlText.Text = $"{gra.Gracz.Poziom}";
        }
        private void Profil_Click(object sender, RoutedEventArgs e)
        {
            SetContent(new ProfilView(this));
        }
        private void Sklep_Click(object sender, RoutedEventArgs e)
        {
            SetContent(new SklepView(gra));
        }
        public void PokazProfil()
        {
            SetContent(new ProfilView(this));
        }
        private async void Wyprawa_Click(object sender, RoutedEventArgs e)
        {
            if (gra.AktualnaWyprawa != null)
            {
                if (gra.AktualnaWyprawa.CzyZakonczona)
                {
                    PokazWalke();
                }
                else
                {
                    SetContent(new WyprawaWTokuView(this));
                }
            }
            else
            {
               SetContent(new WyprawaWyborView(this));
            }
        }
        private void Wyloguj_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow)?.PokazMenadzerPostaci();
            ZapiszWszystko();
        }
        public void ZmienZawartoscNaWypraweWToku()
        {
            SetContent(new WyprawaWTokuView(this));
        }
        private void ZapiszWszystko()
        {
            (Application.Current.MainWindow as MainWindow)?.ZapiszWszystko();
        }
        public void PokazWalke()
        {
            (Application.Current.MainWindow as MainWindow)?.PokazWalke();
        }
        public void SetContent(UserControl newContent)
        {
            if (GraContent.Content is IDisposable oldView)
                oldView.Dispose();

            GraContent.Content = newContent;
        }
    }
}
