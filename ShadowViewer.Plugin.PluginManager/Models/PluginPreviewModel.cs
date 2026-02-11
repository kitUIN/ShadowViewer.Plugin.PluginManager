using CommunityToolkit.Mvvm.ComponentModel;
using NuGet.Versioning;
using ShadowViewer.Sdk.Plugins;
using System;
using System.Collections.ObjectModel;
using ShadowViewer.Plugin.PluginManager.I18n;

namespace ShadowViewer.Plugin.PluginManager.Models;

/// <summary>
/// 插件预览模型，用于在ContentDialog中显示待安装插件的信息
/// </summary>
public partial class PluginPreviewModel : ObservableObject
{
    /// <summary>
    /// 插件Id
    /// </summary>
    [ObservableProperty]
    public partial string Id { get; set; } = string.Empty;

    /// <summary>
    /// 插件名称
    /// </summary>
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    /// <summary>
    /// 插件版本
    /// </summary>
    [ObservableProperty]
    public partial NuGetVersion? Version { get; set; }

    /// <summary>
    /// 插件描述
    /// </summary>
    [ObservableProperty]
    public partial string Description { get; set; } = string.Empty;

    /// <summary>
    /// 插件作者
    /// </summary>
    [ObservableProperty]
    public partial string Authors { get; set; } = string.Empty;

    /// <summary>
    /// 项目地址
    /// </summary>
    [ObservableProperty]
    public partial string WebUri { get; set; } = string.Empty;

    /// <summary>
    /// Logo路径
    /// </summary>
    [ObservableProperty]
    public partial string Logo { get; set; } = "font://\\uE714";

    /// <summary>
    /// SDK版本要求
    /// </summary>
    [ObservableProperty]
    public partial string SdkVersion { get; set; } = string.Empty;

    /// <summary>
    /// 插件依赖项
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<PluginPreviewDependency> Dependencies { get; set; } = [];

    /// <summary>
    /// 是否已安装相同Id的插件
    /// </summary>
    [ObservableProperty]
    public partial bool IsAlreadyInstalled { get; set; }

    /// <summary>
    /// 已安装版本
    /// </summary>
    [ObservableProperty]
    public partial NuGetVersion? InstalledVersion { get; set; }

    /// <summary>
    /// 安装状态文本
    /// </summary>
    public string InstallStatusText => IsAlreadyInstalled switch
    {
        true when InstalledVersion != null && Version != null && Version > InstalledVersion => I18N.Upgrade,
        true when InstalledVersion != null && Version != null && Version < InstalledVersion => I18N.Downgrade,
        true => I18N.Installed,
        _ => I18N.Install
    };

    /// <summary>
    /// 从PluginMetaData创建预览模型
    /// </summary>
    public static PluginPreviewModel FromMetaData(PluginMetaData metaData)
    {
        return new PluginPreviewModel
        {
            Id = metaData.Id,
            Name = metaData.Name,
            Version = metaData.Version,
            Description = metaData.Description ?? string.Empty,
            Authors = metaData.Authors ?? string.Empty,
            WebUri = metaData.WebUri ?? string.Empty,
            Logo = metaData.Logo ?? "font://\\uE714",
        };
    }
}

/// <summary>
/// 插件预览依赖项
/// </summary>
public partial class PluginPreviewDependency : ObservableObject
{
    /// <summary>
    /// 依赖插件Id
    /// </summary>
    [ObservableProperty]
    public partial string Id { get; set; } = string.Empty;

    /// <summary>
    /// 依赖版本要求
    /// </summary>
    [ObservableProperty]
    public partial string VersionRequirement { get; set; } = string.Empty;

    /// <summary>
    /// 是否已安装
    /// </summary>
    [ObservableProperty]
    public partial bool IsInstalled { get; set; }

    /// <summary>
    /// 已安装的版本
    /// </summary>
    [ObservableProperty]
    public partial string? InstalledVersion { get; set; }

    /// <summary>
    /// 依赖是否满足
    /// </summary>
    [ObservableProperty]
    public partial bool IsSatisfied { get; set; }
}
