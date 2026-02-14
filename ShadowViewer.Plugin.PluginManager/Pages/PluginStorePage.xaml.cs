using DryIoc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Serilog;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Plugin.PluginManager.Helpers;
using ShadowViewer.Plugin.PluginManager.ViewModels;
using ShadowViewer.Sdk.Services;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ShadowViewer.Plugin.PluginManager.Pages;

/// <summary>
/// 商店页
/// </summary>
public sealed partial class PluginStorePage
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
    /// 进入页面
    /// </summary>
    /// <param name="e"></param>
    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is Uri uri) _ = ViewModel.NavigateTo(uri);
        
        if (ViewModel.PluginManagerConfig.StoreUri.IsNullOrEmpty())
        {
            StoreUriTeachingTip.IsOpen = true;
            return;
        }
        
        try
        {
            LoadingProgress.Visibility = Visibility.Visible;
            await ViewModel.Refresh();
        }
        catch (Exception exception)
        {
            Log.Error("获取插件报错:{E}", exception);
            ViewModel.NotifyService.NotifyTip(this, $"获取插件报错:{exception}",
                InfoBarSeverity.Error);
        }
        finally
        {
            LoadingProgress.Visibility = Visibility.Collapsed;
        }
    }

    /// <summary>
    /// Navigate to settings page
    /// </summary>
    private void GoToSettings_Click(TeachingTip sender, object args)
    {
        StoreUriTeachingTip.IsOpen = false;
        var navigateService = DiFactory.Services.Resolve<INavigateService>();
        navigateService.Navigate(new Uri("shadow://pluginmanager/settings"));
    }

    /// <summary>
    /// 
    /// </summary>
    private async void VersionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not FrameworkElement { Tag: string id }) return;
        await ViewModel.ChangeVersion(id);
    }
}