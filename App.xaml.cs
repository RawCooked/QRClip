using System.Windows;
using QRClip.Hotkeys;
using QRClip.Models;
using QRClip.Services;
using QRClip.UI;
using WinForms = System.Windows.Forms;

namespace QRClip;

public partial class App
{
    private WinForms.NotifyIcon _trayIcon = null!;
    private WinForms.ToolStripMenuItem _showQrMenuItem = null!;
    private HotkeyManager _hotkeyManager = null!;
    private PopupWindow _popup = null!;
    private SettingsService _settingsService = null!;
    private ClipboardService _clipboardService = null!;
    private QrGenerator _qrGenerator = null!;
    private Settings _settings = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        _settingsService = new SettingsService();
        _clipboardService = new ClipboardService();
        _qrGenerator = new QrGenerator();
        _settings = _settingsService.Load();
        _popup = new PopupWindow();

        InitializeTray();

        _hotkeyManager = new HotkeyManager();
        _hotkeyManager.HotkeyPressed += OnHotkeyPressed;

        RegisterHotkey();
    }

    private void RegisterHotkey()
    {
        bool registered = _hotkeyManager.Register(_settings.HotkeyModifiers, _settings.HotkeyKey);
        UpdateTrayTooltip();
        if (!registered)
        {
            _trayIcon.ShowBalloonTip(4000, "QRClip",
                "Impossible d'enregistrer le raccourci clavier.\nIl est peut-être utilisé par une autre application.\nChangez-le dans Paramètres.",
                WinForms.ToolTipIcon.Warning);
        }
    }

    private void InitializeTray()
    {
        _trayIcon = new WinForms.NotifyIcon
        {
            Icon = TrayIconBuilder.Create(),
            Text = "QRClip",
            Visible = true
        };

        var menu = new WinForms.ContextMenuStrip();

        var header = (WinForms.ToolStripMenuItem)menu.Items.Add("QRClip");
        header.Enabled = false;
        header.Font = new System.Drawing.Font(header.Font, System.Drawing.FontStyle.Bold);

        menu.Items.Add(new WinForms.ToolStripSeparator());

        _showQrMenuItem = new WinForms.ToolStripMenuItem("Afficher QR");
        _showQrMenuItem.Click += (_, _) => OnHotkeyPressed();
        menu.Items.Add(_showQrMenuItem);

        menu.Items.Add(new WinForms.ToolStripSeparator());
        menu.Items.Add("Paramètres...", null, (_, _) => OpenSettings());
        menu.Items.Add(new WinForms.ToolStripSeparator());
        menu.Items.Add("Quitter", null, (_, _) => Shutdown());

        _trayIcon.ContextMenuStrip = menu;
        _trayIcon.DoubleClick += (_, _) => OnHotkeyPressed();
    }

    private void UpdateTrayTooltip()
    {
        var label = HotkeyLabel(_settings.HotkeyModifiers, _settings.HotkeyKey);
        _trayIcon.Text = $"QRClip — {label}";
        _showQrMenuItem.Text = $"Afficher QR  ({label})";
    }

    private void OnHotkeyPressed()
    {
        var text = _clipboardService.GetText();
        if (string.IsNullOrWhiteSpace(text))
        {
            _trayIcon.ShowBalloonTip(2000, "QRClip", "Presse-papiers vide ou non-texte.", WinForms.ToolTipIcon.Info);
            return;
        }
        var image = _qrGenerator.Generate(text, _settings.QrSize);
        _popup.ShowQr(image, text);
    }

    private void OpenSettings()
    {
        var win = new SettingsWindow(_settingsService);
        win.SettingsSaved += newSettings =>
        {
            _settings = newSettings;
            RegisterHotkey();
        };
        win.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _hotkeyManager?.Dispose();
        _trayIcon?.Dispose();
        base.OnExit(e);
    }

    private static string HotkeyLabel(uint modifiers, uint vk)
    {
        var parts = new List<string>();
        if ((modifiers & 0x0002) != 0) parts.Add("Ctrl");
        if ((modifiers & 0x0004) != 0) parts.Add("Shift");
        if ((modifiers & 0x0001) != 0) parts.Add("Alt");
        if ((modifiers & 0x0008) != 0) parts.Add("Win");
        var key = System.Windows.Input.KeyInterop.KeyFromVirtualKey((int)vk);
        parts.Add(key == System.Windows.Input.Key.Space ? "Espace" : key.ToString());
        return string.Join("+", parts);
    }
}
