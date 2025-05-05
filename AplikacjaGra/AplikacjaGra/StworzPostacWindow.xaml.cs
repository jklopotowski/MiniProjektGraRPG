using BibliotekaGra.Gra;
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

namespace AplikacjaGra
{
    /// <summary>
    /// Logika interakcji dla klasy StworzPostacWindow.xaml
    /// </summary>
    public partial class StworzPostacWindow : Window
    {
        public string ImiePostaci { get; private set; }
        public KlasaPostaci KlasaPostaci { get; private set; }
        public bool CzyZatwierdzono { get; private set; } = false;

        public StworzPostacWindow()
        {
            InitializeComponent();
            KlasaComboBox.ItemsSource = Enum.GetValues(typeof(KlasaPostaci));
        }

        private void Zatwierdz_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ImieTextBox.Text) || KlasaComboBox.SelectedItem == null)
            {
                MessageBox.Show("Wprowadź imię i wybierz klasę!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ImiePostaci = ImieTextBox.Text.Trim();
            KlasaPostaci = (KlasaPostaci)KlasaComboBox.SelectedItem;
            CzyZatwierdzono = true;

            this.DialogResult = true;
            this.Close();
        }

        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
