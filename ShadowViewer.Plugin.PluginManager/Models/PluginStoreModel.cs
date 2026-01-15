using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DryIoc;
using NuGet.Versioning;
using ShadowPluginLoader.WinUI;
using ShadowPluginLoader.WinUI.Converters;
using ShadowPluginLoader.WinUI.Extensions;
using ShadowViewer.Plugin.PluginManager.Enums;
using ShadowViewer.Plugin.PluginManager.I18n;
using ShadowViewer.Sdk;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.Helpers;
using FluentIcons.Common;
using Microsoft.IdentityModel.Tokens;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

namespace ShadowViewer.Plugin.PluginManager.Models;

/// <summary>
/// 
/// </summary>
/// <seealso cref="CommunityToolkit.Mvvm.ComponentModel.ObservableObject" />
public class PluginStoreBaseModel : ObservableObject
{
    /// <summary>
    /// 获取依赖检查的图标
    /// </summary>
    public Symbol GetDependencyCheckSymbol(bool? status)
    {
        return status switch
        {
            true => Symbol.ShieldCheckmark,
            false => Symbol.ShieldProhibited,
            _ => Symbol.Shield
        };
    }

    /// <summary>
    /// 获取依赖检查的图标颜色
    /// </summary>
    public SolidColorBrush GetDependencyColorBrush(bool? status)
    {
        return status switch
        {
            true => new SolidColorBrush("#0f7b0f".ToColor()),
            false => new SolidColorBrush("#c42b1c".ToColor()),
            _ => new SolidColorBrush(Colors.Black)
        };
    }

    /// <summary>
    /// 获取依赖检查的图标
    /// </summary>
    public Symbol GetDependencySymbol(bool? status)
    {
        return status switch
        {
            true => Symbol.ShieldTask,
            false => Symbol.ShieldDismiss,
            _ => Symbol.ShieldQuestion
        };
    }
}

/// <summary>
/// 插件项，包含插件的详细信息。
/// </summary>
public partial class PluginStoreModel : PluginStoreBaseModel
{
    /// <summary>
    /// 插件唯一标识
    /// </summary>
    [JsonPropertyName("Id")]
    [ObservableProperty]
    public partial string Id { get; set; } = null!;

    /// <summary>
    /// 插件名称
    /// </summary>
    [JsonPropertyName("Name")]
    [ObservableProperty]
    public partial string Name { get; set; } = null!;

    /// <summary>
    /// 当前版本号
    /// </summary>
    [JsonPropertyName("Version")]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(InstallButtonText))]
    public partial string Version { get; set; } = null!;

    /// <summary>
    /// 所有可用版本号
    /// </summary>
    [JsonPropertyName("Versions")]
    public ObservableCollection<string> Versions { get; set; } = [];

    /// <summary>
    /// 背景颜色（如用于UI展示）
    /// </summary>
    [JsonPropertyName("BackgroundColor")]
    [ObservableProperty]
    public partial string? BackgroundColor { get; set; }

    /// <summary>
    /// 插件标签
    /// </summary>
    [JsonPropertyName("Tags")]
    public ObservableCollection<string> Tags { get; set; } = [];

    /// <summary>
    /// 插件描述
    /// </summary>
    [JsonPropertyName("Description")]
    [ObservableProperty]
    public partial string? Description { get; set; }

    /// <summary>
    /// 作者信息
    /// </summary>
    [JsonPropertyName("Authors")]
    [ObservableProperty]
    public partial string? Authors { get; set; }

    /// <summary>
    /// 插件主页链接
    /// </summary>
    [JsonPropertyName("WebUri")]
    [ObservableProperty]
    public partial string? WebUri { get; set; }

    /// <summary>
    /// 插件Logo图片链接
    /// </summary>
    [JsonPropertyName("Logo")]
    [ObservableProperty]
    public partial string? Logo { get; set; }

    /// <summary>
    /// 依赖的SDK版本
    /// </summary>
    [JsonPropertyName("SdkVersion")]
    [ObservableProperty]
    [JsonConverter(typeof(VersionRangeJsonConverter))]
    public partial VersionRange SdkVersion { get; set; } = null!;

    /// <summary>
    /// 插件依赖项列表
    /// </summary>
    [JsonPropertyName("Dependencies")]
    public ObservableCollection<PluginItemDependency> Dependencies { get; set; } = [];

    /// <summary>
    /// 插件下载链接
    /// </summary>
    [JsonPropertyName("DownloadUrl")]
    [ObservableProperty]
    public partial string? DownloadUrl { get; set; }

    /// <summary>
    /// 最后更新时间
    /// </summary>
    [JsonPropertyName("LastUpdated")]
    [ObservableProperty]
    public partial DateTime LastUpdated { get; set; }


    /// <summary>
    /// 安装状态说明
    /// </summary> 
    [ObservableProperty]
    [JsonIgnore]
    [NotifyPropertyChangedFor(nameof(InstallButtonText))]
    [NotifyPropertyChangedFor(nameof(InstallButtonEnabled))]
    [NotifyCanExecuteChangedFor(nameof(InstallCommand))]
    public partial PluginInstallStatus InstallStatus { get; set; }

    /// <summary>
    /// 安装按钮文本
    /// </summary> 
    [JsonIgnore]
    public string InstallButtonText => InstallStatus switch
    {
        PluginInstallStatus.Upgrade => I18N.UpgradeAction,
        PluginInstallStatus.Downgrade => I18N.DowngradeAction,
        PluginInstallStatus.Installed => I18N.Installed,
        _ => I18N.Install
    } + " " + Version;

    /// <summary>
    /// 安装按钮是否启用
    /// </summary> 
    [JsonIgnore]
    public bool InstallButtonEnabled => InstallStatus != PluginInstallStatus.Installed;

    /// <summary>
    /// 已经安装的Sdk版本
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SdkVersionStatus))]
    [JsonIgnore]
    public partial NuGetVersion? InstalledSdkVersion { get; set; }

    /// <summary>
    /// Sdk版本检查状态
    /// </summary>
    [JsonIgnore]
    public bool? SdkVersionStatus
    {
        get
        {
            if (InstalledSdkVersion == null) return null;
            return SdkVersion.Satisfies(InstalledSdkVersion);
        }
    }

    /// <summary>
    /// 依赖检查状态
    /// </summary> 
    [JsonIgnore]
    [ObservableProperty]
    public partial bool? DependencyCheckStatus { get; set; }

    partial void OnIdChanged(string oldValue, string newValue)
    {
        if (oldValue == newValue) return;
        InstalledSdkVersion = DiFactory.Services.Resolve<PluginLoader>().GetPlugin(Id)?.MetaData.Version;
    }

    partial void OnVersionChanged(string oldValue, string newValue)
    {
        if (oldValue == newValue) return;
        InstalledSdkVersion ??= DiFactory.Services.Resolve<PluginLoader>().GetPlugin(Id)?.MetaData.Version;
        if (InstalledSdkVersion == null)
        {
            InstallStatus = PluginInstallStatus.Install;
            return;
        }

        var pluginStoreVersion = new NuGetVersion(Version);
        if (pluginStoreVersion > InstalledSdkVersion)
        {
            InstallStatus = PluginInstallStatus.Upgrade;
        }
        else if (pluginStoreVersion == InstalledSdkVersion)
        {
            InstallStatus = PluginInstallStatus.Installed;
        }
        else
        {
            InstallStatus = PluginInstallStatus.Downgrade;
        }
    }

    [RelayCommand]
    private void CheckDependencies()
    {
        var pluginManager = DiFactory.Services.Resolve<PluginLoader>();
        Dictionary<string, NuGetVersion> installedPlugins = new();
        foreach (var plugin in pluginManager.GetPlugins())
        {
            installedPlugins[plugin.MetaData.Id] = plugin.MetaData.Version;
        }

        var flag = true;
        foreach (var dependency in Dependencies)
        {
            if (installedPlugins.TryGetValue(dependency.Id, out var version))
            {
                dependency.Installed = version;
                dependency.Status = dependency.Need.Satisfies(version);
            }
            else
            {
                flag = false;
            }
        }

        DependencyCheckStatus = flag;
    }

    [RelayCommand(CanExecute = nameof(InstallButtonEnabled))]
    private async Task Install()
    {
        var pluginManager = DiFactory.Services.Resolve<PluginLoader>();
        if (InstallStatus == PluginInstallStatus.Install && !DownloadUrl.IsNullOrEmpty())
        {
            var pipeline = pluginManager.CreatePipeline();
            await pipeline.Feed(new Uri(DownloadUrl!)).ProcessAsync();
        }
    }
}

/// <summary>
/// 插件依赖项，描述依赖的插件及需求。
/// </summary>
public partial class PluginItemDependency : PluginStoreBaseModel
{
    /// <summary>
    /// 依赖插件的唯一标识
    /// </summary>
    [JsonPropertyName("Id")]
    [ObservableProperty]
    public partial string Id { get; set; } = null!;

    /// <summary>
    /// 依赖需求描述
    /// </summary>
    [JsonPropertyName("Need")]
    [ObservableProperty]
    [JsonConverter(typeof(VersionRangeJsonConverter))]
    public partial VersionRange Need { get; set; } = null!;

    /// <summary>
    /// 已经安装依赖版本
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    public partial NuGetVersion? Installed { get; set; }


    /// <summary>
    /// 依赖是否符合条件
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    public partial bool Status { get; set; }
}