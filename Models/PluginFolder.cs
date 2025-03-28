using ShadowViewer.Core.Models.Interfaces;
using ShadowViewer.Core.Settings;
using ShadowViewer.Plugin.PluginManager.I18n;

namespace ShadowViewer.Plugin.PluginManager.Models;

/// <summary>
/// 插件文件夹
/// </summary>
public class PluginFolder: ISettingFolder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pluginId"></param>
    public PluginFolder(string pluginId)
    {
        PluginId = pluginId;
    }
    /// <inheritdoc />
    public string PluginId { get; }

    /// <inheritdoc />
    public string Name => I18N.PluginFolder;

    /// <inheritdoc />
    public string Description => I18N.PluginFolderDescription;

    /// <inheritdoc />
    public string Path
    {
        get => CoreSettings.Instance.PluginsPath;
        set => CoreSettings.Instance.PluginsPath = value;
    }

    /// <inheritdoc />
    public bool CanOpen => true;

    /// <inheritdoc />
    public bool CanChange => false;
}