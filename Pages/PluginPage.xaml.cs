using DryIoc;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Serilog;
using ShadowViewer.Plugin.PluginManager.ViewModels;
using System;
using ShadowPluginLoader.WinUI;
using Microsoft.UI.Xaml.Media.Animation;
using ShadowViewer.Plugin.PluginManager.I18n;
using ShadowViewer.Core;
using ShadowViewer.Core.Plugins;

namespace ShadowViewer.Plugin.PluginManager.Pages
{
    /// <summary>
    /// ��������� ҳ��
    /// </summary>
    public sealed partial class PluginPage : Page
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public PluginViewModel ViewModel { get; } = DiFactory.Services.Resolve<PluginViewModel>();

        private PluginLoader PluginService { get; } = DiFactory.Services.Resolve<PluginLoader>();

        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>
        public PluginPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// ǰ���������
        /// </summary>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as HyperlinkButton;
            if (button != null && button.Tag is string tag && PluginService.GetPlugin(tag) is
                    { SettingsPage: not null } plugin)
            {
                Frame.Navigate(plugin.SettingsPage, null,
                    new SlideNavigationTransitionInfo
                    {
                        Effect = SlideNavigationTransitionEffect.FromRight
                    });
            }
        }
        /// <summary>
        /// ɾ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is not FrameworkElement { Tag: AShadowViewerPlugin plugin }) return;

                var dialog = new ContentDialog
                {
                    XamlRoot = XamlRoot,
                    Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    DefaultButton = ContentDialogButton.Close,
                    Title = I18N.Delete,
                    Content = I18N.Delete,
                    IsPrimaryButtonEnabled = false,
                    PrimaryButtonText = I18N.Accept,
                    CloseButtonText = I18N.Cancel,
                };
                dialog.PrimaryButtonClick += (_, _) => PluginService.RemovePlugin(plugin.Id);
                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Log.Error("Delete_Click Error,{ex}", ex);
            }
        }
        /// <summary>
        /// ��ת�̵�ҳ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void PluginStore_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PluginStorePage), null,
                new SlideNavigationTransitionInfo
                {
                    Effect = SlideNavigationTransitionEffect.FromRight
                });
        }

        /// <summary>
        /// ��һ�ν���ʱչʾ˵��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SecurityContentDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.InitPlugins();
                if (PluginManagerPlugin.Setting.PluginSecurityStatement) return;
                await SecurityContentDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Log.Error("SecurityContentDialog_OnLoaded Error,{ex}", ex);
            }
        }
    }
}