using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QRClip.UI;

public static class TrayIconBuilder
{
    public static Icon Create()
    {
        using var bmp = new Bitmap(32, 32);
        using var g = Graphics.FromImage(bmp);
        g.Clear(System.Drawing.Color.Transparent);

        g.FillRectangle(System.Drawing.Brushes.Black, 0, 0, 32, 32);
        g.FillRectangle(System.Drawing.Brushes.White, 2, 2, 28, 28);

        // Top-left finder
        g.FillRectangle(System.Drawing.Brushes.Black, 4, 4, 10, 10);
        g.FillRectangle(System.Drawing.Brushes.White, 6, 6, 6, 6);
        g.FillRectangle(System.Drawing.Brushes.Black, 7, 7, 4, 4);

        // Top-right finder
        g.FillRectangle(System.Drawing.Brushes.Black, 18, 4, 10, 10);
        g.FillRectangle(System.Drawing.Brushes.White, 20, 6, 6, 6);
        g.FillRectangle(System.Drawing.Brushes.Black, 21, 7, 4, 4);

        // Bottom-left finder
        g.FillRectangle(System.Drawing.Brushes.Black, 4, 18, 10, 10);
        g.FillRectangle(System.Drawing.Brushes.White, 6, 20, 6, 6);
        g.FillRectangle(System.Drawing.Brushes.Black, 7, 21, 4, 4);

        // Data modules
        g.FillRectangle(System.Drawing.Brushes.Black, 16, 16, 4, 4);
        g.FillRectangle(System.Drawing.Brushes.Black, 22, 20, 3, 3);
        g.FillRectangle(System.Drawing.Brushes.Black, 20, 24, 3, 3);
        g.FillRectangle(System.Drawing.Brushes.Black, 16, 22, 3, 3);
        g.FillRectangle(System.Drawing.Brushes.Black, 24, 16, 4, 3);

        var hIcon = bmp.GetHicon();
        return Icon.FromHandle(hIcon);
    }

    // Returns a WPF ImageSource suitable for Window.Icon
    public static ImageSource CreateWpfIcon()
    {
        var icon = Create();
        var source = Imaging.CreateBitmapSourceFromHIcon(
            icon.Handle,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());
        source.Freeze();
        return source;
    }
}
