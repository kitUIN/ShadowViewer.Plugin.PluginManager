using DryIoc;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Serilog;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Plugin.PluginManager.ViewModels;
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
    /// Init
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
    {
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
    /// 进入页面
    /// </summary>
    /// <param name="e"></param>
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is Uri uri) _ = ViewModel.NavigateTo(uri);
    }
}