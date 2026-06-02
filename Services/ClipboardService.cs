using System.Runtime.InteropServices;

namespace QRClip.Services;

public class ClipboardService
{
    public string? GetText()
    {
        for (int i = 0; i < 3; i++)
        {
            try
            {
                if (System.Windows.Clipboard.ContainsText())
                    return System.Windows.Clipboard.GetText();
                return null;
            }
            catch (ExternalException)
            {
                Thread.Sleep(50);
            }
        }
        return null;
    }
}
