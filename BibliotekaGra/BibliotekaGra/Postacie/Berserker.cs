using BibliotekaGra.Ekwipunek;
using BibliotekaGra.Wyprawy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Postacie
{
    public class Berserker : Postac
    {
        public override string Klasa => "Berserker";
        public override string GlownaCecha => "Sila";
        public override int Obrazenia
        {
            get
            {
                var bron = Ekwipunek.ZalozonePrzedmioty[SlotEkwipunku.Bron];
                int obrazeniaBroni = bron != null ? bron.Statystyki.Obrazenia : 1;
                return obrazeniaBroni * 2 * (1 + StatystykiCalkowite.Sila / 10);
            }
        }
        public Berserker(string imie) : base(imie){
        }
        public override List<RundaWalki> WykonajAtak(Postac przeciwnik)
        {
            List<RundaWalki> logi = new();
            int szansaNaKolejnyAtak = 20; 
            int losKryt = Random.Next(0, 100);
            bool czyKryt = losKryt < SzansaNaKrytProc;
            int dmg = Obrazenia * (100 - przeciwnik.RedukcjaObrazenProc) / 100;
            int kryt = 0;
            if (czyKryt)
            {
                kryt = 1;
                dmg = (int)(dmg * 1.5);
            }
            logi.Add(new RundaWalki("Gracz", "Atak", $"{Imie} atakuje za {dmg} {(czyKryt ? "(KRYT!)" : "")}",dmg, kryt));
            logi.AddRange(przeciwnik.OtrzymajDmg(this, czyKryt));
            int losAtak = Random.Next(0, 100);
            if (losAtak < szansaNaKolejnyAtak)
            {
                do
                {
                    kryt = 0;
                    dmg = Obrazenia * (100 - przeciwnik.RedukcjaObrazenProc) / 100;
                    if (czyKryt)
                    {
                        kryt = 1;
                        dmg = (int)(dmg * 1.5);
                    }
                    logi.Add(new RundaWalki("Gracz", "Atak specjalny", $"{Imie} atakuje za {dmg} {(czyKryt ? "(KRYT!)" : "")}", dmg, kryt));
                    logi.AddRange(przeciwnik.OtrzymajDmg(this, czyKryt));
                    szansaNaKolejnyAtak = Math.Max(szansaNaKolejnyAtak - 5, 0); 
                    losAtak = Random.Next(0, 100);
                } while (losAtak < szansaNaKolejnyAtak);
            }
            return logi;
        }
    }
}
