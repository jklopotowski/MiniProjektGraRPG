using BibliotekaGra.Ekwipunek;
using BibliotekaGra.Postacie;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Wyprawy
{
    public class Potwor : Postac
    {
        public override string Klasa => "Potwor";
        public override string GlownaCecha => "";
        public override int Obrazenia
        {
            get
            {
                return Math.Max(1,Poziom/2) * 2 * (5 + (StatystykiCalkowite.Sila+StatystykiCalkowite.Zrecznosc+StatystykiCalkowite.Inteligencja) / 5);
            }
        }
        public Potwor() : base() { }
        public Potwor(string nazwa, Statystyki.Statystyki staty, int poziom) : base(nazwa) {
            this.BazoweStatystyki = staty;
            this.Poziom= poziom;
            Debug.WriteLine(BazoweStatystyki.ToString());
        }

        public override List<RundaWalki> WykonajAtak(Postac przeciwnik)
        {
            List<RundaWalki> logi = new();
            int los = Random.Next(0, 100);
            bool czyKryt = los < SzansaNaKrytProc;
            int dmg = Obrazenia * (100 - przeciwnik.RedukcjaObrazenProc) / 100;
            int kryt = 0;
            if (czyKryt)
            {
                kryt = 1;
                dmg = (int)(dmg * 1.5);
            }
            logi.Add(new RundaWalki("Potwor", "Atak", $"{Imie} atakuje za {dmg} {(czyKryt ? "(KRYT!)" : "")}", dmg, kryt));
            logi.AddRange(przeciwnik.OtrzymajDmg(this, czyKryt));
            return logi;
        }
        public override List<RundaWalki> OtrzymajDmg(Postac atakujacy, bool czyKryt)
        {
            List<RundaWalki> logi = new();
            logi.AddRange(base.OtrzymajDmg(atakujacy, czyKryt));
            foreach(var log in logi)
            {
                log.Kto = "Potwor";
            }
            return logi;
        }
    }
}
