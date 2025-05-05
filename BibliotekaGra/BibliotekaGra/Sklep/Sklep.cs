using BibliotekaGra.Ekwipunek;
using BibliotekaGra.Gra;
using BibliotekaGra.Postacie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using BibliotekaGra.Wyjatki;

namespace BibliotekaGra.Sklep
{
    public class Sklep
    {
        public int MaksRozmiar { get; set; } = 5;
        public List<Przedmiot?> Asortyment { get; set; }

        private Random random = new();

        private DateTime ostatnieDarmoweOdnowienie;

        [JsonIgnore]
        public Postac AktualnaPostac { get; set; }

        public bool CzyDarmoweOdswiezenie => (DateTime.Now - ostatnieDarmoweOdnowienie).TotalMinutes >= 60;

        public DateTime OstatnieDarmoweOdnowienie
        {
            get => ostatnieDarmoweOdnowienie;
            set => ostatnieDarmoweOdnowienie = value;
        }

        public Sklep()
        { 
            random = new Random();
        }

        public Sklep(Postac postac)
        {
            AktualnaPostac = postac;
            Asortyment = new List<Przedmiot?>(new Przedmiot?[MaksRozmiar]);
            random = new Random();
        }

        public void UstawPostac(Postac postac)
        {
            AktualnaPostac = postac;
        }

        public bool OdnowAsortyment(bool wymuszenieZaGolda = false)
        {
            if (AktualnaPostac == null)
                throw new InvalidOperationException("Nie przypisano postaci do sklepu.");

            var teraz = DateTime.Now;

            if (!wymuszenieZaGolda)
            {
                if ((teraz - ostatnieDarmoweOdnowienie).TotalMinutes < 60)
                {
                    return false;
                }
                ostatnieDarmoweOdnowienie = teraz;
            }
            else
            {
                int koszt = 100 * Math.Max(1,AktualnaPostac.Poziom / 5);
                if (AktualnaPostac.Zloto < koszt)
                    throw new ZaMaloZlotaException($"Potrzebujesz {koszt} złota, a masz tylko {AktualnaPostac.Zloto}.");

                AktualnaPostac.Zloto -= koszt;
            }

            for (int i = 0; i < MaksRozmiar; i++)
            {
                Asortyment[i] = GeneratorPrzedmiotow.GenerujPrzedmiotPoziom(AktualnaPostac);
            }

            return true;
        }

        public void KupPrzedmiot(int index)
        {
            if (index < 0 || index >= MaksRozmiar)
                throw new ArgumentOutOfRangeException(nameof(index), "Niepoprawny numer przedmiotu w asortymencie.");

            var item = Asortyment[index];
            if (item == null)
                throw new InvalidOperationException("W podanym miejscu nie ma przedmiotu.");

            if (AktualnaPostac.Zloto < item.Wartosc)
                throw new ZaMaloZlotaException("Nie masz tyle zlota.");

            if (!AktualnaPostac.Ekwipunek.Plecak.CzyJestMiejsce)
                throw new PlecakPelnyException("Nie masz miejsca w plecaku.");

            AktualnaPostac.Zloto -= item.Wartosc;
            item.Wartosc = Math.Max(item.Wartosc / 2, 1);
            AktualnaPostac.Ekwipunek.Plecak.DodajPrzedmiot(item);
            Asortyment[index] = null;
        }

        public string WypiszAsortyment()
        {
            string a = "Asortyment sklepu:\n";
            for (int i = 0; i < MaksRozmiar; i++)
            {
                a += $"- {i}. {(Asortyment[i] != null ? Asortyment[i].ToString() : "[pusty slot]")}\n";
            }
            return a;
        }
        public void NaprawAsortyment()
        {
            for (int i = 0; i < MaksRozmiar; i++)
            {
                if (Asortyment[i] == null)
                {
                    Asortyment[i] = GeneratorPrzedmiotow.GenerujPrzedmiotPoziom(AktualnaPostac);
                }
            }
        }
    }
}
