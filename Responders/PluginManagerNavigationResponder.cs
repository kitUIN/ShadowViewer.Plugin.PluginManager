﻿using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Models;
using ShadowViewer.Models.Interfaces;
using ShadowViewer.Responders;
using ShadowViewer.Services;
using ShadowViewer.Plugin.PluginManager.Helpers;
using SqlSugar;
using ShadowViewer.Plugin.PluginManager.Pages;

namespace ShadowViewer.Plugin.PluginManager.Responders;

/// <summary>
/// 自定义导航响应
/// </summary>
/// <remarks>
/// <inheritdoc/>
/// </remarks>
public class PluginManagerNavigationResponder(
    ICallableService callableService, 
    ISqlSugarClient sqlSugarClient,
    CompressService compressServices, 
    PluginLoader pluginService, 
    string id) : AbstractNavigationResponder(
    id,callableService, sqlSugarClient, compressServices, pluginService)
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override IEnumerable<IShadowNavigationItem> NavigationViewFooterItems { get; } =
        new List<IShadowNavigationItem>
        {
            new ShadowNavigationItem(pluginId:PluginManagerPlugin.Meta.Id, id: "PluginManager",icon: new FontIcon { Glyph = "\uE74C" }, 
                content: I18N.PluginManager)
        };

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override ShadowNavigation? NavigationViewItemInvokedHandler(IShadowNavigationItem item)
    {
        return item.Id switch
        {
            "PluginManager" => new ShadowNavigation(typeof(PluginPage)),
            _ => null
        };
    }

}