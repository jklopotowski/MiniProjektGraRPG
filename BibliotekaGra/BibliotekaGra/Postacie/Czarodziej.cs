using BibliotekaGra.Ekwipunek;
using BibliotekaGra.Wyprawy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Postacie
{
    public class Czarodziej : Postac
    {
        public override string Klasa => "Czarodziej";
        public override string GlownaCecha => "Inteligencja";
        public override int Obrazenia
        {
            get
            {
                var bron = Ekwipunek.ZalozonePrzedmioty[SlotEkwipunku.Bron];
                int obrazeniaBroni = bron != null ? bron.Statystyki.Obrazenia : 1;
                return (obrazeniaBroni * 2 * (1 + StatystykiCalkowite.Inteligencja / 10) *60)/55;
            }
        }
        public Czarodziej(string imie) : base(imie) { }
        public override List<RundaWalki> WykonajAtak(Postac przeciwnik)
        {
            List<RundaWalki> logi = new();
            int los = Random.Next(0, 100);
            bool czyKryt = los < SzansaNaKrytProc;
            int kryt = 0;
            int dmg = Obrazenia * (100 - przeciwnik.RedukcjaObrazenProc) / 100;
            if (czyKryt)
            {
                kryt = 1;
                dmg = (int)(dmg * 1.5);
            }
            logi.Add(new RundaWalki("Gracz", "Atak", $"{Imie} atakuje za {dmg} {(czyKryt ? "(KRYT!)" : "")}",dmg,kryt));
            logi.AddRange(przeciwnik.OtrzymajDmg(this, czyKryt));
            return logi;
        }
    }
}
