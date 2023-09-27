using ModelRender.Helpers;
using ModelRender.Models;
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
        public static void DrawPolygon(double[][] points, SolidColorBrush color, Canvas gp)
        {
            Polygon polygon = new Polygon
            {
                Fill = color,  // Optional: Set the fill color
                Stroke = Brushes.Black,    // Optional: Set the border color
                StrokeThickness = 1       // Optional: Set the border thickness
            };

            var pointPoints = ArrayHelper.DoubleArrayToPoints(points);

            foreach (var point in pointPoints)
            {
                polygon.Points.Add(point);
            }

            gp.Children.Add(polygon);
        }

        public static void DrawPolygon(Point[] points, SolidColorBrush color, GraphicContextControl gcc )
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

            gcc.MainStage.Children.Add(polygon);
        }

        public static void DrawCircle(double radius, SolidColorBrush color, Canvas gp)
        {

            DrawPolygon(GenerateCirclePoints(radius), color, gp);
        }

        public static double[][] GenerateCirclePoints(double radius, int numPoints = 0)
        {
            if (numPoints == 0)
                numPoints = (int)(radius * 2 * Math.PI); // Reasonable approximation for number of points

            double[,] points = new double[numPoints, 2];

            const double threshold = 1e-15; // Values below this (in absolute terms) are treated as 0.0.

            for (int i = 0; i < numPoints; i++)
            {
                double theta = 2 * Math.PI * i / numPoints;
                double x = radius * Math.Cos(theta);
                double y = -radius * Math.Sin(theta);

                // Correcting tiny values close to zero
                points[i, 0] = Math.Abs(x) < threshold ? 0.0 : x;
                points[i, 1] = Math.Abs(y) < threshold ? 0.0 : y;
            }

            return ArrayHelper.ToJaggedArray(points);
        }

        //public static double[][] GenerateCirclePoints(double radius, int numPoints = 0)
        //{
        //    if (numPoints == 0)
        //        numPoints = (int)(radius * 2 * Math.PI); // Reasonable approximation for number of points

        //    double[,] points = new double[numPoints,2];

        //    // Helper method to round to 3 decimals


        //    for (int i = 0; i < numPoints; i++)
        //    {
        //        double theta = 2 * Math.PI * i / numPoints;
        //        points[i,0] = radius * Math.Cos(theta);
        //        points[i,1] = - radius * Math.Sin(theta);
        //    }

        //    return ArrayHelper.ToJaggedArray(points);
        //}

        //public static Point[] GenerateUnitCirclePoints(int numberOfPoints)
        //{
        //    Point[] points = new Point[numberOfPoints];

        //    double angleIncrement = 2 * Math.PI / numberOfPoints;

        //    for (int i = 0; i < numberOfPoints; i++)
        //    {
        //        double angle = i * angleIncrement;
        //        double x = 10 + Math.Cos(angle);
        //        double y = 10 + Math.Sin(angle);

        //        points[i] = new Point(x, y);
        //    }

        //    var reversed = points.Reverse().ToArray();

        //    return reversed;
        //}


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
