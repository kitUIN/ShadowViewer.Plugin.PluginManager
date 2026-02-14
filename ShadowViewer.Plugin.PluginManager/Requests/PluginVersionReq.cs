using System.Text.Json.Serialization;

namespace ShadowViewer.Plugin.PluginManager.Requests;

/// <summary>
/// 
/// </summary>
public class PluginVersionReq
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PluginVersionReq"/> class.
    /// </summary>
    public PluginVersionReq(string pluginId, string version)
    {
        PluginId = pluginId;
        Version = version;
    }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("plugin_id")] public string PluginId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("version")] public string Version { get; set; }
}