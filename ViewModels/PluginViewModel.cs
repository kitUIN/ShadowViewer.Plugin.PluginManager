using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Serilog;
using ShadowPluginLoader.MetaAttributes;
using ShadowViewer.Plugins;
using ShadowViewer.Core.Helpers;
using ShadowViewer.Core;

namespace ShadowViewer.Plugin.PluginManager.ViewModels;

public partial class PluginViewModel : ObservableObject
{
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
    /// 加载插件
    /// </summary>
    [RelayCommand]
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
            }

            InitPlugins();
        }
    }
}