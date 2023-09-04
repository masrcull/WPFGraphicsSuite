using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsCommon
{
    public class ShapeHelper
    {
        public static void DrawPolygon(Point[] points, SolidColorBrush color, Canvas gp)
        {
            Polygon polygon = new Polygon
            {
                Fill = color,  // Optional: Set the fill color
                Stroke = Brushes.Black,    // Optional: Set the border color
                StrokeThickness = 1       // Optional: Set the border thickness
            };

            foreach (var point in points)
            {
                polygon.Points.Add(point);
            }

            gp.Children.Add(polygon);
        }

        public static Ellipse CreateEllipse(double width, double height, double xPosition, double yPosition, SolidColorBrush fillColor)
        {
            var ellipse = new Ellipse();

            ellipse.Width = width;   // example width
            ellipse.Height = height;  // example height
            ellipse.Fill = fillColor; // example fill color

            // Set its position on the Canvas
            Canvas.SetLeft(ellipse, xPosition); // X position
            Canvas.SetTop(ellipse, yPosition);  // Y position

            return ellipse;
        }

        public static Ellipse CreateCircle(double diameter, double xPosition, double yPosition, SolidColorBrush fillColor)
        {
            return CreateEllipse(diameter, diameter, xPosition, yPosition, fillColor);
        }

    }
}
