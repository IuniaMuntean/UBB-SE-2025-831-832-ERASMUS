using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using UBB_SE_2025_EUROTRUCKERS.Data;
using UBB_SE_2025_EUROTRUCKERS.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UBB_SE_2025_EUROTRUCKERS.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FullMapView : Window
    {
        private MapService graphService;
        private TransportDbContext transportDbContext;

        public FullMapView()
        {
            this.InitializeComponent();
            setSize();
            //transportDbContext = new TransportDbContext();
            graphService = new MapService(transportDbContext);

            Canvas canvas = new Canvas
            {
                Width = 600,
                Height = 600,
                Background = new SolidColorBrush(Microsoft.UI.Colors.LightGray)
            };

            DrawCircles(canvas, graphService.Graph.Cities().Select(city => (city.x, city.y, city.name)).ToList());

            //var path = Fabi__Path_Finding.Path(graphService.Graph, startCityID, endCityID);
            DrawLines(canvas, new SolidColorBrush(Microsoft.UI.Colors.Black), graphService.Graph.Roads().Select(road => ((graphService.Graph.City(road.startCityID).x, graphService.Graph.City(road.startCityID).y), (graphService.Graph.City(road.endCityID).x, graphService.Graph.City(road.endCityID).y))).ToList());
            //DrawLine(canvas, new SolidColorBrush(Microsoft.UI.Colors.Yellow), path.Select(id => (graphService.Graph.City(id).x, graphService.Graph.City(id).y)).ToList());
        }

        private void setSize()
        {
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new SizeInt32(1000, 900));
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
    }
}
