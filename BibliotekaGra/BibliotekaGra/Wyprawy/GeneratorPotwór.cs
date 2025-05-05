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
    public static class GeneratorPotwór
    {
        private static readonly Random random = new();

        public static Potwor GenerujPotwórPoziom(Postac postac)
        {
            var imiona = new[] { "Smok", "Ogr", "Ork", "Bazyliszek", "Troll" };
            var imie = imiona[random.Next(imiona.Length)];

            int poziomPostaci = postac.Poziom;
            int poziomPotwora = Math.Max(1, poziomPostaci + random.Next(-3, 4)); 

            double mnoznikStatystyk = 1 + Math.Log(poziomPotwora) * 0.2;
            Debug.WriteLine(mnoznikStatystyk);
            
            Statystyki.Statystyki staty = new()
            {
                Sila = GenerujStat(postac.StatystykiCalkowite.Sila, mnoznikStatystyk),
                Zrecznosc = GenerujStat(postac.StatystykiCalkowite.Zrecznosc, mnoznikStatystyk),
                Inteligencja = GenerujStat(postac.StatystykiCalkowite.Inteligencja, mnoznikStatystyk),
                Wytrzymalosc = GenerujStat(postac.Obrazenia, mnoznikStatystyk),
                Szczescie = GenerujStat(postac.StatystykiCalkowite.Szczescie, mnoznikStatystyk),
                Pancerz = GenerujStat(postac.StatystykiCalkowite.Pancerz, mnoznikStatystyk)
            };
            Debug.WriteLine(staty.ToString());
            Potwor potwor = new(imie, staty, poziomPotwora);
            return potwor;
        }

        private static int GenerujStat(int baza, double mnoznik)
        {
            int min = (int)Math.Max(1, (baza * mnoznik)/10);  
            int max = (int)(baza * mnoznik * 2);
            var stat = random.Next(min, max);
            Debug.WriteLine(baza+" "+mnoznik+" "+min+" "+max+" "+stat);
            return stat;
        }
    }
}
