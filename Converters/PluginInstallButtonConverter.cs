using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System;
using ShadowViewer.Plugin.PluginManager.Enums;

namespace ShadowViewer.Plugin.PluginManager.Converters;
/// <summary>
/// PluginStore InstallButton Text
/// </summary>
public class PluginInstallButtonConverter : DependencyObject, IValueConverter
{
    /// <summary>
    /// Identifies the <see cref="InstallValue"/> property.
    /// </summary>
    public static readonly DependencyProperty InstallValueProperty =
        DependencyProperty.Register(nameof(InstallValue), typeof(object), typeof(PluginInstallButtonConverter), new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="InstalledValue"/> property.
    /// </summary>
    public static readonly DependencyProperty InstalledValueProperty =
        DependencyProperty.Register(nameof(InstalledValue), typeof(object), typeof(PluginInstallButtonConverter), new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="UpgradeValue"/> property.
    /// </summary>
    public static readonly DependencyProperty UpgradeValueProperty =
        DependencyProperty.Register(nameof(UpgradeValue), typeof(object), typeof(PluginInstallButtonConverter), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the value to be returned when the object is neither null nor empty
    /// </summary>
    public object InstalledValue
    {
        get => GetValue(InstalledValueProperty);
        set => SetValue(InstalledValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the value to be returned when the object is either null or empty
    /// </summary>
    public object InstallValue
    {
        get => GetValue(InstallValueProperty);
        set => SetValue(InstallValueProperty, value);
    }
    /// <summary>
    /// Gets or sets the value to be returned when the object is either null or empty
    /// </summary>
    public object UpgradeValue
    {
        get => GetValue(UpgradeValueProperty);
        set => SetValue(UpgradeValueProperty, value);
    }



    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not PluginInstallStatus) return InstallValue;
        return value switch
        {
            PluginInstallStatus.Installed => InstalledValue,
            PluginInstallStatus.Upgrade => UpgradeValue,
            PluginInstallStatus.None => InstallValue,
            _ => InstallValue
        };
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {

        throw new NotImplementedException();
    }
}
