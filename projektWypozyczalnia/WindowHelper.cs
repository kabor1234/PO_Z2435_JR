namespace projektWypozyczalnia;
using System;
using System.Media;
using System.Windows;

public class WindowHelper: Window
{
    private bool _isAccepted = false;
    private bool _isClosing = false;

    private  void Window_Deactivated(object sender, EventArgs e)
    {
        if (_isClosing) return;
        if (_isAccepted) return;

        if (sender is Window window)
        {
            SystemSounds.Beep.Play();
            Activate();
        }

    }
}