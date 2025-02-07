using DryIoc;
using Serilog;
using ShadowViewer.Plugin.PluginManager.ViewModels;
using ShadowViewer.Plugins;
using SqlSugar;
using System;
using ShadowPluginLoader.MetaAttributes;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Core.Plugins;
using ShadowViewer.Core.Services;
using ShadowViewer.Core;
using ShadowViewer.Core.Models;

namespace ShadowViewer.Plugin.PluginManager
{
    /// <summary>
    /// 插件管理器主类
    /// </summary>
    [AutoPluginMeta]
    public partial class PluginManagerPlugin : AShadowViewerPlugin
    {
        /// <inheritdoc />
        public PluginManagerPlugin(ICallableService caller, ISqlSugarClient db, PluginEventService pluginEventService,
            CompressService compressService, ILogger logger, PluginLoader pluginService, INotifyService notifyService) :
            base(caller, db, pluginEventService, compressService, logger, pluginService, notifyService)
        {
            DiFactory.Services.Register<PluginViewModel>(reuse: Reuse.Transient);
            DiFactory.Services.Register<PluginStoreViewModel>(reuse: Reuse.Transient);
        }

        /// <inheritdoc />
        public override PluginMetaData MetaData => Meta;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override LocalTag AffiliationTag { get; } = new LocalTag("PluginManager", "#000000", "#ffd657");

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override Type? SettingsPage => null;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool CanSwitch => false;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool CanDelete => false;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool CanOpenFolder => false;


        /// <inheritdoc />
        public override string DisplayName => "插件管理器";
    }
}