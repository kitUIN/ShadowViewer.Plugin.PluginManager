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

namespace ShadowViewer.Plugin.PluginManager.Responders;

/// <summary>
/// 自定义导航响应
/// </summary>
/// <remarks>
/// <inheritdoc/>
/// </remarks>
[EntryPoint(Name = nameof(PluginResponder.NavigationResponder))]
public partial class PluginManagerNavigationResponder : AbstractNavigationResponder
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
                icon: new FontIcon { Glyph = "\uE74C" },
                content: I18N.PluginManager)
        };

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override ShadowNavigation? NavigationViewItemInvokedHandler(IShadowNavigationItem item)
    {
        return item.Id switch
        {
            "PluginManager" => new ShadowNavigation(typeof(PluginPage), SelectItemId: item.Id),
            _ => null
        };
    }
}