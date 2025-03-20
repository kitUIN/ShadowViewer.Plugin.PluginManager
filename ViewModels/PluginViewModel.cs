using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml.Media.Animation;
using Serilog;
using ShadowPluginLoader.MetaAttributes;
using ShadowViewer.Core.Helpers;
using ShadowViewer.Core;
using ShadowViewer.Core.Plugins;
using ShadowViewer.Core.Extensions;
using ShadowViewer.Core.Services;
using ShadowViewer.Plugin.PluginManager.Models;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Plugin.PluginManager.I18n;
using ShadowViewer.Plugin.PluginManager.Pages;

namespace ShadowViewer.Plugin.PluginManager.ViewModels;

public partial class PluginViewModel : ObservableObject
{
    /// <summary>
    /// 插件安全声明弹出框确定
    /// </summary>
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddPluginCommand))]
    private bool pluginSecurityCheck = PluginManagerPlugin.Setting.PluginSecurityStatement;

    /// <summary>
    /// 插件服务
    /// </summary>
    [Autowired]
    public PluginLoader PluginService { get; }

    /// <summary>
    /// 导航服务
    /// </summary>
    [Autowired]
    public INavigateService NavigateService { get; }

    /// <summary>
    /// 日志服务
    /// </summary>
    [Autowired]
    public ILogger Logger { get; }

    /// <summary>
    /// 插件列表
    /// </summary>
    public ObservableCollection<UiPlugin> Plugins { get; } = [];

    /// <summary>
    /// 初始化插件列表
    /// </summary>
    public void InitPlugins()
    {
        Plugins.Clear();
        foreach (var plugin in PluginService.GetPlugins())
        {
            Plugins.Add(new UiPlugin(plugin));
        }
    }

    /// <summary>
    /// 同意安全声明
    /// </summary>
    [RelayCommand]
    private void SecurityConfirm()
    {
        PluginManagerPlugin.Setting.PluginSecurityStatement = PluginSecurityCheck;
    }

    /// <summary>
    /// 前往插件设置页面
    /// </summary>
    [RelayCommand]
    private void ToPluginSettingPage(Type? settingsPage)
    {
        if (settingsPage == null) return;
        NavigateService.Navigate(settingsPage, null,
            new SlideNavigationTransitionInfo
            {
                Effect = SlideNavigationTransitionEffect.FromRight
            });
    }

    /// <summary>
    /// 前往插件商店页面
    /// </summary>
    [RelayCommand]
    private void ToPluginStorePage()
    {
        NavigateService.Navigate(typeof(PluginStorePage), null,
            new SlideNavigationTransitionInfo
            {
                Effect = SlideNavigationTransitionEffect.FromRight
            });
    }

    /// <summary>
    /// 打开文件夹
    /// </summary>
    [RelayCommand]
    private async Task OpenFolder(Type pluginType)
    {
        try
        {
            var file = await pluginType.Assembly.Location.GetFile();
            var folder = await file.GetParentAsync();
            folder.LaunchFolderAsync();
        }
        catch (Exception ex)
        {
            Log.Error("打开文件夹错误{Ex}", ex);
        }
    }

    /// <summary>
    /// 删除插件
    /// </summary>
    [RelayCommand]
    private async Task Delete(string pluginId)
    {
        try
        {
            var dialog = new ContentDialog
            {
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                DefaultButton = ContentDialogButton.Close,
                Title = I18N.Delete,
                Content = I18N.Delete,
                IsPrimaryButtonEnabled = false,
                PrimaryButtonText = I18N.Accept,
                CloseButtonText = I18N.Cancel,
            };
            dialog.PrimaryButtonClick += (_, _) => PluginService.RemovePlugin(pluginId);
            await DialogHelper.ShowDialog(dialog);
        }
        catch (Exception ex)
        {
            Log.Error("Delete_Click Error,{ex}", ex);
        }
    }


    /// <summary>
    /// 加载插件
    /// </summary>
    [RelayCommand(CanExecute = nameof(PluginSecurityCheck))]
    private async Task AddPlugin(XamlRoot root)
    {
        var file = await FileHelper.SelectFileAsync("ShadowViewer_AddPlugin",
            PickerViewMode.List, FileHelper.Zips);
        if (file != null)
        {
            try
            {
                await PluginService.ImportFromZipAsync(file.Path);
            }
            catch (Exception ex)
            {
                Logger.Error("添加插件失败:{Ex}", ex);
                return;
            }

            InitPlugins();
        }
    }
}