using BibliotekaGra.Ekwipunek;
using BibliotekaGra.Postacie;
using BibliotekaGra.Sklep;
using BibliotekaGra.Wyprawy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Gra
{
    public class Gra
    {
        public Postac Gracz { get; set; }
        public Sklep.Sklep Sklep { get; set; }
        public Wyprawa AktualnaWyprawa { get; set; }
        public ListaWypraw Lista { get; set; }
        public Gra(Postac gracz)
        {
            Gracz = gracz;
            Sklep = new Sklep.Sklep(gracz);
            AktualnaWyprawa = null;
            Lista = new ListaWypraw(Gracz);
        }

        public void Start()
        {
            int wybor;
            do
            {
                Console.WriteLine("Co chcesz zrobic:\n1-ulepszyc statystyki postaci\n2-wyslac na wyprawe\n3-otworzyc sklep\n4-Wylogowac");
                if (int.TryParse(Console.ReadLine(), out wybor))
                {
                    switch (wybor)
                    {
                        case 1:
                            {
                                Console.WriteLine($"Jaka statystyke ulepszyc:\nSila {Gracz.BazoweStatystyki.Sila}lvl koszt {Gracz.BazoweStatystyki.SilaCena} gold\nZrecznosc {Gracz.BazoweStatystyki.Zrecznosc}lvl koszt {Gracz.BazoweStatystyki.ZrecznoscCena} gold\nInteligencja {Gracz.BazoweStatystyki.Inteligencja}lvl koszt {Gracz.BazoweStatystyki.InteligencjaCena} gold\nWtrzymalosc {Gracz.BazoweStatystyki.Wytrzymalosc}lvl koszt {Gracz.BazoweStatystyki.WytrzymaloscCena} gold\nSzczescie {Gracz.BazoweStatystyki.Szczescie}lvl koszt {Gracz.BazoweStatystyki.Szczescie} gold");
                                string stat = Console.ReadLine();
                                Gracz.UlepszStatystyke(stat);
                                break;
                            }
                        case 2:
                            {
                                if (AktualnaWyprawa == null)
                                {
                                    Console.WriteLine(Lista.WypiszWyprawy());
                                    int wyp = int.Parse(Console.ReadLine());
                                    AktualnaWyprawa = Lista.Lista[wyp];
                                    AktualnaWyprawa.ZacznijWyprawe(Gracz);
                                }
                                else
                                {
                                    if(!AktualnaWyprawa.CzyZakonczona)
                                        Console.WriteLine($"Jesteś aktualnie na wyprawie\nZakończenie za {AktualnaWyprawa.CzasDoZakonczenia}");
                                    else
                                    {
                                        AktualnaWyprawa.Zakoncz(Gracz);
                                        AktualnaWyprawa = null;
                                        Lista.OdswiezListe();
                                    }
                                }
                                break;
                            }
                        case 3:
                            {
                                Console.WriteLine(Sklep.WypiszAsortyment());
                                int wyb = int.Parse(Console.ReadLine());
                                Sklep.KupPrzedmiot(wyb);
                                break;
                            }
                    }
                }
            } while (wybor != 4);
        }
    }
}
