using DryIoc;
using ShadowViewer.Plugin.PluginManager.ViewModels;
using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Core.Plugins;
using ShadowViewer.Plugin.PluginManager.I18n;

namespace ShadowViewer.Plugin.PluginManager
{
    /// <summary>
    /// 插件管理器主类
    /// </summary>
    [MainPlugin(BuiltIn = true)]
    [CheckAutowired]
    public partial class PluginManagerPlugin : AShadowViewerPlugin
    {
        /// <summary>
        /// 
        /// </summary>
        partial void ConstructorInit()
        {
            DiFactory.Services.Register<PluginManagerSettingsViewModel>(reuse: Reuse.Transient);
            DiFactory.Services.Register<PluginViewModel>(reuse: Reuse.Transient);
            DiFactory.Services.Register<PluginStoreViewModel>(reuse: Reuse.Transient);
        }

        /// <inheritdoc />
        public override string DisplayName => I18N.PluginManager;
    }
}