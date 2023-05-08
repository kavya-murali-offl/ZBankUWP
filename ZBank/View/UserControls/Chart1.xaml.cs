using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.UserControls
{
    public sealed partial class Chart1 : UserControl
    {
        public Chart1()
        {
            this.InitializeComponent();
            DrawPieChart();
        }

        private void DrawPieChart()
        {


            int sliceCount = 4;
            double centerX = PieChartCanvas.Width / 2;
            double centerY = PieChartCanvas.Height / 2;
            double radius = Math.Min(centerX, centerY) - 10;
            double sliceAngle = 360.0 / sliceCount;

            for (int i = 0; i < sliceCount; i++)
            {
                Ellipse slice = new Ellipse();
                slice.Width = radius * 2;
                slice.Height = radius * 2;
                slice.Stroke = new SolidColorBrush(Colors.White);
                slice.StrokeThickness = 2;
                slice.Fill = new SolidColorBrush(Colors.Blue);

                // Calculate the position of the slice
                double sliceStartAngle = i * sliceAngle;
                double sliceEndAngle = sliceStartAngle + sliceAngle;
                double sliceStartX = centerX + radius * Math.Sin(sliceStartAngle * Math.PI / 180);
                double sliceStartY = centerY - radius * Math.Cos(sliceStartAngle * Math.PI / 180);
                double sliceEndX = centerX + radius * Math.Sin(sliceEndAngle * Math.PI / 180);
                double sliceEndY = centerY - radius * Math.Cos(sliceEndAngle * Math.PI / 180);

                // Set the position of the slice
                Canvas.SetLeft(slice, sliceStartX);
                Canvas.SetTop(slice, sliceStartY);

                // Add the slice to the canvas
                PieChartCanvas.Children.Add(slice);
            }

        }

    }
}
