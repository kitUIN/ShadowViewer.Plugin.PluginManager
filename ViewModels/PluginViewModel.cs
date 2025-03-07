﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Serilog;
using ShadowPluginLoader.MetaAttributes;
using ShadowViewer.Core.Helpers;
using ShadowViewer.Core;
using ShadowViewer.Core.Plugins;
using ShadowViewer.Core.Extensions;

namespace ShadowViewer.Plugin.PluginManager.ViewModels;

public partial class PluginViewModel : ObservableObject
{
    /// <summary>
    /// 插件安全声明弹出框确定
    /// </summary>
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddPluginCommand))]
    private bool pluginSecurityCheck;

    /// <summary>
    /// 插件服务
    /// </summary>
    [Autowired]
    public PluginLoader PluginService { get; }

    /// <summary>
    /// 日志服务
    /// </summary>
    [Autowired]
    public ILogger Logger { get; }

    /// <summary>
    /// 插件列表
    /// </summary>
    public ObservableCollection<AShadowViewerPlugin> Plugins { get; } = [];

    /// <summary>
    /// 初始化插件列表
    /// </summary>
    public void InitPlugins()
    {
        Plugins.Clear();
        foreach (var plugin in PluginService.GetPlugins())
        {
            Plugins.Add(plugin);
        }
    }

    /// <summary>
    /// 同意安全声明
    /// </summary>
    [RelayCommand]
    private Task SecurityConfirm()
    {
        PluginManagerPlugin.Setting.PluginSecurityStatement = PluginSecurityCheck;
        return Task.CompletedTask;
    }

    /// <summary>
    /// 打开文件夹
    /// </summary>
    [RelayCommand]
    private async Task OpenFolder(AShadowViewerPlugin plugin)
    {
        try
        {
            var file = await plugin.GetType().Assembly.Location.GetFile();
            var folder = await file.GetParentAsync();
            folder.LaunchFolderAsync();
        }
        catch (Exception ex)
        {
            Log.Error("打开文件夹错误{Ex}", ex);
        }
    }

    /// <summary>
    /// 判断是否展示更多按钮
    /// </summary>
    /// <param name="plugin"></param>
    /// <returns></returns>
    public static Visibility CheckMoreVisible(AShadowViewerPlugin plugin)
    {
        return plugin is { CanOpenFolder: false, CanDelete: false } ? Visibility.Collapsed : Visibility.Visible;
    }

    /// <summary>
    /// 加载插件
    /// </summary>
    [RelayCommand(CanExecute = nameof(PluginSecurityCheck))]
    private async Task AddPlugin(XamlRoot root)
    {
        var file = await FileHelper.SelectFileAsync(root, "ShadowViewer_AddPlugin",
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