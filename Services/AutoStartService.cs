using Microsoft.Win32;

namespace QRClip.Services;

public static class AutoStartService
{
    private const string RegKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    private const string AppName = "QRClip";

    public static bool IsEnabled()
    {
        using var key = Registry.CurrentUser.OpenSubKey(RegKey, false);
        return key?.GetValue(AppName) != null;
    }

    public static void Enable()
    {
        using var key = Registry.CurrentUser.OpenSubKey(RegKey, true);
        key?.SetValue(AppName, $"\"{ExePath}\"");
    }

    public static void Disable()
    {
        using var key = Registry.CurrentUser.OpenSubKey(RegKey, true);
        key?.DeleteValue(AppName, throwOnMissingValue: false);
    }

    private static string ExePath =>
        System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName
        ?? System.Reflection.Assembly.GetExecutingAssembly().Location;
}
