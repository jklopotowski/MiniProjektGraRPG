using BibliotekaGra.Gra;
using BibliotekaGra.Postacie;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace AplikacjaGra
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MenadżerPostaci menadzerPostaci;
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
            menadzerPostaci = new MenadżerPostaci();
            SetContent(new MenadzerPostaciView(menadzerPostaci));
        }
        public void PokazMenadzerPostaci()
        {
            menadzerPostaci.AktualnaGra = null;
            SetContent(new MenadzerPostaciView(menadzerPostaci));
        }
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            var wynik = MessageBox.Show("Chcesz zapisać grę przed wyjściem?", "Zapis gry", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if (wynik == MessageBoxResult.Yes)
            {
                menadzerPostaci.ZapiszWszystko();
            }
            else if (wynik == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
        public void ZapiszWszystko()
        {
            menadzerPostaci.ZapiszWszystko();
        }
        public void PokazWalke()
        {
            var gra = menadzerPostaci.AktualnaGra;
            gra.AktualnaWyprawa.OnWyprawaCompleted += () =>
            {
                gra.AktualnaWyprawa = null;
                gra.Lista.OdswiezListe();
                var gc = new GraView(gra);
                gc.SetContent(new WyprawaWyborView(gc));
                SetContent(gc);
            }; 
            var kontrolkaWalki = new WalkaView(gra.AktualnaWyprawa, gra.Gracz);
            SetContent(kontrolkaWalki);
        }
        public void PokazGre()
        {
            SetContent(new GraView(menadzerPostaci.AktualnaGra));
        }
        public void SetContent(UserControl newContent)
        {
            if (MainWindowContent.Content is IDisposable oldView)
                oldView.Dispose();

            MainWindowContent.Content = newContent;
        }
    }
}