using System.IO;
using System.Text.Json;
using QRClip.Models;

namespace QRClip.Services;

public class SettingsService
{
    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "QRClip", "settings.json");

    private Settings? _cached;

    public Settings Load()
    {
        if (_cached != null) return _cached;
        try
        {
            if (File.Exists(SettingsPath))
            {
                var json = File.ReadAllText(SettingsPath);
                _cached = JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
                return _cached;
            }
        }
        catch { }
        _cached = new Settings();
        return _cached;
    }

    public void Save(Settings settings)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsPath, json);
            _cached = settings;
        }
        catch { }
    }
}
