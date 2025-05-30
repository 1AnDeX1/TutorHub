using System.Text.Json;
using System.Text.Json.Serialization;
using TutorHub.BusinessLogic.Exceptions;

namespace TutorHub.BusinessLogic.Converters;

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private const string TimeFormat = "HH:mm";

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String &&
            TimeOnly.TryParseExact(reader.GetString(), TimeFormat, out var time))
        {
            return time;
        }

        throw new ValidationException("Invalid time format. Use HH:mm.");
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(TimeFormat));
    }
}
