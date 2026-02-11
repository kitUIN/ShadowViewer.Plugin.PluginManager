using CustomExtensions.WinUI;
using FluentIcons.Common;
using FluentIcons.WinUI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ShadowViewer.Plugin.PluginManager.Models;

namespace ShadowViewer.Plugin.PluginManager.Helpers;

/// <summary>
/// 
/// </summary>
public static class PluginHelper
{
    /// <summary>
    /// Gets the bitmap from zip asynchronous.
    /// </summary>
    private static async Task FillBitmapFromZipAsync(BitmapImage bitmap, string zipPath, string entryPath)
    {
        try
        {
            using var archive = ZipFile.OpenRead(zipPath);
            var entry = archive.GetEntry(entryPath);
            if (entry == null) return;

            await using var entryStream = entry.Open();
            using var ms = new MemoryStream();
            await entryStream.CopyToAsync(ms);
            ms.Position = 0;

            using var winrtStream = ms.AsRandomAccessStream();
            // SetSourceAsync 会在内部处理调度，确保在 UI 线程更新
            await bitmap.SetSourceAsync(winrtStream);
        }
        catch (Exception ex)
        {
            // 可以在这里打日志，或者设置一个默认的失败图标
            System.Diagnostics.Debug.WriteLine($"Failed to load icon from zip: {ex.Message}");
        }
    }
    /// <summary>
    /// Gets the login icon.
    /// </summary>
    /// <param name="plugin">The plugin.</param>
    /// <returns></returns>
    public static IconElement? GetLoginIcon(UiPlugin plugin)
    {
        return GetLoginIcon(plugin.MetaData.Logo, plugin.ZipPath);
    }

    /// <summary>
    /// Gets the login icon.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="parameter">The parameter.</param>
    /// <returns></returns>
    public static IconElement? GetLoginIcon(object? value, object? parameter)
    {
        if (value is not string valueString) return null;
        var uri = new Uri(valueString);
        string glyph;
        var zipPath = parameter as string;
        switch (uri.Scheme)
        {
            case "ms-plugin" or "ms-appx" or "http" or "https":
                if (uri.Scheme == "ms-plugin" && zipPath == null)
                {
                    uri = new Uri(valueString.PluginPath());
                }
                else if (uri.Scheme == "ms-plugin" && zipPath != null)
                {
                    var iconPath = valueString.Replace("ms-plugin://", "");
                    var bitmap = new BitmapImage();
                    var imageIcon = new ImageIcon { Source = bitmap };
                    _ = FillBitmapFromZipAsync(bitmap, zipPath, iconPath);
                    return imageIcon;
                }

                return new ImageIcon
                {
                    Source = new BitmapImage(uri)
                };
            case "font":
                glyph = valueString.Replace("font://", "");
                if (glyph.StartsWith('\\')) glyph = Regex.Unescape(glyph);
                return new FontIcon()
                {
                    Glyph = glyph
                };

            case "symbol":
                glyph = valueString.Replace("symbol://", "");
                return new Microsoft.UI.Xaml.Controls.SymbolIcon(
                    Enum.Parse<Microsoft.UI.Xaml.Controls.Symbol>(glyph, ignoreCase: true));
            case "fluent":
                switch (uri.Host)
                {
                    case "regular":
                        glyph = valueString.Replace("fluent://regular/", "");
                        if (glyph.StartsWith('\\'))
                        {
                            glyph = Regex.Unescape(glyph);
                            return new FluentIcon()
                            {
                                IconVariant = IconVariant.Regular,
                                Glyph = glyph
                            };
                        }

                        if (Enum.TryParse(glyph, out Icon regularIcon))
                        {
                            return new FluentIcon()
                            {
                                IconVariant = IconVariant.Regular,
                                Icon = regularIcon
                            };
                        }

                        break;
                    case "filled":
                        glyph = valueString.Replace("fluent://filled/", "");
                        if (glyph.StartsWith('\\'))
                        {
                            glyph = Regex.Unescape(glyph);
                            return new FluentIcon()
                            {
                                IconVariant = IconVariant.Filled,
                                Glyph = glyph,
                            };
                        }

                        if (Enum.TryParse(glyph, out Icon filledIcon))
                        {
                            return new FluentIcon()
                            {
                                IconVariant = IconVariant.Filled,
                                Icon = filledIcon
                            };
                        }

                        break;
                }

                break;
        }

        return null;
    }
}