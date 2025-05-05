using BibliotekaGra.Ekwipunek;
using BibliotekaGra.Postacie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Sklep
{
    public static class GeneratorPrzedmiotow
    {
        private static readonly Random random = new();

        public static Przedmiot GenerujPrzedmiotPoziom(Postac p)
        {
            var sloty = new[] {
            SlotEkwipunku.Bron,
            SlotEkwipunku.Helm,
            SlotEkwipunku.Napiersnik,
            SlotEkwipunku.Spodnie,
            SlotEkwipunku.Buty
        };
            int poziom = p.Poziom;
            var slot = sloty[random.Next(sloty.Length)];
            var staty = new Statystyki.Statystyki();
            string nazwa = "";
            string sciezkadoobrazka="";
            int wartosc = poziom * random.Next(5, 20); 

            if (slot != SlotEkwipunku.Bron)
            {
                staty.Pancerz = random.Next(Math.Max(10, poziom - 1 * 10), (poziom + 2) * 10);
                staty.Obrazenia = 0;
            }
            if (slot == SlotEkwipunku.Bron)
                staty.Obrazenia = random.Next(Math.Max(10, poziom - 2 * 10), (poziom + 3) * 10);

            if (LosujCzyDodacStatystyke(0.7))
                staty.Sila = random.Next(Math.Max(10, poziom - 2 * 10), (poziom + 3) * 10);
            else staty.Sila = 0;

            if (LosujCzyDodacStatystyke(0.7))
                staty.Zrecznosc = random.Next(Math.Max(10, poziom - 2 * 10), (poziom + 3) * 10);
            else staty.Zrecznosc = 0;

            if (LosujCzyDodacStatystyke(0.7))
                staty.Inteligencja = random.Next(Math.Max(10, poziom - 2 * 10), (poziom + 3) * 10);
            else staty.Inteligencja = 0;

            if (LosujCzyDodacStatystyke(0.7))
                staty.Szczescie = random.Next(Math.Max(10, poziom - 2 * 10), (poziom + 3) * 10);
            else staty.Szczescie = 0;

            if (LosujCzyDodacStatystyke(0.7))
                staty.Wytrzymalosc = random.Next(Math.Max(10, poziom - 2 * 10), (poziom + 3) * 10);
            else staty.Wytrzymalosc = 0;

            switch (slot)
            {
                case SlotEkwipunku.Bron:
                    nazwa = $"Broń";
                    sciezkadoobrazka="Obrazki/"+p.Klasa+"/bron.png";
                    break;
                case SlotEkwipunku.Helm:
                    nazwa = $"Hełm";
                    sciezkadoobrazka = "Obrazki/" + p.Klasa + "/helm.png";
                    break;
                case SlotEkwipunku.Napiersnik:
                    nazwa = $"Napierśnik";
                    sciezkadoobrazka = "Obrazki/" + p.Klasa + "/napiersnik.png";
                    break;
                case SlotEkwipunku.Spodnie:
                    nazwa = $"Spodnie";
                    sciezkadoobrazka = "Obrazki/" + p.Klasa + "/spodnie.png";
                    break;
                case SlotEkwipunku.Buty:
                    nazwa = $"Buty";
                    sciezkadoobrazka = "Obrazki/" + p.Klasa + "/buty.png";
                    break;
            }

            return new Przedmiot(nazwa, slot, staty, wartosc, sciezkadoobrazka);
        }
        private static bool LosujCzyDodacStatystyke(double szansa)
        {
            return random.NextDouble() < szansa;
        }
    }

}
