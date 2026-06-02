using System.IO;
using System.Windows.Media.Imaging;
using QRCoder;

namespace QRClip.Services;

public class QrGenerator
{
    public BitmapImage Generate(string text, int pixelsPerModule = 5)
    {
        using var generator = new QRCodeGenerator();
        var data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(data);
        var pngBytes = qrCode.GetGraphic(pixelsPerModule);

        var image = new BitmapImage();
        using var ms = new MemoryStream(pngBytes);
        image.BeginInit();
        image.StreamSource = ms;
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.EndInit();
        image.Freeze();
        return image;
    }
}
