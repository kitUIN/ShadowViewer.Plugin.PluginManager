using DryIoc;
using Serilog;
using ShadowViewer.Plugin.PluginManager.ViewModels;
using SqlSugar;
using System;
using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Core.Plugins;
using ShadowViewer.Core.Services;
using ShadowViewer.Core;
using ShadowViewer.Core.Models;
using ShadowViewer.Plugin.PluginManager.Pages;

namespace ShadowViewer.Plugin.PluginManager
{
    /// <summary>
    /// 插件管理器主类
    /// </summary>
    [MainPlugin]
    public partial class PluginManagerPlugin : AShadowViewerPlugin
    {
        /// <inheritdoc />
        public PluginManagerPlugin(ICallableService caller, ISqlSugarClient db, PluginEventService pluginEventService,
            ILogger logger, PluginLoader pluginService, INotifyService notifyService) :
            base(caller, db, pluginService, notifyService, logger, pluginEventService)
        {
            DiFactory.Services.Register<PluginManagerSettingsViewModel>(reuse: Reuse.Transient);
            DiFactory.Services.Register<PluginViewModel>(reuse: Reuse.Transient);
            DiFactory.Services.Register<PluginStoreViewModel>(reuse: Reuse.Transient);
        }

        /// <inheritdoc />
        public override PluginMetaData MetaData => Meta;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override ShadowTag AffiliationTag { get; } =
            new ShadowTag("PluginManager", "#ffd657", "#000000", null, "PluginManager");

        /// <inheritdoc />
        public override string DisplayName => "插件管理器";
    }
}