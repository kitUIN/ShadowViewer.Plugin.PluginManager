using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using ShadowViewer.Plugin.PluginManager.ViewModels;
using System;
using ShadowViewer.Core.Plugins;

namespace ShadowViewer.Plugin.PluginManager.Converters;

/// <summary>
/// 判断是否展示更多按钮
/// </summary>
public class PluginMoreVisibilityConverter : IValueConverter
{
    /// <summary>
    /// 判断是否展示更多按钮
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is AShadowViewerPlugin { CanOpenFolder: false, CanDelete: false }
            ? Visibility.Collapsed
            : Visibility.Visible;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}