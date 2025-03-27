using DryIoc;
using Serilog;
using ShadowViewer.Plugin.PluginManager.ViewModels;
using SqlSugar;
using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Core.Plugins;
using ShadowViewer.Core.Services;
using ShadowViewer.Core;

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

        /// <inheritdoc />
        public override string DisplayName => "插件管理器";
    }
}