using System.Text.Json;

public static class Serializer
{
    public static string ToJson(object obj)
    {
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            WriteIndented = false
        });
    }

    public static string? SerializeSafely(object? obj)
    {
        if (obj == null)
            return null;

        var options = new JsonSerializerOptions
        {
            WriteIndented = false,

            // 🔥 THIS FIXES YOUR CYCLE ISSUE PERMANENTLY
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        };

        return JsonSerializer.Serialize(obj, options);
    }
}