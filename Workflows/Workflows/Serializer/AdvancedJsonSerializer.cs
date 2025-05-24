using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace WorkflowsEx.Serializer;

internal class AdvancedJsonSerializer
{

    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        ReadCommentHandling = JsonCommentHandling.Skip,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public static string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, JsonSerializerOptions);
    }

    public static string Serialize<T>(T obj, Type type)
    {
        return JsonSerializer.Serialize(obj, type, JsonSerializerOptions);
    }

    public static T Deserialize<T>(string objJson)
    {
        return JsonSerializer.Deserialize<T>(objJson, JsonSerializerOptions);
    }

    public static ValueTask<T> DeserializeAsync<T>(Stream objStream)
    {
        return JsonSerializer.DeserializeAsync<T>(objStream, JsonSerializerOptions);
    }
}
