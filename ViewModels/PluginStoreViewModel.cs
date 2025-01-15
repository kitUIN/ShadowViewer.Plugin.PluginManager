using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShadowViewer.Plugin.PluginManager.Helpers;
using ShadowViewer.Plugin.PluginManager.Models;

namespace ShadowViewer.Plugin.PluginManager.ViewModels;
public class PluginStoreViewModel : ObservableObject
{
    /// <summary>
    /// 插件列表
    /// </summary>
    public ObservableCollection<PluginStoreModel> Models { get; } = [];
    public PluginLoader PluginService { get; }
    /// <summary>
    /// 
    /// </summary>
    public PluginStoreViewModel(PluginLoader pluginLoader)
    {
        PluginService = pluginLoader;
        Init();
    }

    public async void Init()
    {
        Models.Clear();
        var pluginList = await PluginStoreHelper.Instance.GetPluginList();
        foreach (var meta in pluginList)
        {
            var model = new PluginStoreModel(meta);
            Models.Add(model);
        }
    }

    public async void Install(string uri)
    {
        await PluginService.ImportFromZipAsync(uri);
    }
    public async void Upgrade(string id ,string uri)
    {
        await PluginService.UpgradePlugin(id,uri);
    }
}