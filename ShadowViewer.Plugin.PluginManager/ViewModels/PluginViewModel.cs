using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Scriban;
using Serilog;
using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI;
using ShadowPluginLoader.WinUI.Extensions;
using ShadowViewer.Plugin.PluginManager.Configs;
using ShadowViewer.Plugin.PluginManager.Constants;
using ShadowViewer.Plugin.PluginManager.I18n;
using ShadowViewer.Plugin.PluginManager.Models;
using ShadowViewer.Plugin.PluginManager.Pages;
using ShadowViewer.Sdk;
using ShadowViewer.Sdk.Extensions;
using ShadowViewer.Sdk.Helpers;
using ShadowViewer.Sdk.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace ShadowViewer.Plugin.PluginManager.ViewModels;

public partial class PluginViewModel : ObservableObject
{
    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    public PluginManagerConfig PluginManagerConfig { get; }

    partial void ConstructorInit()
    {
        PluginSecurityCheck = PluginManagerConfig.PluginSecurityStatement;
    }


    /// <summary>
    /// 插件安全声明弹出框确定
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddPluginCommand))]
    public partial bool PluginSecurityCheck { get; set; }

    /// <summary>
    /// Sdk 版本号
    /// </summary>
    public string SdkVersion { get; } = I18N.SdkVersion + ": " + PluginConstants.SdkVersion;

    /// <summary>
    /// 插件服务
    /// </summary>
    [Autowired]
    public PluginLoader PluginService { get; }

    /// <summary>
    /// 通知服务
    /// </summary>
    [Autowired]
    public INotifyService NotifyService { get; }

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
    /// 文件选择服务
    /// </summary>
    [Autowired]
    public IFilePickerService FilePickerService { get; }

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
        foreach (var plugin in PluginService.GetPlugins().OrderBy(f => f.Id))
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
        PluginManagerConfig.PluginSecurityStatement = PluginSecurityCheck;
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
            var plugin = PluginService.GetPlugin(pluginId);
            if (plugin == null)
            {
                return;
            }

            await NotifyService.ShowDialog(this, XamlHelper.CreateMessageDialog(
                $"{I18N.Delete}{plugin.DisplayName}(v{plugin.MetaData.Version})?",
                $"{Sdk.I18n.I18N.Confirm}{I18N.Delete}{plugin.DisplayName}(v{plugin.MetaData.Version})?",
                async void (sender, args) =>
                {
                    try
                    {
                        args.Cancel = true;
                        sender.PrimaryButtonText = I18N.Installing;
                        await PluginService.RemovePlugin(pluginId);
                        sender.Hide();
                        await NotifyService.ShowDialog(this, XamlHelper.CreateMessageDialog(
                            $"{I18N.RestartApp}?",
                            I18N.RestartAppForPluginDelete,
                            RestartApp
                        ));
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "Catch Error in DeleteAsync");
                    }
                }
            ));
            
        }
        catch (Exception ex)
        {
            Log.Error("Delete_Click Error,{ex}", ex);
        }
    }

    private void RestartApp(ContentDialog dialog, ContentDialogButtonClickEventArgs args)
    {

        var appUserModelId = "kitUIN.ShadowViewer_fka8f3r9nhqje!App";

        Process.Start(new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c timeout /t 3 & explorer.exe shell:AppsFolder\\{appUserModelId}",
            CreateNoWindow = true,
            UseShellExecute = false
        });

        Environment.Exit(0);
    }


    /// <summary>
    /// 加载插件
    /// </summary>
    [RelayCommand(CanExecute = nameof(PluginSecurityCheck))]
    private async Task AddPlugin(XamlRoot root)
    {
        var file = await FilePickerService.PickSingleFileAsync(
            [".zip", ".rar", ".sdow"],
            PickerLocationId.Downloads,
            PickerViewMode.List,
            "ShadowViewer_AddPlugin");
        if (file != null)
        {
            try
            {
                await PluginService.CreatePipeline().Feed(new Uri(file.Path)).ProcessAsync();
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