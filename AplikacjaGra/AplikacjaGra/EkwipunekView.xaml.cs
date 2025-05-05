using BibliotekaGra.Ekwipunek;
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
    /// Logika interakcji dla klasy EkwipunekView.xaml
    /// </summary>
    public partial class EkwipunekView : UserControl
    {
        private Postac postac { get; set; }
        private EkwipunekPostaci ekwipunek { get; set; }
        private UserControl view { get; set; }
        public EkwipunekView(Postac p, UserControl v)
        {
            InitializeComponent();
            view = v;
            postac = p;
            ekwipunek = postac.Ekwipunek;
            DataContext = ekwipunek;
            generujEkwipunek();
        }

        public void generujEkwipunek()
        {
            Itemy.Children.Clear();

            foreach (var slot in ekwipunek.ZalozonePrzedmioty)
            {
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

                if (slot.Value != null)
                {
                    var binding = new Binding("SciezkaDoObrazka")
                    {
                        Source = slot.Value,
                        Converter = (IValueConverter)FindResource("SciezkaDoObrazkaConverter")
                    };

                    BindingOperations.SetBinding(obrazek, Image.SourceProperty, binding);
                    border.ToolTip = slot.Value.ToString(); 
                }
                else
                {
                    obrazek.Source = new BitmapImage(new Uri("/Obrazki/pusty_slot.png", UriKind.Relative));
                    border.ToolTip = $"{slot.Key} - pusty slot";
                }

                grid.Children.Add(obrazek);

                if (slot.Value != null)
                {
                    var button = new Button
                    {
                        Content = "✖",
                        Width = 20,
                        Height = 20,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Top,
                        Tag = slot.Key,
                        FontSize = 10,
                        Padding = new Thickness(0)
                    };
                    button.Click += Zdejmij_Click;

                    if (!ekwipunek.Plecak.CzyJestMiejsce)
                        button.IsEnabled = false;

                    grid.Children.Add(button);
                }

                border.Child = grid;
                Itemy.Children.Add(border);
            }
        }
        private void Zdejmij_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is SlotEkwipunku slot)
            {
                try
                {
                    postac.Ekwipunek.ZdejmijPrzedmiot(slot);
                    if (view is ProfilView profil)
                    {
                        profil.OdswiezPlecEq();
                    }else if(view is SklepView sklep)
                    {
                        sklep.OdswiezPlecEq();
                    }
                }
                catch (PlecakPelnyException ex)
                {
                    MessageBox.Show(ex.Message, "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
