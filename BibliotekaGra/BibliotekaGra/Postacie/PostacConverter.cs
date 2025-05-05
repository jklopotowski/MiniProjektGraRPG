using System.Text.Json;
using System.Text.Json.Serialization;
using BibliotekaGra.Postacie;
using BibliotekaGra.Wyprawy;

namespace BibliotekaGra.Postacie
{

    public class PostacConverter : JsonConverter<Postac>
    {
        public override Postac Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var root = jsonDoc.RootElement;

            if (!root.TryGetProperty("Klasa", out var klasaProp))
                throw new JsonException("Brak właściwości 'Klasa' – nie wiadomo jaką klasę postaci stworzyć.");

            string typ = klasaProp.GetString();
            Postac postac = typ switch
            {
                "Berserker" => JsonSerializer.Deserialize<Berserker>(root.GetRawText(), options),
                "Czarodziej" => JsonSerializer.Deserialize<Czarodziej>(root.GetRawText(), options),
                "Lucznik" => JsonSerializer.Deserialize<Lucznik>(root.GetRawText(), options),
                "Potwor" => JsonSerializer.Deserialize<Potwor>(root.GetRawText(), options),
                _ => throw new JsonException($"Nieznany typ postaci: {typ}")
            };

            return postac;
        }

        public override void Write(Utf8JsonWriter writer, Postac value, JsonSerializerOptions options)
        {
            var type = value.GetType();
            JsonSerializer.Serialize(writer, value, type, options);
        }
    }
}
