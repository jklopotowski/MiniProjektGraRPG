using BibliotekaGra.Ekwipunek;
using BibliotekaGra.Postacie;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Wyprawy
{
    public static class GeneratorWypraw
    {
        private static readonly Random random = new();

        public static Wyprawa GenerujWyprawaPoziom(Postac postac, int i)
        {
            var lokalizacje = new[] {
            "Zamczysko",
            "Grota",
            "Bagna",
            "Dzikie wzgorze"
            };
            int poziom = postac.Poziom;
            var lokalizacja = lokalizacje[random.Next(lokalizacje.Length)];
            var minuty = i*Math.Max(1, (poziom / 4)+1);
            TimeSpan czas = new TimeSpan(0, minuty, 0);
            int energia = czas.Minutes * 5;
            Potwor potwór = GeneratorPotwór.GenerujPotwórPoziom(postac);
            Debug.WriteLine($"Potwor {potwór.Imie} {potwór.BazoweStatystyki} {potwór.StatystykiCalkowite}");
            var xp = 3*i* (postac.PotrzebneXPDoPoziomu / 10);
            var gold = i*(random.Next(Math.Max(10,(poziom - 1) * 10), (poziom + 2) * 10));
            return new Wyprawa(lokalizacja, czas, energia, potwór, xp, gold);
        }
    }
}
