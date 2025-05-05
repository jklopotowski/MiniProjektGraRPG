using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotekaGra.Statystyki;

namespace BibliotekaGra.Ekwipunek
{
    public enum SlotEkwipunku
    {
        Bron,
        Helm,
        Napiersnik,
        Spodnie,
        Buty,
    }
    public class Przedmiot
    {
        public string Nazwa { get; set; }
        public SlotEkwipunku Slot { get; set; }
        public Statystyki.Statystyki Statystyki { get; set; }
        public int Wartosc { get; set; }
        public string SciezkaDoObrazka { get; set; }
        public Przedmiot(string nazwa, SlotEkwipunku slot, Statystyki.Statystyki statystyki, int wartosc, string sciezkaDoObrazka)
        {
            Nazwa = nazwa;
            Slot = slot;
            Statystyki = statystyki;
            Wartosc = wartosc;
            SciezkaDoObrazka = sciezkaDoObrazka;    
        }
        public Przedmiot() { }
        public override string ToString()
        {
            return $"{Nazwa}\nSlot: {Slot}\nStaty: \n{Statystyki}\nWartość: {Wartosc} zł";
        }
    }
}
