using BibliotekaGra.Ekwipunek;
using BibliotekaGra.Postacie;
using BibliotekaGra.Sklep;
using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace BibliotekaGra.Wyprawy
{
    public class Wyprawa
    {
        private readonly Random random = new();
        public delegate void DelegatWyprawa();
        public event DelegatWyprawa OnWyprawaCompleted;
        public string Nazwa { get; }
        public TimeSpan CzasTrwania { get; }
        public int KosztEnergii { get; }
        public Potwor Potwor { get; }
        public int NagrodaXP { get; }
        public int NagrodaGold { get; }
        public bool CzyNagrodaPrzyznana { get; private set; } = false;

        [JsonInclude]
        public DateTime Rozpoczecie { get; private set; }

        public bool CzyZakonczona => DateTime.Now >= Rozpoczecie + CzasTrwania;

        public string CzasDoZakonczenia
        {
            get
            {
                var roznica = ((Rozpoczecie + CzasTrwania) - DateTime.Now);
                return $"{roznica.Hours:D2}:{roznica.Minutes:D2}:{roznica.Seconds:D2}";
            }
        }

        public int ProcentUkonczenia
        {
            get
            {
                var czasMiniony = DateTime.Now - Rozpoczecie;
                double procent = (czasMiniony.TotalSeconds / CzasTrwania.TotalSeconds) * 100;
                return (int)Math.Min(100, procent);
            }
        }

        [JsonConstructor]
        public Wyprawa(string nazwa, TimeSpan czasTrwania, int kosztEnergii, Potwor potwor, int nagrodaXP, int nagrodaGold)
        {
            Nazwa = nazwa;
            CzasTrwania = czasTrwania;
            KosztEnergii = kosztEnergii;
            Potwor = potwor;
            NagrodaXP = nagrodaXP;
            NagrodaGold = nagrodaGold;
        }

        public void ZacznijWyprawe(Postac postac)
        {
            postac.ZuzyjEnergie(KosztEnergii);
            Rozpoczecie = DateTime.Now;
        }
        public (bool wygrana, List<RundaWalki> log, string nagrody) RozegrajWyprawe(Postac postac)
        {
            var wynik = WalkaManager.PrzeprowadzWalke(postac, Potwor);
                string nagrody = wynik.wygrana ? Zakoncz(postac) : "Brak nagród";
            Debug.WriteLine("Koniec rozegrajwyprawe");
            return (wynik.wygrana, wynik.log, nagrody);
        }
        public string Zakoncz(Postac postac)
        {
            if (CzyNagrodaPrzyznana)
                return "Nagrody już odebrane.";

            CzyNagrodaPrzyznana = true;

            Debug.WriteLine("wlazl do zakoncz");
            string nagrody = $"{NagrodaXP} XP\n{NagrodaGold} gold\n";
            postac.DodajDoswiadczenie(NagrodaXP);
            Debug.WriteLine("Dodal xp");
            postac.Zloto += NagrodaGold;
            if (postac.Ekwipunek.Plecak.CzyJestMiejsce) { 
                int los = random.Next(1, 10000);
                if (los > 8000)
                {
                    Debug.WriteLine("dodaje item");
                    var przedmiot = GeneratorPrzedmiotow.GenerujPrzedmiotPoziom(postac);
                    postac.Ekwipunek.Plecak.DodajPrzedmiot(przedmiot);
                    nagrody += $"{przedmiot.Nazwa}";
                }
            }
            Debug.WriteLine("Wychodzi z zakoncz");
            return nagrody;
        }

        public override string ToString()
        {
            return $"Nazwa: {Nazwa}, Czas trwania {CzasTrwania}, Koszt energii: {KosztEnergii}";
        }
        public void wywolajEvent()
        {
            OnWyprawaCompleted?.Invoke();
        }
    }
}
