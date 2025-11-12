using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System;
using ShadowViewer.Plugin.PluginManager.Models;

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
        return value is UiPlugin { CanOpenFolder: false, CanDelete: false }
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