using System;
using CommunityToolkit.Mvvm.ComponentModel;
using DryIoc;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Sdk;
using ShadowViewer.Sdk.Plugins;

namespace ShadowViewer.Plugin.PluginManager.Models;

/// <summary>
/// UiPlugin
/// </summary>
public partial class UiPlugin : ObservableObject
{
    /// <summary>
    /// <inheritdoc cref="AbstractPlugin{TMeta}.MetaData"/>
    /// </summary>
    [ObservableProperty]
    public partial PluginMetaData MetaData { get; set; }

    /// <summary>
    /// 是否开启插件
    /// </summary>
    [ObservableProperty] public partial bool IsEnabled { get; set; }

    /// <summary>
    /// <inheritdoc cref="PluginManage.CanSwitch"/>
    /// </summary>
    [ObservableProperty] public partial bool CanSwitch { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [ObservableProperty] public partial bool CanDelete { get; set; }

    /// <summary>
    /// <inheritdoc cref="PluginManage.CanOpenFolder"/>
    /// </summary>
    [ObservableProperty] public partial bool CanOpenFolder { get; set; }

    /// <summary>
    /// <inheritdoc cref="PluginManage.SettingsPage"/>
    /// </summary>
    [ObservableProperty] public partial Type? SettingsPage { get; set; }

    /// <summary>
    /// 插件Type
    /// </summary>
    [ObservableProperty] public partial Type PluginType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="plugin"></param>
    public UiPlugin(AShadowViewerPlugin plugin)
    {
        MetaData = plugin.MetaData;
        IsEnabled = plugin.IsEnabled;
        CanOpenFolder = plugin.MetaData.PluginManage.CanOpenFolder;
        CanSwitch = plugin.MetaData.PluginManage.CanSwitch;
        CanDelete = !plugin.MetaData.BuiltIn;
        SettingsPage = plugin.MetaData.PluginManage.SettingsPage?.EntryPointType;
        PluginType = plugin.GetType();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    partial void OnIsEnabledChanged(bool oldValue, bool newValue)
    {
        if (oldValue == newValue) return;
        var loader = DiFactory.Services.Resolve<PluginLoader>();
        if (newValue) loader.EnablePlugin(MetaData.Id);
        else loader.DisablePlugin(MetaData.Id);
    }
}