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
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GraphicsCommon
{

    public class CoordinateMapper
    {
        private readonly Dictionary<Tuple<int, int>, int> _coordinateToInt = new Dictionary<Tuple<int, int>, int>();
        private int _nextInt = 0;

        public int GetOrAdd(int x, int y)
        {
            var tuple = new Tuple<int, int>(x, y);

            if (_coordinateToInt.TryGetValue(tuple, out int existingValue))
            {
                return existingValue;
            }
            else
            {
                _coordinateToInt[tuple] = _nextInt;
                return _nextInt++;
            }
        }
    }

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

        public static double GetDistanceFromOrigin3D(double[] point)
        {
            return Math.Sqrt(point[0] * point[0] + point[1] * point[1] + point[2] * point[2]);
        }

        public double GetDistanceFromOrigin3D(double X, double Y, double Z) => GetDistanceFromOrigin3D(new double[] { X, Y, Z });

        public static double GetDistanceFromOrigin2D(double[] point)
        {
            var bar = Math.Pow(point[0], 2);
            var baz = (point[1] * point[1]);

            var foo = Math.Sqrt((point[0] * point[0]) + point[1] * point[1]);
            return Math.Sqrt(point[0] * point[0] + point[1] * point[1]);
        }

        public double GetDistanceFromOrigin2D(double X, double Y) => GetDistanceFromOrigin2D(new double[] { X, Y });

        public static double CalculateDistance(double x1, double y1, double x2, double y2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double CalculateDistance(double[] point1, double[] point2) => CalculateDistance(point1[0], point1[1], point2[0], point2[1]);


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

        //public static double[][] GenerateSpherePoints(int divisions)
        //{
        //    double increment = .5 / divisions;
        //    //double[,] points = new double[,3];


        //    for(int i = 0; i<.5; i++)
        //    {

        //    }



        //    return new double[3][] {  6.0,0,0 };
        //}

        public static double[] GenerateSphereRadi(int numPoints)
        {
            var quarterPoints = GenerateQuarterCirclePoints(1, numPoints);
            double[] radi = new double[numPoints];

            for (int i = 0; i < quarterPoints.Length; i++)
            {
                radi[i] = CalculateDistance(quarterPoints[i], new double[] { 0, quarterPoints[i ][1]}  );
            }

            return radi;
            
        }

        public static double[][] GenerateSphere(int numPoints)
        {
            var coordMapper = new CoordinateMapper();
            var sphereRadii = GenerateSphereRadi(numPoints);
            List<double[][]> circles = new List<double[][]>();

            int currentVert = 0;
            double[][] vertices = new double[((sphereRadii.Length - 1) * numPoints) * 4][];

            int nVertices = ((sphereRadii.Length - 1) * numPoints) * 4;

            int currentFace = 0;
            int[][] faceVertMap = new int[(sphereRadii.Length - 1) * numPoints][];


            for(int i = 0;  i < sphereRadii.Length; i++)
            {
                circles.Add(GenerateCirclePoints3D(sphereRadii[i], numPoints));
            }
            List<double[][]> faces = new List<double[][]>();
            for (int i = 0; i < circles.Count - 1; i++)
            {

                for(int j = 0; j< numPoints; j++)
                {
                    double[][] face = new double[4][];
                    int[] faceVertices = new int[4];




                    face[0] = circles[i][j];
                    faceVertices[0] = coordMapper.GetOrAdd(i, j);

                    face[1] = circles[(i + 1)][j];
                    faceVertices[1] = coordMapper.GetOrAdd((i + 1), j);

                    face[2] = circles[(i + 1)][(j + 1) % numPoints];
                    faceVertices[2] = coordMapper.GetOrAdd(((i + 1)), ((j + 1) % numPoints));

                    face[3] = circles[i][(j + 1) % numPoints];
                    faceVertices[3] = coordMapper.GetOrAdd(i, ((j + 1) % numPoints));

                    vertices[currentVert++] = circles[i][j];
                    vertices[currentVert++] = circles[(i + 1)][j];
                    vertices[currentVert++] = circles[(i + 1)][(j + 1) % numPoints];
                    vertices[currentVert++] = circles[i][(j + 1) % numPoints];



                    faceVertMap[currentFace++] = faceVertices;
                    faces.Add(face);
                }
            }

            var exportModel = new ExportModel()
            {
                vertices = vertices,
                edges = new int[][] { new int[] { 0, 0 } },
                faces = faceVertMap,
                nVertices = nVertices,
                nEdges = 1,
                nFaces = faceVertMap.Length,
                color = new int[] { 255, 0, 128 }
            };

            Model.ExportModel("C:\\ModelExports\\sphereplz.json", exportModel);

            //double[numpoints][] spherePoints 

            return new double[3][];

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


        public static double[][] GenerateCirclePoints3D(double radius, int numPoints = 0)
        {
            if (numPoints == 0)
                numPoints = (int)(radius * 2 * Math.PI); // Reasonable approximation for number of points

            double[,] points = new double[numPoints, 3];

            var increaseAmmount = 1.0 / 16;
            double currentZ = 0; 

            const double threshold = 1e-15; // Values below this (in absolute terms) are treated as 0.0.

            for (int i = 0; i < numPoints; i++)
            {
                double theta = 2 * Math.PI * i / numPoints;
                double x = radius * Math.Cos(theta);
                double y = -radius * Math.Sin(theta);

                // Correcting tiny values close to zero
                points[i, 0] = Math.Abs(x) < threshold ? 0.0 : x;
                points[i, 1] = Math.Abs(y) < threshold ? 0.0 : y;
                points[i, 2] = currentZ += increaseAmmount;
            }

            return ArrayHelper.ToJaggedArray(points);
        }




        public static double[][] GenerateQuarterCirclePoints(double radius, int numPoints)
        {
            if (numPoints == 0)
                numPoints = (int)(radius * 2 * Math.PI); // Reasonable approximation for number of points

            double[,] points = new double[numPoints, 3];

            const double threshold = 1e-15; // Values below this (in absolute terms) are treated as 0.0.

            for (int i = 0; i < numPoints; i++)
            {
                double theta =  (Math.PI/2) * i / numPoints;
                double x = radius * Math.Cos(theta);
                double y = -radius * Math.Sin(theta);

                // Correcting tiny values close to zero
                points[i, 0] = Math.Abs(x) < threshold ? 0.0 : x;
                points[i, 1] = Math.Abs(y) < threshold ? 0.0 : y;
                points[i, 2] = 0;
            }

            return ArrayHelper.ToJaggedArray(points);
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
