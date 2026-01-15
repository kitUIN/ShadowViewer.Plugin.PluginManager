using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NuGet.Versioning;

namespace ShadowViewer.Plugin.PluginManager.Converters;
/// <summary>
/// JsonConverter <seealso cref="string" /> -> <seealso cref="VersionRange" />
/// </summary>
public class VersionRangeJsonConverter : JsonConverter<VersionRange>
{
    /// <inheritdoc />
    public override VersionRange Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var text = reader.GetString();
        return string.IsNullOrEmpty(text) ? VersionRange.None : VersionRange.Parse(text!);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, VersionRange value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToNormalizedString());
    }
}