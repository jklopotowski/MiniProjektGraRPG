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
    public class Lucznik : Postac
    {
        public override string Klasa => "Lucznik";
        public override string GlownaCecha => "Zrecznosc";
        public override int Obrazenia
        {
            get
            {
                var bron = Ekwipunek.ZalozonePrzedmioty[SlotEkwipunku.Bron];
                int obrazeniaBroni = bron != null ? bron.Statystyki.Obrazenia : 1;
                return (obrazeniaBroni * 2 * (1 + StatystykiCalkowite.Zrecznosc / 10)*7)/9;
            }
        }
        public Lucznik(string imie) : base(imie) { }
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
            logi.Add(new RundaWalki("Gracz","Atak",$"{Imie} atakuje za {dmg} {(czyKryt ? "(KRYT!)" : "")}", dmg,kryt));
            logi.AddRange(przeciwnik.OtrzymajDmg(this, czyKryt));
            return logi;
        }
        public override List<RundaWalki> OtrzymajDmg(Postac atakujacy, bool czyKryt)
        {
            List<RundaWalki> logi = new();
            int los = Random.Next(0, 100);
            if (los < 40)
            {
                logi.Add(new RundaWalki("Gracz", "Unik", $"{Imie} wykonuje unik i nie dostaje żadnych obrażeń",0,0));
            }
            else
            {
                logi.AddRange(base.OtrzymajDmg(atakujacy, czyKryt));
            }
            return logi;
        }
        public List<RundaWalki> OtrzymajDmgBezUnik(Postac atakujacy, bool czyKryt)
        {
            List<RundaWalki> logi = new();
            logi.AddRange(base.OtrzymajDmg(atakujacy, czyKryt));
            return logi;
        }
    }
}
