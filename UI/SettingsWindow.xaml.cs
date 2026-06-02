using System.Windows;
using System.Windows.Input;
using QRClip.Models;
using QRClip.Services;

namespace QRClip.UI;

public partial class SettingsWindow : Window
{
    private readonly SettingsService _settingsService;
    private Settings _current;

    private uint _capturedModifiers;
    private uint _capturedKey;
    private bool _isCapturing;

    public event Action<Settings>? SettingsSaved;

    public SettingsWindow(SettingsService settingsService)
    {
        InitializeComponent();
        Icon = TrayIconBuilder.CreateWpfIcon();
        _settingsService = settingsService;
        _current = _settingsService.Load();

        LoadCurrent();

        KeyDown += OnWindowKeyDown;
        PreviewKeyDown += OnPreviewKeyDown;
    }

    private void LoadCurrent()
    {
        _capturedModifiers = _current.HotkeyModifiers;
        _capturedKey = _current.HotkeyKey;

        ChkCtrl.IsChecked = (_current.HotkeyModifiers & 0x0002) != 0;
        ChkShift.IsChecked = (_current.HotkeyModifiers & 0x0004) != 0;
        ChkAlt.IsChecked = (_current.HotkeyModifiers & 0x0001) != 0;
        ChkWin.IsChecked = (_current.HotkeyModifiers & 0x0008) != 0;

        HotkeyDisplay.Text = KeyDisplayName(_current.HotkeyKey);
        ChkAutoStart.IsChecked = AutoStartService.IsEnabled();
    }

    private void BtnCapture_Click(object sender, RoutedEventArgs e)
    {
        _isCapturing = true;
        BtnCapture.Content = "En attente...";
        HotkeyStatus.Text = "Appuyez sur la combinaison souhaitée (ex. Ctrl+Shift+Espace)...";
        HotkeyStatus.Foreground = System.Windows.Media.Brushes.DodgerBlue;
        BtnCapture.IsEnabled = false;
        Focus();
    }

    private void OnPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (!_isCapturing) return;

        // Ignore pure modifier keys — wait for an actual key
        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        if (key is Key.LeftCtrl or Key.RightCtrl or Key.LeftShift or Key.RightShift
            or Key.LeftAlt or Key.RightAlt or Key.LWin or Key.RWin)
            return;

        uint modifiers = 0;
        if ((Keyboard.Modifiers & ModifierKeys.Control) != 0) modifiers |= 0x0002;
        if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0) modifiers |= 0x0004;
        if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0) modifiers |= 0x0001;
        if ((Keyboard.Modifiers & ModifierKeys.Windows) != 0) modifiers |= 0x0008;

        if (modifiers == 0)
        {
            HotkeyStatus.Text = "Au moins un modificateur (Ctrl, Shift, Alt) est requis.";
            HotkeyStatus.Foreground = System.Windows.Media.Brushes.Red;
            e.Handled = true;
            return;
        }

        _capturedModifiers = modifiers;
        _capturedKey = (uint)KeyInterop.VirtualKeyFromKey(key);

        ChkCtrl.IsChecked = (modifiers & 0x0002) != 0;
        ChkShift.IsChecked = (modifiers & 0x0004) != 0;
        ChkAlt.IsChecked = (modifiers & 0x0001) != 0;
        ChkWin.IsChecked = (modifiers & 0x0008) != 0;

        HotkeyDisplay.Text = KeyDisplayName(_capturedKey);
        HotkeyStatus.Text = $"Raccourci capturé : {BuildHotkeyLabel(modifiers, _capturedKey)}";
        HotkeyStatus.Foreground = System.Windows.Media.Brushes.Green;

        StopCapture();
        e.Handled = true;
    }

    private void OnWindowKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (_isCapturing && e.Key == Key.Escape)
        {
            StopCapture();
            HotkeyStatus.Text = "Capture annulée.";
            HotkeyStatus.Foreground = System.Windows.Media.Brushes.Gray;
            e.Handled = true;
        }
    }

    private void StopCapture()
    {
        _isCapturing = false;
        BtnCapture.Content = "Capturer";
        BtnCapture.IsEnabled = true;
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        var settings = new Settings
        {
            HotkeyModifiers = _capturedModifiers,
            HotkeyKey = _capturedKey,
            QrSize = _current.QrSize
        };

        _settingsService.Save(settings);

        if (ChkAutoStart.IsChecked == true)
            AutoStartService.Enable();
        else
            AutoStartService.Disable();

        SettingsSaved?.Invoke(settings);
        Close();
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();

    private static string KeyDisplayName(uint vk)
    {
        var key = KeyInterop.KeyFromVirtualKey((int)vk);
        return key switch
        {
            Key.Space => "Espace",
            Key.Return => "Entrée",
            Key.Tab => "Tab",
            Key.Back => "Retour arrière",
            Key.Delete => "Suppr",
            Key.Insert => "Inser",
            Key.Home => "Début",
            Key.End => "Fin",
            Key.PageUp => "Pg.Préc",
            Key.PageDown => "Pg.Suiv",
            Key.Up => "↑",
            Key.Down => "↓",
            Key.Left => "←",
            Key.Right => "→",
            _ => key.ToString()
        };
    }

    private static string BuildHotkeyLabel(uint modifiers, uint vk)
    {
        var parts = new List<string>();
        if ((modifiers & 0x0002) != 0) parts.Add("Ctrl");
        if ((modifiers & 0x0004) != 0) parts.Add("Shift");
        if ((modifiers & 0x0001) != 0) parts.Add("Alt");
        if ((modifiers & 0x0008) != 0) parts.Add("Win");
        parts.Add(KeyDisplayName(vk));
        return string.Join("+", parts);
    }
}
