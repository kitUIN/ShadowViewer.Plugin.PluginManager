using System;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Plugin.PluginManager.ViewModels;
using ShadowPluginLoader.WinUI;
using DryIoc;
using Microsoft.UI.Xaml;
using Serilog;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ShadowViewer.Plugin.PluginManager.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PluginManagerSettingsPage : Page
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public PluginManagerSettingsViewModel ViewModel { get; } =
            DiFactory.Services.Resolve<PluginManagerSettingsViewModel>();

        /// <summary>
        /// 
        /// </summary>
        public PluginManagerSettingsPage()
        {
            this.InitializeComponent();
        }

        private async void SecurityContentDialog_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await SecurityContentDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Log.Error("SecurityContentDialog_OnClick: {Ex}", ex);
            }
        }
    }
}