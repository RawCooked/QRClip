using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace QRClip.UI;

public partial class PopupWindow : Window
{
    public PopupWindow()
    {
        InitializeComponent();
        Deactivated += (_, _) => Hide();
        KeyDown += OnKeyDown;
    }

    public void ShowQr(BitmapImage image, string text)
    {
        QrImage.Source = image;
        UrlText.Text = text;

        CenterOnActiveScreen();

        if (!IsVisible)
            Show();

        // Re-assert topmost in case another app stole focus
        Topmost = false;
        Topmost = true;
        Activate();
        Focus();
    }

    private void CenterOnActiveScreen()
    {
        var cursorPos = System.Windows.Forms.Cursor.Position;
        var screen = System.Windows.Forms.Screen.FromPoint(cursorPos);

        double dpiScale;
        using (var g = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
            dpiScale = g.DpiX / 96.0;

        double screenLeft = screen.WorkingArea.Left / dpiScale;
        double screenTop = screen.WorkingArea.Top / dpiScale;
        double screenWidth = screen.WorkingArea.Width / dpiScale;
        double screenHeight = screen.WorkingArea.Height / dpiScale;

        double winW = ActualWidth > 0 ? ActualWidth : 360;
        double winH = ActualHeight > 0 ? ActualHeight : 440;

        Left = screenLeft + (screenWidth - winW) / 2;
        Top = screenTop + (screenHeight - winH) / 2;
    }

    private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Escape)
        {
            Hide();
            e.Handled = true;
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e) => Hide();

    protected override void OnClosing(CancelEventArgs e)
    {
        // Never close — only hide (so it stays instant on re-open)
        e.Cancel = true;
        Hide();
    }
}
