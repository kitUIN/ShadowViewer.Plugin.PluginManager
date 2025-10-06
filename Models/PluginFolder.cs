using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI.Config;
using ShadowViewer.Sdk.Models.Interfaces;
using ShadowViewer.Sdk.Plugins;
using ShadowViewer.Plugin.PluginManager.I18n;

namespace ShadowViewer.Plugin.PluginManager.Models;

/// <summary>
/// 插件文件夹
/// </summary>
[EntryPoint(Name = nameof(PluginResponder.SettingFolders))]
public partial class PluginFolder : ISettingFolder
{
    /// <inheritdoc />
    [Autowired]
    public string PluginId { get; }

    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    public BaseSdkConfig BaseSdkConfig { get; }

    /// <inheritdoc />
    public string Name => I18N.PluginFolder;

    /// <inheritdoc />
    public string Description => I18N.PluginFolderDescription;

    /// <inheritdoc />
    public string Path
    {
        get => BaseSdkConfig.PluginFolderPath;
        set => BaseSdkConfig.PluginFolder = value;
    }

    /// <inheritdoc />
    public bool CanOpen => true;

    /// <inheritdoc />
    public bool CanChange => false;
}