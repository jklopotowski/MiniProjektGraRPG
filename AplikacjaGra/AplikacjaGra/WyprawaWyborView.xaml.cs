using BibliotekaGra.Ekwipunek;
using BibliotekaGra.Gra;
using BibliotekaGra.Postacie;
using BibliotekaGra.Wyprawy;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AplikacjaGra
{
    /// <summary>
    /// Logika interakcji dla klasy WyprawaWybor.xaml
    /// </summary>
    public partial class WyprawaWyborView : UserControl
    {
        private GraView GraView { get; set; }
        private Gra gra { get; set; }
        public WyprawaWyborView(GraView grv)
        {
            InitializeComponent();
            GraView = grv;
            gra = GraView.gra;
            DataContext = gra;
            GenerujWyprawy();
        }

        private void GenerujWyprawy()
        {
            WyprawyPanel.Children.Clear();
            int i = 0;
            foreach (var wyprawa in gra.Lista.Lista)
            {
                var border = new Border
                {
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(5),
                    Margin = new Thickness(10),
                    Padding = new Thickness(10),
                    Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
                    Width = 350,
                    Height = 300
                };

                var stack = new StackPanel();

                var nazwa = new TextBlock
                {
                    Text = wyprawa.Nazwa,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Foreground = Brushes.Gold,
                    Margin = new Thickness(0, 0, 0, 5)
                };
                stack.Children.Add(nazwa);

                var potwor = new TextBlock
                {
                    Text = "Potwór: " + wyprawa.Potwor.Imie+"\nObrażenia:"+wyprawa.Potwor.Obrazenia,
                    Foreground = Brushes.LightGray
                };
                stack.Children.Add(potwor);

                var energia = new TextBlock
                {
                    Text = "Koszt energii: " + wyprawa.KosztEnergii,
                    Foreground = Brushes.LightGray
                };
                stack.Children.Add(energia);

                var czas = new TextBlock
                {
                    Text = "Czas trwania: " + wyprawa.CzasTrwania.Minutes + " min " + wyprawa.CzasTrwania.Seconds + " s",
                    Foreground = Brushes.LightGray
                };
                stack.Children.Add(czas);

                Image obrazek = new Image
                {
                    Width = 80,
                    Height = 80,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(10),
                    Stretch = Stretch.UniformToFill
                };

               
                var binding = new Binding("SciezkaDoObrazka")
                {
                    Source = wyprawa.Potwor,
                    Converter = (IValueConverter)FindResource("SciezkaDoObrazkaConverter")
                };
                BindingOperations.SetBinding(obrazek, Image.SourceProperty, binding);
                
                stack.Children.Add(obrazek);

                var btn = new Button
                {
                    Content = "Wyrusz na wyprawę",
                    Margin = new Thickness(0, 10, 0, 0),
                    Tag = i,
                    IsEnabled = gra.Gracz.Energia >= wyprawa.KosztEnergii,
                    VerticalAlignment = VerticalAlignment.Bottom
                };
                if (gra.Gracz.Energia<wyprawa.KosztEnergii) btn.ToolTip = "Brak energii";
                btn.Click += Wyrusz_Click;
                stack.Children.Add(btn);

                border.Child = stack;
                WyprawyPanel.Children.Add(border);
                i++;
            }
        }

        private void Wyrusz_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag.ToString(), out int index))
            {
                gra.AktualnaWyprawa = gra.Lista.Lista[index];
                gra.AktualnaWyprawa.ZacznijWyprawe(gra.Gracz);
                GraView.ZmienZawartoscNaWypraweWToku();
            }
        }
    }
}
