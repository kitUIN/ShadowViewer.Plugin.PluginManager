using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Serilog;
using ShadowViewer.Extensions;
using ShadowViewer.Helpers;
using SqlSugar;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using ShadowViewer.Plugins;
using ShadowViewer.Analyzer.Attributes;

namespace ShadowViewer.Plugin.PluginManager.ViewModels
{
    [AutoDi]
    public partial class PluginViewModel: ObservableObject
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
            var file = await FileHelper.SelectFileAsync(root, "AddPlugin", PickerViewMode.List, ".zip", ".rar", ".7z", ".tar");
            if (file != null)
            {
                // Caller.ImportPlugin(this, new List<StorageFile> { file });
            }
        }
        /// <summary>
        /// 更多
        /// </summary>
        [RelayCommand]
        private void GoPluginTip()
        {
            var url = new Uri("https://github.com/kitUIN/ShadowViewer/blob/master/README.md#%E6%8F%92%E4%BB%B6%E5%88%97%E8%A1%A8");
            url.LaunchUriAsync();
        }
    }
}
