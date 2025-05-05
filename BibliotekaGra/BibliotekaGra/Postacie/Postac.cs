using BibliotekaGra.Ekwipunek;
using BibliotekaGra.Wyjatki;
using BibliotekaGra.Wyprawy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Postacie
{
    public abstract class Postac
    {
        public Random Random { get; set; } = new Random();
        public string Imie { get; set; }
        public int Poziom { get; set; }
        public int XP { get; set; }
        public int PotrzebneXPDoPoziomu { get; set; }
        public int Energia { get; set; }
        public int MaksEnergia { get; set; }
        public int EnergiaNaMinute { get; set; } = 1;
        public DateTime OstatniaAktualizacjaEnergii { get; set; }
        public int Zloto { get; set; }
        public virtual int Obrazenia { get; set; }
        public string XPDoMax => $"{XP}/{PotrzebneXPDoPoziomu}";
        public EkwipunekPostaci Ekwipunek { get; set; }
        public Statystyki.Statystyki BazoweStatystyki { get; set; }
        public Statystyki.Statystyki StatystykiCalkowite
        {
            get
            {
                var suma = BazoweStatystyki.Kopiuj();
                Ekwipunek.SumujBonusy(suma);
                return suma;
            }
        }
        public int MaxZdrowie => StatystykiCalkowite.Wytrzymalosc * 10;
        public int Zdrowie { get; set; }
        public int RedukcjaObrazenProc => Math.Min(StatystykiCalkowite.Pancerz*100 / 200, 50);
        public int SzansaNaKrytProc => Math.Min(StatystykiCalkowite.Szczescie*100 / 150, 50);
        public abstract List<RundaWalki> WykonajAtak(Postac postac);
        public abstract string Klasa { get; }
        public abstract string GlownaCecha { get; }
        public string SciezkaDoObrazka { get; set; }
        public bool CzyZyje => Zdrowie > 0;
        public Postac()
        {
            Imie = "Anonim";
            Poziom = 1;
            XP = 0;
            PotrzebneXPDoPoziomu = 50;
            Energia = 100;
            MaksEnergia = 100;
            Zloto = 0;
            Ekwipunek = new();
            BazoweStatystyki = new();
            Zdrowie = MaxZdrowie;
            UstawSciezkeDoObrazka();
        }
        public Postac(string imie)
        {
            Imie = imie;
            Poziom = 1;
            XP = 0;
            PotrzebneXPDoPoziomu = 50;
            Energia = 100;
            MaksEnergia = 100;
            Zloto = 10;
            Ekwipunek = new();
            BazoweStatystyki = new();
            Zdrowie = MaxZdrowie;
            UstawSciezkeDoObrazka();
        }
        public void DodajDoswiadczenie(int ilosc)
        {
            XP += ilosc;
            while (XP >= PotrzebneXPDoPoziomu)
                LevelUp(); 
            
        }
        public void LevelUp()
        {
            Debug.WriteLine("leveluje");
                Poziom++;
                XP -= PotrzebneXPDoPoziomu;
                PotrzebneXPDoPoziomu = (PotrzebneXPDoPoziomu * 120) / 100;
            Debug.WriteLine("Dodaje staty");
                BazoweStatystyki.Pancerz += Math.Max(1,Poziom/5);
                BazoweStatystyki.Sila += Math.Max(1, Poziom / 5);
                BazoweStatystyki.Inteligencja += Math.Max(1, Poziom / 5);
                BazoweStatystyki.Zrecznosc += Math.Max(1, Poziom / 5);
                BazoweStatystyki.Wytrzymalosc += Math.Max(1, Poziom / 5);
                BazoweStatystyki.Szczescie += Math.Max(1, Poziom / 5);
            Debug.WriteLine("wcyhdzoi z levelup");
        }
        public virtual void UlepszStatystyke(string nazwaStatystyki)
        {
            switch (nazwaStatystyki.ToLower())
            {
                case "sila":
                    {
                        if (Zloto >= BazoweStatystyki.SilaCena)
                        {
                            Zloto -= BazoweStatystyki.SilaCena;
                            BazoweStatystyki.SilaCena++;
                            BazoweStatystyki.Sila++;
                        }
                        else throw new ZaMaloZlotaException("Nie posiadasz odpowiedniej ilości złota");
                        break;
                    } 
                case "zrecznosc":
                    {
                        if (Zloto >= BazoweStatystyki.ZrecznoscCena)
                        {
                            Zloto -= BazoweStatystyki.ZrecznoscCena;
                            BazoweStatystyki.ZrecznoscCena++;
                            BazoweStatystyki.Zrecznosc++;
                        }
                        else throw new ZaMaloZlotaException("Nie posiadasz odpowiedniej ilości złota");
                        break;
                    }
                case "inteligencja":
                    {
                        if (Zloto >= BazoweStatystyki.InteligencjaCena)
                        {
                            Zloto -= BazoweStatystyki.InteligencjaCena;
                            BazoweStatystyki.InteligencjaCena++;
                            BazoweStatystyki.Inteligencja++;
                        }
                        else throw new ZaMaloZlotaException("Nie posiadasz odpowiedniej ilości złota");
                        break;
                    }
                case "wytrzymalosc":
                    {
                        if (Zloto >= BazoweStatystyki.WytrzymaloscCena)
                        {
                            Zloto -= BazoweStatystyki.WytrzymaloscCena;
                            BazoweStatystyki.WytrzymaloscCena++;
                            BazoweStatystyki.Wytrzymalosc++;
                        }
                        else throw new ZaMaloZlotaException("Nie posiadasz odpowiedniej ilości złota");
                        break;
                    }
                case "szczescie":
                    {
                        if (Zloto >= BazoweStatystyki.SzczescieCena)
                        {
                            Zloto -= BazoweStatystyki.SzczescieCena;
                            BazoweStatystyki.SzczescieCena++;
                            BazoweStatystyki.Szczescie++;
                        }
                        else throw new ZaMaloZlotaException("Nie posiadasz odpowiedniej ilości złota");
                        break;
                    }
                default: throw new ArgumentException("Nieprawidłowa statystyka");
            }
        }
        public void OdnowEnergie(int ile)
        {

            Energia = Math.Min(Energia + ile, MaksEnergia);
        }

        public void AktualizujEnergie()
        {
            if (Energia >= MaksEnergia)
                return;

            var czasMiniety = DateTime.Now - OstatniaAktualizacjaEnergii;
            int odzyskanaEnergia = (int)(czasMiniety.TotalMinutes * EnergiaNaMinute);

            if (odzyskanaEnergia > 0)
            {
                Energia = Math.Min(MaksEnergia, Energia + odzyskanaEnergia);
                OstatniaAktualizacjaEnergii = DateTime.Now;
            }
        }
        public void ZuzyjEnergie(int ile)
        {
            if (Energia < ile)
                throw new BrakEnergiiException("Nie posiadasz wystarczającej ilości energii do tej wyprawy.");

            bool mialaPelna = Energia == MaksEnergia;
            Energia -= ile;

            if (mialaPelna)
                OstatniaAktualizacjaEnergii = DateTime.Now;
        }
        public virtual List<RundaWalki> OtrzymajDmg(Postac atakujacy, bool czyKryt)
        {
            List<RundaWalki> logi = new();
            int dmg = atakujacy.Obrazenia * (100 - RedukcjaObrazenProc) / 100;
            int kryt = 0;
            if (czyKryt)
            {
                kryt = 1;
                dmg = (int)(dmg * 1.5);
            }
            Zdrowie -= dmg;
            logi.Add(new RundaWalki("Gracz", "Obrazenia", $"{Imie} otrzymuje {dmg} obrażeń {(czyKryt ? "(KRYT!)" : "")}", dmg, kryt));
            return logi;
        }
        public override string ToString()
        {
            return $"Postać klasy {Klasa} imie {Imie} Poziom {Poziom}";
        }
        public void SprzedajPrzedmiot(int indeksWPlecaku)
        {
            var plecak = Ekwipunek.Plecak;

            if (plecak.Sloty[indeksWPlecaku] != null)
            {
                var przedmiot = plecak.Sloty[indeksWPlecaku];
                Zloto += przedmiot.Wartosc;
                Ekwipunek.Plecak.UsunPrzedmiot(indeksWPlecaku);
            }
        }
        public void UstawSciezkeDoObrazka()
        {
            if(this is Potwor pot)
            {
                this.SciezkaDoObrazka = "/Obrazki/" + pot.Imie.ToLower() + ".png";
            }
            else
            {
                this.SciezkaDoObrazka = "/Obrazki/" + this.Klasa.ToLower() + ".png";
            }
        }
    }
}
