using BibliotekaGra.Postacie;
using System.IO;
using System.Text.Json;

namespace BibliotekaGra.Gra
{
    public static class ZapisIOdczyt
    {
        private static readonly string FolderUzytkownika = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"GraRPG");

        private static readonly string Sciezka = Path.Combine(FolderUzytkownika, "save.json");

        public static void ZapiszGry(List<Gra> gry)
        {
            if (!Directory.Exists(FolderUzytkownika))
                Directory.CreateDirectory(FolderUzytkownika);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
            };
            options.Converters.Add(new PostacConverter());
            string json = JsonSerializer.Serialize(gry, options);
            File.WriteAllText(Sciezka, json);
        }

        public static List<Gra> WczytajGry()
        {
            if (!File.Exists(Sciezka))
                return new List<Gra>();

            string json = File.ReadAllText(Sciezka);
            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };
            options.Converters.Add(new PostacConverter());
            return JsonSerializer.Deserialize<List<Gra>>(json, options);
        }
    }
}
