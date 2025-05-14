using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using UBB_SE_2025_EUROTRUCKERS.ViewModels;
using Windows.Graphics;


namespace UBB_SE_2025_EUROTRUCKERS.Views
{
    public sealed partial class MapView : Window
    {
        public MapViewModel ViewModel { get; }

        public MapView()
        {
            this.InitializeComponent();
            setSize();
            ViewModel = App.Services.GetRequiredService<MapViewModel>();
            GraphCanvas.DataContext = ViewModel;

            GraphCanvas.Loaded += MapView_Loaded;
        }

        private async void MapView_Loaded(object sender, RoutedEventArgs e)
        {
            Canvas canvas = new Canvas
            {
                Width = 600,
                Height = 600,
                Background = new SolidColorBrush(Microsoft.UI.Colors.LightGray)
            };

            List<(float x, float y, string name)> coordinates = ViewModel.CityCoordinates.ToList();
            DrawCircles(canvas, coordinates);


            List<((float x, float y) start, (float x, float y) end)> roadCoords = ViewModel.RoadCoordinates.ToList();
            DrawLines(canvas, new SolidColorBrush(Microsoft.UI.Colors.Black), roadCoords);


        }

        private void DrawCircles(Canvas canvas, List<(float x, float y, string name)> coordinates)
        {
            foreach (var (x, y, name) in coordinates)
            {
                Ellipse circle = new Ellipse
                {
                    Width = 50,
                    Height = 50,
                    Fill = new SolidColorBrush(Microsoft.UI.Colors.Red)
                };
                Canvas.SetLeft(circle, x - 25); // Center the circle based on its radius
                Canvas.SetTop(circle, y - 25);

                TextBlock label = new TextBlock
                {
                    Text = name,
                    Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black),
                    FontSize = 14
                };
                Canvas.SetLeft(label, x - 10);
                Canvas.SetTop(label, y + 30);

                canvas.Children.Add(circle);
                canvas.Children.Add(label);
            }

            this.Content = canvas; // Set the canvas as the content of the window
        }
        private void DrawLine(Canvas canvas, Brush brush, List<(float x, float y)> points)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                var start = points[i];
                var end = points[i + 1];

                Line line = new Line
                {
                    X1 = start.x,
                    Y1 = start.y,
                    X2 = end.x,
                    Y2 = end.y,
                    Stroke = brush,
                    StrokeThickness = 2
                };

                canvas.Children.Add(line);
            }
        }
        private void DrawLines(Canvas canvas, Brush brush, List<((float x, float y) start, (float x, float y) end)> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                var start = lines[i].start;
                var end = lines[i].end;

                Line line = new Line
                {
                    X1 = start.x,
                    Y1 = start.y,
                    X2 = end.x,
                    Y2 = end.y,
                    Stroke = brush,
                    StrokeThickness = 2
                };

                canvas.Children.Add(line);
            }
        }
        private void setSize()
        {
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new SizeInt32(900, 900));
        }
    }
}
