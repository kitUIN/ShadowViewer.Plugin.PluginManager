using System.Linq;
using System.Text.Json;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Plugin.PluginManager.Models;

namespace ShadowViewer.Plugin.PluginManager.Extensions;

/// <summary>
/// 
/// </summary>
public static class MetaDataExtension
{
    /// <summary>
    /// Gets the plugin manage.
    /// </summary>
    /// <param name="metaData">The meta data.</param>
    /// <returns></returns>
    public static PluginManage GetPluginManage(this BasePluginMetaData metaData)
    {
        if (metaData.Raw.TryGetProperty(nameof(PluginManage), out var jsonElement) &&
            jsonElement.ValueKind == JsonValueKind.Object)
        {
            return new PluginManage()
            {
                CanOpenFolder = jsonElement.GetProperty(nameof(PluginManage.CanOpenFolder)).GetBoolean(),
                CanSwitch = jsonElement.GetProperty(nameof(PluginManage.CanSwitch)).GetBoolean(),
                SettingsPage = metaData.EntryPoints
                    .FirstOrDefault(x => x.Name == nameof(PluginManage.SettingsPage))
            };
        }

        return new PluginManage()
        {
            SettingsPage = metaData.EntryPoints
                .FirstOrDefault(x => x.Name == nameof(PluginManage.SettingsPage))
        };
    }
}