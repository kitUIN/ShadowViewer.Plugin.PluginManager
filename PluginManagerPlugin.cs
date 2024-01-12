using DryIoc;
using Serilog;
using Serilog.Core;
using ShadowViewer.Analyzer.Attributes;
using ShadowViewer.Interfaces;
using ShadowViewer.Models;
using ShadowViewer.Plugin.PluginManager.Services;
using ShadowViewer.Plugin.PluginManager.ViewModels;
using ShadowViewer.Plugins;
using ShadowViewer.Services;

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Plugin.PluginManager
{
    /// <summary>
    /// ����
    /// </summary>
    [AutoPluginMeta]
    public partial class PluginManagerPlugin : PluginBase
    {
        /// <summary>
        /// �Զ�����ע��
        /// </summary>
        public PluginManagerPlugin(ICallableService callableService, ISqlSugarClient sqlSugarClient, CompressService compressServices, IPluginService pluginService, ILogger logger) : base(callableService, sqlSugarClient, compressServices, pluginService, logger)
        {
            DiFactory.Services.Register<PluginViewModel>(reuse: Reuse.Transient);
        }
        /// <summary>
        /// ֱ�ӳ�ʼ��DI
        /// </summary>
        public static void PluginServiceInit()
        {
            DiFactory.Services.Register<IPluginService, PluginService>(Reuse.Singleton);
        }
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
        protected override void PluginEnabled()
        {
            //Logger.Information("[{ID}]����PluginEnabled", MetaData.Id);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void PluginDisabled()
        {
            //Logger.Information("[{ID}]����PluginDisabled", MetaData.Id);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void PluginDeleting()
        {
            //Logger.Information("[{ID}]����PluginDeleting", MetaData.Id);
        }
    }
}
