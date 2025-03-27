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
    /// <inheritdoc cref="PluginMetaData.CanSwitch"/>
    /// </summary>
    [ObservableProperty] private bool canSwitch;

    /// <summary>
    /// <inheritdoc cref="PluginMetaData.CanDelete"/>
    /// </summary>
    [ObservableProperty] private bool canDelete;

    /// <summary>
    /// <inheritdoc cref="PluginMetaData.CanOpenFolder"/>
    /// </summary>
    [ObservableProperty] private bool canOpenFolder;

    /// <summary>
    /// <inheritdoc cref="AShadowViewerPlugin.SettingsPage"/>
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
        this.metaData = plugin.MetaData;
        this.isEnabled = plugin.IsEnabled;
        this.canOpenFolder = plugin.MetaData.CanOpenFolder;
        this.canSwitch = plugin.MetaData.CanSwitch;
        this.canDelete = plugin.MetaData.CanDelete;
        this.settingsPage = plugin.MetaData.SettingsPage;
        this.pluginType = plugin.GetType();
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