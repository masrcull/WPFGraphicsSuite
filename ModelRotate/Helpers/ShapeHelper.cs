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

        public static void DrawCircle(double radius, SolidColorBrush color, Canvas gp)
        {
            DrawPolygon(GenerateCirclePoints(radius), color, gp);
        }

        public static Point[] GenerateCirclePoints(double radius)
        {
            int numPoints = (int)(radius * 2 * Math.PI); // Reasonable approximation for number of points
            Point[] points = new Point[numPoints];

            for (int i = 0; i < numPoints; i++)
            {
                double theta = 2 * Math.PI * i / numPoints;
                points[i] = new Point(radius * Math.Cos(theta), -radius * Math.Sin(theta));
            }

            return points;
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
