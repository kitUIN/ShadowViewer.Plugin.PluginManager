using DryIoc;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;
using Serilog;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Plugin.PluginManager.ViewModels;
using System;

namespace ShadowViewer.Plugin.PluginManager.Pages
{
    /// <summary>
    /// 插件管理器 页面
    /// </summary>
    public sealed partial class PluginPage
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public PluginViewModel ViewModel { get; private set; } = DiFactory.Services.Resolve<PluginViewModel>();

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PluginPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 第一次进入时展示说明
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SecurityContentDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.InitPlugins();
                if (ViewModel.PluginManagerConfig.PluginSecurityStatement) return;
                await SecurityContentDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Log.Error("SecurityContentDialog_OnLoaded Error,{ex}", ex);
            }
        }

        /// <inheritdoc />
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}