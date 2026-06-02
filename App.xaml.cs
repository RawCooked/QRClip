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

        bool registered = _hotkeyManager.Register(_settings.HotkeyModifiers, _settings.HotkeyKey);
        if (!registered)
        {
            _trayIcon.ShowBalloonTip(4000, "QRClip",
                "Impossible d'enregistrer le raccourci clavier (Ctrl+Shift+Espace).\nIl est peut-être utilisé par une autre application.",
                WinForms.ToolTipIcon.Warning);
        }
    }

    private void InitializeTray()
    {
        _trayIcon = new WinForms.NotifyIcon
        {
            Icon = TrayIconBuilder.Create(),
            Text = "QRClip — Ctrl+Shift+Espace",
            Visible = true
        };

        var menu = new WinForms.ContextMenuStrip();
        var header = menu.Items.Add("QRClip");
        header.Enabled = false;
        header.Font = new System.Drawing.Font(header.Font, System.Drawing.FontStyle.Bold);
        menu.Items.Add(new WinForms.ToolStripSeparator());
        menu.Items.Add("Afficher QR (Ctrl+Shift+Espace)", null, (_, _) => OnHotkeyPressed());
        menu.Items.Add(new WinForms.ToolStripSeparator());
        menu.Items.Add("Quitter", null, (_, _) => Shutdown());

        _trayIcon.ContextMenuStrip = menu;
        _trayIcon.DoubleClick += (_, _) => OnHotkeyPressed();
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

    protected override void OnExit(ExitEventArgs e)
    {
        _hotkeyManager?.Dispose();
        _trayIcon?.Dispose();
        base.OnExit(e);
    }
}
