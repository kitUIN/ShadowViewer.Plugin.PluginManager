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

public sealed partial class PluginStorePage : Page
{
    private PluginStoreViewModel ViewModel { get; } = DiFactory.Services.Resolve<PluginStoreViewModel>();

    public PluginStorePage()
    {
        this.InitializeComponent();
    }

    private async void UpgradeClick(object sender, RoutedEventArgs e)
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
            ViewModel.Upgrade(model.Id,asset);
        }
        else
        {
            ViewModel.Install(asset);
        }
    }
}
