using System.Drawing;

namespace QRClip.UI;

public static class TrayIconBuilder
{
    public static Icon Create()
    {
        using var bmp = new Bitmap(32, 32);
        using var g = Graphics.FromImage(bmp);
        g.Clear(Color.Transparent);

        // Outer border
        g.FillRectangle(Brushes.Black, 0, 0, 32, 32);
        g.FillRectangle(Brushes.White, 2, 2, 28, 28);

        // Top-left finder pattern
        g.FillRectangle(Brushes.Black, 4, 4, 10, 10);
        g.FillRectangle(Brushes.White, 6, 6, 6, 6);
        g.FillRectangle(Brushes.Black, 7, 7, 4, 4);

        // Top-right finder pattern
        g.FillRectangle(Brushes.Black, 18, 4, 10, 10);
        g.FillRectangle(Brushes.White, 20, 6, 6, 6);
        g.FillRectangle(Brushes.Black, 21, 7, 4, 4);

        // Bottom-left finder pattern
        g.FillRectangle(Brushes.Black, 4, 18, 10, 10);
        g.FillRectangle(Brushes.White, 6, 20, 6, 6);
        g.FillRectangle(Brushes.Black, 7, 21, 4, 4);

        // Timing and data dots (center area)
        g.FillRectangle(Brushes.Black, 16, 16, 4, 4);
        g.FillRectangle(Brushes.Black, 22, 20, 3, 3);
        g.FillRectangle(Brushes.Black, 20, 24, 3, 3);

        var hIcon = bmp.GetHicon();
        return Icon.FromHandle(hIcon);
    }
}
