using System;
using CommunityToolkit.Mvvm.ComponentModel;
using DryIoc;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Core;
using ShadowViewer.Plugin.PluginManager.Enums;
using ShadowViewer.Plugin.PluginManager.Responses;

namespace ShadowViewer.Plugin.PluginManager.Models;

/// <summary>
/// 
/// </summary>
public partial class PluginStoreModel : ObservableObject
{
    /// <summary>
    /// Plugin Id
    /// </summary>
    public string Id => MetaData.Id;
    /// <summary>
    /// 
    /// </summary>
    public PluginItem MetaData { get; }

    /// <summary>
    /// 能否安装
    /// </summary>
    public bool CouldUpdate => InstallStatus == PluginInstallStatus.Upgrade;

    /// <summary>
    /// 控制按钮
    /// </summary>
    public bool ButtonEnabled => InstallStatus != PluginInstallStatus.Installed;

    /// <summary>
    /// 安装状态说明
    /// </summary> 
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ButtonEnabled))]
    [NotifyPropertyChangedFor(nameof(CouldUpdate))]
    private PluginInstallStatus installStatus;
    /// <summary>
    /// 当前版本
    /// </summary>
    public string CurrentVersion { get; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public PluginStoreModel(PluginItem data)
    {
        MetaData = data;
        InstallStatus = PluginInstallStatus.None;
        var plugin = DiFactory.Services.Resolve<PluginLoader>().GetPlugin(Id);
        if (plugin == null) return;
        InstallStatus = PluginInstallStatus.Installed;
        CurrentVersion = plugin.MetaData.Version;
        if (new Version(plugin.MetaData.Version) < new Version(MetaData.Version))
        {
            InstallStatus = PluginInstallStatus.Upgrade;
        }
    }

}