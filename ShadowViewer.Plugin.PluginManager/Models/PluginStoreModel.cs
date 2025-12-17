using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DryIoc;
using NuGet.Versioning;
using Serilog;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Plugin.PluginManager.Enums;
using ShadowViewer.Plugin.PluginManager.I18n;
using ShadowViewer.Sdk;
using ShadowViewer.Sdk.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Scriban;

namespace ShadowViewer.Plugin.PluginManager.Models;

/// <summary>
/// 插件项，包含插件的详细信息。
/// </summary>
public partial class PluginStoreModel : ObservableObject
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
    public partial string SdkVersion { get; set; } = null!;

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
    public partial PluginInstallStatus InstallStatus { get; set; }

    /// <summary>
    /// 安装按钮文本
    /// </summary>
    [ObservableProperty]
    public partial string? InstallButtonText { get; set; }


    /// <summary>
    /// 已经安装的版本
    /// </summary>
    [ObservableProperty]
    public partial NuGetVersion? InstalledVersion { get; set; }

    partial void OnIdChanged(string oldValue, string newValue)
    {
        if (oldValue == newValue) return;
        InstalledVersion = DiFactory.Services.Resolve<PluginLoader>().GetPlugin(Id)?.MetaData.Version;
    }

    partial void OnVersionChanged(string oldValue, string newValue)
    {
        if (oldValue == newValue) return;
        InstalledVersion ??= DiFactory.Services.Resolve<PluginLoader>().GetPlugin(Id)?.MetaData.Version;
        if (InstalledVersion == null)
        {
            InstallStatus = PluginInstallStatus.Install;
            return;
        }

        var pluginStoreVersion = new NuGetVersion(Version);
        if (pluginStoreVersion > InstalledVersion)
        {
            InstallStatus = PluginInstallStatus.Upgrade;
        }
        else if (pluginStoreVersion == InstalledVersion)
        {
            InstallStatus = PluginInstallStatus.Installed;
        }
        else
        {
            InstallStatus = PluginInstallStatus.Downgrade;
        }
    }

    partial void OnInstallStatusChanged(PluginInstallStatus value)
    {
        InstallButtonText = value switch
        {
            PluginInstallStatus.Upgrade => "升级到",
            PluginInstallStatus.Downgrade => "降级到",
            PluginInstallStatus.Installed => "已安装",
            _ => "安装"
        } + " " + Version;
    }

    [RelayCommand]
    private async Task Install()
    {
        var pluginManager = DiFactory.Services.Resolve<PluginLoader>();
        if (DownloadUrl == null) return;
        if (InstallStatus == PluginInstallStatus.Install)
        {
            await DialogHelper.ShowDialog(XamlHelper.CreateMessageDialog(I18N.Install,
                await Template.Parse(I18N.InstallTextTemplate)
                    .RenderAsync(new { action = I18N.Install, name = Id, version = Version }),
                async void (sender, args) =>
                {
                    try
                    {
                        args.Cancel = true;
                        sender.PrimaryButtonText = I18N.Installing;
                        await pluginManager.InstallAsync([DownloadUrl]);
                        sender.Hide();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "Catch Error in InstallAsync");
                    }
                }
            ));
        }
        else if (InstallStatus is PluginInstallStatus.Upgrade or PluginInstallStatus.Downgrade)
        {
            var action = InstallStatus == PluginInstallStatus.Upgrade ? I18N.Upgrade : I18N.Downgrade;
            await DialogHelper.ShowDialog(XamlHelper.CreateMessageDialog(action,
                await Template.Parse(I18N.UpgradeTextTemplate)
                    .RenderAsync(new { action = action, name = Id, newVersion = Version, oldVersion = InstalledVersion?.ToString() }),
                async void (sender, args) =>
                {
                    try
                    {
                        args.Cancel = true;
                        sender.PrimaryButtonText = I18N.Installing;
                        await pluginManager.InstallAsync([DownloadUrl]);
                        sender.Hide();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "Catch Error in InstallAsync");
                    }
                }
            ));
        }
    }
}

/// <summary>
/// 插件依赖项，描述依赖的插件及需求。
/// </summary>
public class PluginItemDependency
{
    /// <summary>
    /// 依赖插件的唯一标识
    /// </summary>
    [JsonPropertyName("Id")]
    public string Id { get; set; } = null!;

    /// <summary>
    /// 依赖需求描述
    /// </summary>
    [JsonPropertyName("Need")]
    public string Need { get; set; } = null!;
}