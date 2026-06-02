using System.Windows.Interop;

namespace QRClip.Hotkeys;

public class HotkeyManager : IDisposable
{
    private const int HOTKEY_ID = 9001;
    private HwndSource? _source;
    private bool _registered;

    public event Action? HotkeyPressed;

    public bool Register(uint modifiers, uint key)
    {
        EnsureSource();
        if (_registered)
        {
            NativeMethods.UnregisterHotKey(_source!.Handle, HOTKEY_ID);
            _registered = false;
        }
        _registered = NativeMethods.RegisterHotKey(_source!.Handle, HOTKEY_ID, modifiers, key);
        return _registered;
    }

    private void EnsureSource()
    {
        if (_source != null) return;
        var parameters = new HwndSourceParameters("QRClipHotkey")
        {
            WindowStyle = 0,
            ExtendedWindowStyle = 0,
            PositionX = 0,
            PositionY = 0,
            Width = 0,
            Height = 0,
            ParentWindow = new IntPtr(-3) // HWND_MESSAGE
        };
        _source = new HwndSource(parameters);
        _source.AddHook(WndProc);
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == NativeMethods.WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
        {
            HotkeyPressed?.Invoke();
            handled = true;
        }
        return IntPtr.Zero;
    }

    public void Dispose()
    {
        if (_source != null && _registered)
        {
            NativeMethods.UnregisterHotKey(_source.Handle, HOTKEY_ID);
            _registered = false;
        }
        _source?.Dispose();
        _source = null;
    }
}
