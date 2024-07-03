using DryIoc;
using Serilog;
using ShadowViewer.Models;
using ShadowViewer.Plugin.PluginManager.ViewModels;
using ShadowViewer.Plugins;
using ShadowViewer.Services;

using SqlSugar;
using System;
using ShadowPluginLoader.MetaAttributes;

namespace ShadowViewer.Plugin.PluginManager
{

    /// <summary>
    /// 插件管理器主类
    /// </summary>
    [AutoPluginMeta]
    public partial class PluginManagerPlugin : PluginBase
    {

        /// <inheritdoc />
        public PluginManagerPlugin(ICallableService callableService,
            ISqlSugarClient sqlSugarClient,
            CompressService compressServices,
            PluginLoader pluginService,
            ILogger logger) :
            base(callableService,
                sqlSugarClient,
                compressServices,
                pluginService, logger)
        {
            DiFactory.Services.Register<PluginViewModel>(reuse: Reuse.Transient);
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


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override string GetId()
        {
            return Meta.Id;
        }
    }
}
