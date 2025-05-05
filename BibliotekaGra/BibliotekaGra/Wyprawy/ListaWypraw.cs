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
    public class ListaWypraw
    {
        public List<Wyprawa?> Lista { get; set; }
        public int MaksListaWypraw { get; } = 2;
        public Postac Postac { get; set; }
        public ListaWypraw(Postac postac)
        {
            Lista = new List<Wyprawa?>(new Wyprawa?[MaksListaWypraw]);
            Postac = postac;
            OdswiezListe();
        }
        public void OdswiezListe()
        {
            for (int i = 0; i < MaksListaWypraw; i++)
            {
                Lista[i] = GeneratorWypraw.GenerujWyprawaPoziom(Postac,i+1);
                Debug.WriteLine($"{Lista[i].Nazwa} potwor {Lista[i].Potwor.Imie} staty {Lista[i].Potwor.BazoweStatystyki} {Lista[i].Potwor.StatystykiCalkowite}");
            }
        }
        public string WypiszWyprawy()
        {
            int i = 0;
            string ret = "Wyprawy:\n";
            foreach(Wyprawa w in Lista)
            {
                ret += "-" + i + "." + w.ToString()+"\n";
                i++;
            }
            return ret;
        }
    }
}
