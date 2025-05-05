using BibliotekaGra.Postacie;
using BibliotekaGra.Wyjatki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Gra
{
    public enum KlasaPostaci
    {
        Berserker,
        Lucznik,
        Czarodziej
    }
    public class MenadżerPostaci
    {
        public Gra AktualnaGra { get; set; }
        public List<Gra> Gry { get; set; }
        public MenadżerPostaci() { 
            AktualnaGra = null;
            Gry = ZapisIOdczyt.WczytajGry();
            //Start();
        }
        public void Start()
        {
            int wybor=0;
            do
            {
                Console.WriteLine("1-stworz postac\n2-wybierz postac\n3-usun postac\n0-wyjdz");
                wybor = int.Parse(Console.ReadLine());
                switch (wybor)
                {
                    case 1:
                        {
                            Console.WriteLine("Podaj imie");
                            string imie = Console.ReadLine();
                            Console.WriteLine("1-berserk 2-lucznik 3-czarodziej");
                            int klasa = int.Parse(Console.ReadLine());
                            KlasaPostaci k = KlasaPostaci.Berserker;
                            switch (klasa)
                            {
                                case 1:
                                    {
                                        k = KlasaPostaci.Berserker;
                                        break;
                                    }
                                case 2:
                                    {
                                        k = KlasaPostaci.Lucznik;
                                        break;
                                    }
                                case 3:
                                    {
                                        k = KlasaPostaci.Czarodziej;
                                        break;
                                    }
                            }
                            DodajPostac(StworzPostac(imie, k));
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine(WypiszPostacie());
                            int post = int.Parse(Console.ReadLine());
                            AktualnaGra = Gry[post-1]; 
                            if (AktualnaGra.Sklep != null && AktualnaGra.Gracz != null)
                            {
                                AktualnaGra.Sklep.UstawPostac(AktualnaGra.Gracz);
                            }
                            AktualnaGra.Start();
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine(WypiszPostacie());
                            int post = int.Parse(Console.ReadLine());
                            UsunGre(Gry[post]);
                            break;
                        }
                }
            } while (wybor != 0);
        }
        public void DodajPostac(Postac postac)
        {
            if (Gry.Any(gr => gr.Gracz.Imie == postac.Imie))
                throw new PostacJuzIstniejeException($"Postać o imieniu {postac.Imie} już istnieje.");

            Gry.Add(new Gra(postac));
        }

        public void UsunGre(Gra gra)
        {
           Gry.Remove(gra);
        }

        public void WybierzGre(Gra gra)
        {
            AktualnaGra = gra;
            AktualnaGra.Start();
        }
        public void WylogujGre()
        {
            if (AktualnaGra != null)
                AktualnaGra = null;
        }
        public string WypiszPostacie()
        {
            string lista = "Postacie:\n";
            int i = 1;
            foreach(Gra g in Gry)
            {
                lista+= " - "+i+"."+g.Gracz.ToString()+"\n";
            }
            return lista;
        }
        public Postac StworzPostac(string imie, KlasaPostaci klasa)
        {
            return klasa switch
            {
                KlasaPostaci.Berserker => new Berserker(imie),
                KlasaPostaci.Lucznik => new Lucznik(imie),
                KlasaPostaci.Czarodziej => new Czarodziej(imie),
                _ => throw new ArgumentException("Nieznana klasa postaci.")
            };
        }
        public void ZapiszWszystko()
        {
            ZapisIOdczyt.ZapiszGry(Gry);
        }
    }
}
