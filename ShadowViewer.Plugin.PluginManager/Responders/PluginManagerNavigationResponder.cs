using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Sdk.Models;
using ShadowViewer.Sdk.Responders;
using ShadowViewer.Plugin.PluginManager.Pages;
using ShadowViewer.Sdk.Models.Interfaces;
using ShadowViewer.Sdk.Plugins;
using ShadowViewer.Sdk.Utils;
using ShadowViewer.Plugin.PluginManager.I18n;
using ShadowViewer.Sdk.Navigation;

namespace ShadowViewer.Plugin.PluginManager.Responders;

/// <summary>
/// 自定义导航响应
/// </summary>
/// <remarks>
/// <inheritdoc/>
/// </remarks>
[EntryPoint(Name = nameof(PluginResponder.NavigationResponder))]
public class PluginManagerNavigationResponder : AbstractNavigationResponder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public PluginManagerNavigationResponder(string id) : base(id)
    {
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override IEnumerable<IShadowNavigationItem> NavigationViewFooterItems { get; } =
        new List<IShadowNavigationItem>
        {
            new ShadowNavigationItem(
                pluginId: "ShadowViewer.Plugin.PluginManager",
                id: "PluginManager",
                uri: ShadowUri.Parse("shadow://pluginmanager"),
                icon: "font://\uE74C",
                content: I18N.PluginManager)
        };

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void Register()
    {
        ShadowRouteRegistry.RegisterPage(new ShadowNavigation(typeof(PluginPage), SelectItemId: "PluginManager"),
            "pluginmanager");
        ShadowRouteRegistry.RegisterPage(new ShadowNavigation(typeof(PluginStorePage), SelectItemId: "PluginManager"),
            "pluginmanager", "store");
        ShadowRouteRegistry.RegisterPage(
            new ShadowNavigation(typeof(PluginManagerSettingsPage), SelectItemId: "PluginManager"), "pluginmanager",
            "settings");
    }
}