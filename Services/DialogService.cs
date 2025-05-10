// Services/DialogService.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using System.Threading.Tasks;
using System;

namespace UBB_SE_2025_EUROTRUCKERS.Services;

public class DialogService : IDialogService
{
    private Window _mainWindow;

    public void Initialize(Window mainWindow)
    {
        _mainWindow = mainWindow;
    }

    public async Task ShowErrorDialogAsync(string title, string message)
    {
        try
        {
            // Intentar con la ventana principal primero
            if (TryShowInWindow(_mainWindow, title, message))
                return;

            // Fallback: crear ventana temporal
            await ShowInTemporaryWindow(title, message);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al mostrar diálogo: {ex}");
            // Último recurso: mostrar en consola
            Console.WriteLine($"[ERROR] {title}: {message}");
        }
    }

    private bool TryShowInWindow(Window window, string title, string message)
    {
        if (window?.Content?.XamlRoot == null)
            return false;

        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = window.Content.XamlRoot
        };

        _ = dialog.ShowAsync(); // Usar fire-and-forget con precaución
        return true;
    }

    private async Task ShowInTemporaryWindow(string title, string message)
    {
        var tempWindow = new Window();
        tempWindow.Activate();

        // Asegurarnos de que la ventana tiene contenido
        tempWindow.Content = new Grid();

        // Esperar a que el XamlRoot esté disponible
        await WaitForXamlRoot(tempWindow);

        if (tempWindow.Content?.XamlRoot == null)
            throw new InvalidOperationException("No se pudo inicializar XamlRoot");

        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = tempWindow.Content.XamlRoot
        };

        await dialog.ShowAsync();
        tempWindow.Close();
    }

    private async Task WaitForXamlRoot(Window window, int maxRetries = 5, int delayMs = 100)
    {
        for (int i = 0; i < maxRetries; i++)
        {
            if (window.Content?.XamlRoot != null)
                return;

            await Task.Delay(delayMs);
        }
    }
}