using System;
using DryIoc;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Serilog;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Plugin.PluginManager.Models;
using ShadowViewer.Plugin.PluginManager.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ShadowViewer.Plugin.PluginManager.Pages;

/// <summary>
/// 商店页
/// </summary>
public sealed partial class PluginStorePage : Page
{
    /// <summary>
    /// ViewModel
    /// </summary>
    private PluginStoreViewModel ViewModel { get; } = DiFactory.Services.Resolve<PluginStoreViewModel>();

    /// <summary>
    /// 
    /// </summary>
    public PluginStorePage()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// 升级点击
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void UpgradeClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (sender is not Button { Tag: PluginStoreModel model }) return;
            var asset = "";
            if (model.MetaData.Assets.Count > 1)
            {
                UpgradeSelectGridView.ItemsSource = model.MetaData.Assets;
                var result = await UpgradeSelectContentDialog.ShowAsync();
                Log.Information("{e}", result);
                if (result == ContentDialogResult.Primary && UpgradeSelectGridView.SelectedIndex >= 0)
                {
                    asset = model.MetaData.Assets[UpgradeSelectGridView.SelectedIndex].BrowserDownloadUrl;
                }
                else
                {
                    return;
                }
            }
            else
            {
                asset = model.MetaData.Assets[0].BrowserDownloadUrl;
            }

            if (model.CouldUpdate)
            {
                ViewModel.Upgrade(model.Id, asset);
            }
            else
            {
                ViewModel.Install(asset);
            }
        }
        catch (Exception ex)
        {
            Log.Error("UpgradeClick ERROR:{ex}", ex);
        }
    }

    /// <summary>
    /// Init
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
            await ViewModel.Init();
            LoadingProgress.Visibility = Visibility.Collapsed;
        }
        catch (Exception exception)
        {
            Log.Error("获取插件报错:{E}", exception);
            ViewModel.NotifyService.NotifyTip(this, $"获取插件报错:{exception}",
                InfoBarSeverity.Error);
        }
    }
}