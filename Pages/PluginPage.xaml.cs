using DryIoc;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Serilog;
using ShadowViewer.Extensions;
using ShadowViewer.Plugin.PluginManager.ViewModels;
using ShadowViewer.Plugins;
using System;
using ShadowPluginLoader.WinUI;
using Microsoft.UI.Xaml.Media.Animation;

namespace ShadowViewer.Plugin.PluginManager.Pages
{
    /// <summary>
    /// 插件管理器 页面
    /// </summary>
    public sealed partial class PluginPage : Page
    {
        private PluginViewModel ViewModel { get; } = DiFactory.Services.Resolve<PluginViewModel>();
        private PluginLoader PluginService { get; } = DiFactory.Services.Resolve<PluginLoader>();
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PluginPage()
        {
            this.InitializeComponent();
            ViewModel.InitPlugins();
        }
        
        /// <summary>
        /// 前往插件设置
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement { Tag: AShadowViewerPlugin  plugin }) return;
            PluginService.RemovePlugin(plugin.Id);
            /*
            var contentDialog= XamlHelper.CreateMessageDialog(XamlRoot,
                ResourcesHelper.GetString(ResourceKey.DeletePlugin) + plugin.MetaData.Name,
                ResourcesHelper.GetString(ResourceKey.DeletePluginMessage));
            contentDialog.IsPrimaryButtonEnabled = true;
            contentDialog.DefaultButton = ContentDialogButton.Close;
            contentDialog.PrimaryButtonText = ResourcesHelper.GetString(ResourceKey.Confirm) ;
            contentDialog.PrimaryButtonClick += async (dialog, args) =>
            {
                var flag = await PluginService.DeleteAsync(plugin.MetaData.Id);
                    
            };
            await contentDialog.ShowAsync();
            */
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
                Log.Error("打开文件夹错误{Ex}", ex);
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
