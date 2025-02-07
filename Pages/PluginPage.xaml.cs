using DryIoc;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Serilog;
using ShadowViewer.Plugin.PluginManager.ViewModels;
using System;
using ShadowPluginLoader.WinUI;
using Microsoft.UI.Xaml.Media.Animation;
using ShadowViewer.Plugin.PluginManager.I18n;
using ShadowViewer.Core;
using ShadowViewer.Core.Extensions;
using ShadowViewer.Core.Plugins;

namespace ShadowViewer.Plugin.PluginManager.Pages
{
    /// <summary>
    /// ��������� ҳ��
    /// </summary>
    public sealed partial class PluginPage : Page
    {
        private PluginViewModel ViewModel { get; } = DiFactory.Services.Resolve<PluginViewModel>();
        private PluginLoader PluginService { get; } = DiFactory.Services.Resolve<PluginLoader>();
        
        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>
        public PluginPage()
        {
            this.InitializeComponent();
            ViewModel.InitPlugins();
        }
        
        /// <summary>
        /// ǰ���������
        /// </summary>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as HyperlinkButton;
            if(button!=null&& button.Tag is string tag &&PluginService.GetPlugin(tag) is { SettingsPage: not null } plugin)
            {
                Frame.Navigate(plugin.SettingsPage, null,
                    new SlideNavigationTransitionInfo { 
                        Effect = SlideNavigationTransitionEffect.FromRight });
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement { Tag: AShadowViewerPlugin  plugin }) return;

            var dialog = new ContentDialog
            {
                XamlRoot = XamlRoot,
                Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                DefaultButton = ContentDialogButton.Primary,
                Title = I18N.Delete,
                Content = I18N.Delete,
                IsPrimaryButtonEnabled = false,
                CloseButtonText = ResourcesHelper.GetString(ResourceKey.Cancel)
            };
            dialog.IsPrimaryButtonEnabled = true;
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.PrimaryButtonText = I18N.Accept;
            dialog.PrimaryButtonClick += async (sender, args) =>
            {
                PluginService.RemovePlugin(plugin.Id);
            };
            await dialog.ShowAsync();
            
        }

        private void More_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement source) return;
            if (sender is FrameworkElement { Tag: AShadowViewerPlugin { CanOpenFolder: false, CanDelete: false } })return;
            var flyout = FlyoutBase.GetAttachedFlyout(source);
            flyout?.ShowAt(source);
        }

        private async void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement { Tag: AShadowViewerPlugin plugin }) return;
            try
            {
                var file = await plugin.GetType().Assembly.Location.GetFile();
                var folder = await file.GetParentAsync();
                folder.LaunchFolderAsync();
            }
            catch(Exception ex)
            {
                Log.Error("���ļ��д���{Ex}", ex);
            }
        }

        private void More_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement { Tag: AShadowViewerPlugin { CanOpenFolder: false, CanDelete: false } } source)
            {
                source.Visibility = Visibility.Collapsed;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PluginStorePage), null,
                new SlideNavigationTransitionInfo
                {
                    Effect = SlideNavigationTransitionEffect.FromRight
                });
        }
    }
}
