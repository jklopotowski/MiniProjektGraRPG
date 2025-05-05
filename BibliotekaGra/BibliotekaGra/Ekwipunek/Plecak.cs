using BibliotekaGra.Wyjatki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Ekwipunek
{
    public class Plecak
    {
        public List<Przedmiot?> Sloty { get; set; }
        public int MaksRozmiar { get; set; } = 10;
        public bool CzyJestMiejsce => Sloty.Any(i => i == null);
        public Plecak()
        {
            Sloty = new List<Przedmiot?>(new Przedmiot?[MaksRozmiar]);
        }
        public void DodajPrzedmiot(Przedmiot przedmiot)
        {
            for (int i = 0; i < MaksRozmiar; i++)
            {
                if (Sloty[i] == null)
                {
                    Sloty[i] = przedmiot;
                    return;
                }
            }
            throw new PlecakPelnyException(); 
        }
        public void ZamienZEkwipunkiem(Przedmiot przedmiot, int indeks)
        {
            if (indeks >= 0 && indeks < MaksRozmiar)
            {
                Sloty[indeks] = przedmiot;
            }
        }
        public void UsunPrzedmiot(int indeks)
        {
            if (indeks >= 0 && indeks < MaksRozmiar)
            {
                Sloty[indeks] = null;
            }
        }

        public void PokazPlecak()
        {
            for (int i = 0; i < MaksRozmiar; i++)
            {
                var item = Sloty[i];
                Console.WriteLine($"Slot {i + 1}: {(item != null ? item.Nazwa : "pusty")}");
            }
        }
    }

}
