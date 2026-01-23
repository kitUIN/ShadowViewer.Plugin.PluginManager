using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI.Models;

namespace ShadowViewer.Plugin.PluginManager.Models;

/// <summary>
/// 插件管理器使用数据
/// </summary>
public record PluginManage
{
    /// <summary>
    /// 能否开关
    /// </summary>
    [Meta(Required = false)]
    public bool CanSwitch { get; init; } = true;

    /// <summary>
    /// 能否打开文件夹
    /// </summary>
    [Meta(Required = false)]
    public bool CanOpenFolder { get; init; } = true;

    /// <summary>
    /// 设置页面
    /// </summary>
    [Meta(Required = false)]
    public PluginEntryPointType? SettingsPage { get; init; }
}
