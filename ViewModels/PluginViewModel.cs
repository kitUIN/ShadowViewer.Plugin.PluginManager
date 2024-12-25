using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using ShadowViewer.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using ShadowViewer.Plugins;
using ShadowViewer.Analyzer.Attributes;

namespace ShadowViewer.Plugin.PluginManager.ViewModels;

[AutoDi(true, false, false, false, false, true, false, false)]
public partial class PluginViewModel : ObservableObject
{
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