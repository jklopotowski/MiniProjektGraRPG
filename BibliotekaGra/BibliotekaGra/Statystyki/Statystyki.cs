using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Statystyki
{
    public class Statystyki
    {
        public int Sila { get; set; }
        public int SilaCena { get; set; }
        public int Zrecznosc { get; set; }
        public int ZrecznoscCena { get; set; }
        public int Inteligencja { get; set; }
        public int InteligencjaCena { get; set; }
        public int Wytrzymalosc { get; set; }
        public int WytrzymaloscCena { get; set; }
        public int Szczescie { get; set; }
        public int SzczescieCena { get; set; }
        public int Pancerz { get; set; }
        public int Obrazenia { get; set; }

        public Statystyki()
        {
            Sila = 1;
            SilaCena = 1;
            Zrecznosc = 1;
            ZrecznoscCena = 1;
            Inteligencja = 1;
            InteligencjaCena = 1;
            Wytrzymalosc = 1;
            WytrzymaloscCena = 1;
            Szczescie = 1;
            SzczescieCena = 1;
            Pancerz = 1;
            Obrazenia = 1;
        }

        public Statystyki(int sila, int zrecznosc, int inteligencja, int wytrzymalosc, int szczescie, int pancerz, int obrazenia)
        {
            Sila = sila;
            SilaCena = 1;
            Zrecznosc = zrecznosc;
            ZrecznoscCena = 1;
            Inteligencja = inteligencja;
            InteligencjaCena = 1;
            Wytrzymalosc = wytrzymalosc;
            WytrzymaloscCena = 1;
            Szczescie = szczescie;
            SzczescieCena = 1;
            Pancerz = pancerz;
            Obrazenia = obrazenia;
        }

        public void Dodaj(Statystyki inne)
        {
            Sila += inne.Sila;
            Zrecznosc += inne.Zrecznosc;
            Inteligencja += inne.Inteligencja;
            Wytrzymalosc += inne.Wytrzymalosc;
            Szczescie += inne.Szczescie;
            Pancerz += inne.Pancerz;
            Obrazenia+= inne.Obrazenia;
        }

        public Statystyki Kopiuj()
        {
            return new Statystyki(Sila, Zrecznosc, Inteligencja, Wytrzymalosc, Szczescie, Pancerz, Obrazenia);
        }

        public override string ToString()
        {
            string str = "";

            if (Sila != 0)
                str+=$"Siła: {Sila}\n";
            if (Zrecznosc != 0)
                str += $"Zręczność: {Zrecznosc}\n";
            if (Inteligencja != 0)
                str += $"Inteligencja: {Inteligencja}\n";
            if (Wytrzymalosc != 0)
                str += $"Wytrzymałość: {Wytrzymalosc}\n";
            if (Szczescie != 0)
                str += $"Szczęście: {Szczescie}\n";
            if (Pancerz != 0)
                str += $"Pancerz: {Pancerz}\n";
            if (Obrazenia != 0)
                str += $"Obrażenia: {Obrazenia}\n";

            if (str.Equals("")) str = "Brak bonusów";

            return str;
        }
    }
}
