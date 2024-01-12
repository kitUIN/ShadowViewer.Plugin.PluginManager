using Serilog;
using Serilog.Core;
using ShadowViewer.Interfaces;
using ShadowViewer.Models;
using ShadowViewer.Plugins;
using ShadowViewer.Services;
using ShadowViewer.Services.Interfaces;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Plugin.Example
{
    [AutoPluginMeta]
    public partial class ExamplePlugin : PluginBase
    {
        public ExamplePlugin(ICallableService callableService, ISqlSugarClient sqlSugarClient, CompressService compressServices, IPluginService pluginService, ILogger logger) : base(callableService, sqlSugarClient, compressServices, pluginService, logger)
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override LocalTag AffiliationTag { get; } = new LocalTag("Example", "#000000", "#ffd657");

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
        protected override void PluginEnabled()
        {
            Logger.Information("[{ID}]´¥·¢PluginEnabled", MetaData.Id);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void PluginDisabled()
        {
            Logger.Information("[{ID}]´¥·¢PluginDisabled", MetaData.Id);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void PluginDeleting()
        {
            Logger.Information("[{ID}]´¥·¢PluginDeleting", MetaData.Id);
        }
    }
}
