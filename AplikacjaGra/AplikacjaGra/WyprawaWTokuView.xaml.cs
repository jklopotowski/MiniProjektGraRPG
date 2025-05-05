using AplikacjaGra;
using BibliotekaGra.Sklep;
using BibliotekaGra.Wyprawy;
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
using System.Windows.Threading;

namespace AplikacjaGra
{
    /// <summary>
    /// Logika interakcji dla klasy WyprawaWTokuView.xaml
    /// </summary>
    public partial class WyprawaWTokuView : UserControl, IDisposable
    {
        private DispatcherTimer timer;
        private GraView graView;
        private Wyprawa wyprawa;
        private bool walkaPokazana = false;
        public WyprawaWTokuView(GraView gv)
        {
            InitializeComponent();
            graView = gv;
            wyprawa = gv.gra.AktualnaWyprawa;
            DataContext = wyprawa;
            InitTimerOdliczanie();
            this.Unloaded += (s, e) => Dispose();
        }
        private void InitTimerOdliczanie()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            Timer_Tick(null, null);
        }
        public void Dispose()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Tick -= Timer_Tick;
                timer = null;
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!wyprawa.CzyZakonczona)
            {
                CzasWyp.Text = wyprawa.CzasDoZakonczenia;
                CzasBar.Value = wyprawa.ProcentUkonczenia;
            }
            else
            {
                if (walkaPokazana) return;
                walkaPokazana = true;

                Dispose();
                graView.PokazWalke();
            }
        }
    }
}
