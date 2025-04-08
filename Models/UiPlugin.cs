using System;
using CommunityToolkit.Mvvm.ComponentModel;
using DryIoc;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Core;
using ShadowViewer.Core.Plugins;

namespace ShadowViewer.Plugin.PluginManager.Models;

/// <summary>
/// UiPlugin
/// </summary>
public partial class UiPlugin : ObservableObject
{
    /// <summary>
    /// <inheritdoc cref="AShadowViewerPlugin.MetaData"/>
    /// </summary>
    [ObservableProperty] private PluginMetaData metaData;

    /// <summary>
    /// 是否开启插件
    /// </summary>
    [ObservableProperty] private bool isEnabled;

    /// <summary>
    /// <inheritdoc cref="PluginManage.CanSwitch"/>
    /// </summary>
    [ObservableProperty] private bool canSwitch;

    /// <summary>
    /// <inheritdoc cref="PluginManage.CanDelete"/>
    /// </summary>
    [ObservableProperty] private bool canDelete;

    /// <summary>
    /// <inheritdoc cref="PluginManage.CanOpenFolder"/>
    /// </summary>
    [ObservableProperty] private bool canOpenFolder;

    /// <summary>
    /// <inheritdoc cref="PluginManage.SettingsPage"/>
    /// </summary>
    [ObservableProperty] private Type? settingsPage;

    /// <summary>
    /// 插件Type
    /// </summary>
    [ObservableProperty] private Type pluginType;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="plugin"></param>
    public UiPlugin(AShadowViewerPlugin plugin)
    {
        metaData = plugin.MetaData;
        isEnabled = plugin.IsEnabled;
        canOpenFolder = plugin.MetaData.PluginManage.CanOpenFolder;
        canSwitch = plugin.MetaData.PluginManage.CanSwitch;
        canDelete = !plugin.MetaData.BuiltIn;
        settingsPage = plugin.MetaData.PluginManage.SettingsPage;
        pluginType = plugin.GetType();
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
        if (newValue) loader.EnablePlugin(metaData.Id);
        else loader.DisablePlugin(metaData.Id);
    }
}