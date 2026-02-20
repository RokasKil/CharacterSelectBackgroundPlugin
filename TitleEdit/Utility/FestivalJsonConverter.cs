using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TitleEdit.Data.Persistence;

namespace TitleEdit.Utility;

public class FestivalJsonConverter : JsonConverter<Festival[]>
{
    public override Festival[]? ReadJson(JsonReader reader, Type objectType, Festival[]? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return existingValue;
        if (reader.TokenType != JsonToken.StartArray)
            throw new JsonSerializationException("Expected an array for Festival[]");
        var array = JArray.Load(reader);
        var result = new Festival[array.Count];

        for (int i = 0; i < array.Count; i++)
        {
            var token = array[i];
            if (token.Type == JTokenType.Integer)
            {
                uint number = token.ToObject<uint>();
                result[i] = new()
                {
                    Id = (ushort)(number & 0xFFFF),
                    Phase = (ushort)(number >> 16)
                };
            }
            else if (token.Type == JTokenType.Object)
            {
                result[i] = token.ToObject<Festival>(serializer);
            }
            else
            {
                throw new JsonSerializationException($"Unexpected token: {token.Type}");
            }
        }

        return result;
    }

    public override void WriteJson(JsonWriter writer, Festival[]? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartArray();

        foreach (var item in value)
        {
            serializer.Serialize(writer, item);
        }

        writer.WriteEndArray();
    }
}
