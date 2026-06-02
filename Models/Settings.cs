namespace QRClip.Models;

public class Settings
{
    // Default: Ctrl+Shift+Space
    public uint HotkeyModifiers { get; set; } = NativeMethods.MOD_CONTROL | NativeMethods.MOD_SHIFT;
    public uint HotkeyKey { get; set; } = NativeMethods.VK_SPACE;
    public int QrSize { get; set; } = 5; // pixels per module

    private static class NativeMethods
    {
        public const uint MOD_CONTROL = 0x0002;
        public const uint MOD_SHIFT = 0x0004;
        public const uint VK_SPACE = 0x20;
    }
}
