using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaGra.Ekwipunek
{
    public class EkwipunekPostaci
    {
        public Dictionary<SlotEkwipunku, Przedmiot?> ZalozonePrzedmioty { get; set; }
        public Plecak Plecak { get; set; }

        public EkwipunekPostaci()
        {
            ZalozonePrzedmioty = new Dictionary<SlotEkwipunku, Przedmiot?>
            {
                { SlotEkwipunku.Bron, null },
                { SlotEkwipunku.Helm, null },
                { SlotEkwipunku.Napiersnik, null },
                { SlotEkwipunku.Spodnie, null },
                { SlotEkwipunku.Buty, null },
            };
            Plecak = new Plecak();
        }

        public void ZalozPrzedmiot(Przedmiot przedmiot, int index)
        {
            if (ZalozonePrzedmioty[przedmiot.Slot] == null)
            {
                ZalozonePrzedmioty[przedmiot.Slot] = przedmiot;
                Plecak.UsunPrzedmiot(index);
            }
            else if (ZalozonePrzedmioty[przedmiot.Slot]!=null)
            {
                var item = ZalozonePrzedmioty[przedmiot.Slot];
                ZalozonePrzedmioty[przedmiot.Slot] = przedmiot;
                Plecak.ZamienZEkwipunkiem(item, index);
            }
        }
        public void ZdejmijPrzedmiot(SlotEkwipunku slot)
        {
            if (ZalozonePrzedmioty[slot] != null)
            {
                Plecak.DodajPrzedmiot(ZalozonePrzedmioty[slot]);
                ZalozonePrzedmioty[slot] = null;
            }
        }
        public string WypiszZalozone()
        {
            return string.Join("\n", ZalozonePrzedmioty.Select(kv =>
                $"{kv.Key}: {(kv.Value?.Nazwa ?? "Brak")}"));
        }

        public Statystyki.Statystyki SumujBonusy(Statystyki.Statystyki staty)
        {
            var suma = staty;
            foreach (var item in ZalozonePrzedmioty.Values)
            {
                if(item!=null)
                    suma.Dodaj(item.Statystyki);
            }
            return suma;
        }
    }
}
