using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Plugin.PluginManager.Models;

namespace ShadowViewer.Plugin.PluginManager.Controls;

/// <summary>
/// 插件预览控件，用于在ContentDialog中显示插件信息
/// </summary>
public sealed partial class PluginPreviewControl : UserControl
{
    /// <summary>
    /// ViewModel依赖属性
    /// </summary>
    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(
            nameof(ViewModel),
            typeof(UiPlugin),
            typeof(PluginPreviewControl),
            new PropertyMetadata(null));

    /// <summary>
    /// 构造函数
    /// </summary>
    public PluginPreviewControl()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// 预览模型
    /// </summary>
    public UiPlugin ViewModel
    {
        get => (UiPlugin)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
}
