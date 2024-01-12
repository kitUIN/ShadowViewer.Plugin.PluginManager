using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Interfaces;
using ShadowViewer.Models;
using ShadowViewer.Responders;
using ShadowViewer.Services;
using ShadowViewer.Plugin.PluginManager.Helpers;

using SqlSugar;
using ShadowViewer.Plugin.PluginManager.Pages;

namespace ShadowViewer.Plugin.PluginManager.Responders;

/// <summary>
/// 自定义导航响应
/// </summary>
public class PluginManagerNavigationResponder : NavigationResponderBase
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public PluginManagerNavigationResponder(ICallableService callableService, ISqlSugarClient sqlSugarClient, CompressService compressServices, IPluginService pluginService, string id) : base(callableService, sqlSugarClient, compressServices, pluginService, id)
    {
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override IEnumerable<IShadowNavigationItem> NavigationViewFooterItems { get; } =
        new List<IShadowNavigationItem>
        {
            new ShadowNavigationItem
            {
                Icon = new FontIcon { Glyph = "\uE74C" },
                Id = "PluginManager",
                Content = I18N.PluginManager
            }
        };

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void NavigationViewItemInvokedHandler(IShadowNavigationItem item, ref Type? page,
        ref object? parameter)
    {
        switch (item.Id)
        {
            case "PluginManager":
                page = typeof(PluginPage);
                break;
        }
    }

}