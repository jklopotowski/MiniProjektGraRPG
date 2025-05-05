using BibliotekaGra.Postacie;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Wyprawy
{
    public class RundaWalki
    {
        public string Kto { get; set; }
        public string Akcja { get; set; }
        public string Opis { get; set; }
        public int Dmg { get; set; }
        public int CzyKryt { get; set; }

        public RundaWalki(string kto, string akcja, string opis, int dmg, int czyKryt)
        {
            Kto = kto;
            Akcja = akcja;
            Opis = opis;
            Dmg = dmg;
            CzyKryt = czyKryt;
        }
        public override string ToString()
        {
            return $"{Kto} | {Akcja} | {Opis}";
        }
    }

    public class WalkaManager
    {
        public static (bool wygrana, List<RundaWalki> log) PrzeprowadzWalke(Postac postac, Potwor potwor)
        {
            var log = new List<RundaWalki>();

            postac.Zdrowie = postac.MaxZdrowie;
            potwor.Zdrowie = potwor.MaxZdrowie;

            bool turaGracza = true;

            while (postac.CzyZyje && potwor.CzyZyje)
            {
                if (turaGracza)
                {
                    var akcje = postac.WykonajAtak(potwor);
                    log.AddRange(akcje);
                    foreach(var ak in akcje)
                    {
                        Debug.WriteLine(ak.ToString());
                    }
                }
                else
                {
                    var akcje = potwor.WykonajAtak(postac);
                    log.AddRange(akcje);
                    foreach (var ak in akcje)
                    {
                        Debug.WriteLine(ak.ToString());
                    }
                }
                if (!postac.CzyZyje || !potwor.CzyZyje)
                    break;
                turaGracza = !turaGracza;
            }

            bool wygrana = postac.CzyZyje;
            Debug.WriteLine($"Wygrana gracza: {wygrana}");
            postac.Zdrowie = postac.MaxZdrowie;
            potwor.Zdrowie = potwor.MaxZdrowie;
            return (wygrana, log);
        }
    }
}
