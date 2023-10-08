using ModelRender.Helpers;
using ModelRender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        public int Count { get { return _coordinateToInt.Count; } }

    }

    public class ShapeHelper
    {
        public static void DrawPolygon(List<Point> points, SolidColorBrush color, Canvas gp)
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

        public static float GetDistanceFromOrigin3D(Vector3 point)
        {
            return (float)Math.Sqrt(point.X * point.X + point.Y * point.Y + point.Z * point.Z);
        }

        public float GetDistanceFromOrigin3D(float X, float Y, float Z) => GetDistanceFromOrigin3D(new Vector3( X, Y, Z ));

        public static float GetDistanceFromOrigin2D(Vector3 point)
        {
            //var bar = Math.Pow(point[0], 2);
            //var baz = (point[1] * point[1]);

            //var foo = Math.Sqrt((point[0] * point[0]) + point[1] * point[1]);
            return (float)Math.Sqrt(point.X * point.X + point.Y * point.Y);
        }

        public float GetDistanceFromOrigin2D(float X, float Y) => GetDistanceFromOrigin2D(new Vector3(X, Y, 0 ));

        public static float CalculateDistance2D(float x1, float y1, float x2, float y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            var fo = (float)Math.Sqrt(dx * dx + dy * dy);
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public static float CalculateDistance2D(Vector3 point1, Vector3 point2) => CalculateDistance2D(point1.X, point1.Y, point2.X, point2.Y);


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

        public static void DrawCircle(float radius, SolidColorBrush color, Canvas gp)
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

        public static float[] GenerateSphereRadi(int numPoints)
        {
            var quarterPoints = GenerateQuarterCirclePoints(1, numPoints);
            float[] radi = new float[numPoints];

            for (int i = 0; i < quarterPoints.Count; i++)
            {
                radi[i] = CalculateDistance2D(quarterPoints[i], new Vector3(0, quarterPoints[i].Y, 0));
            }

            return radi;
            
        }

        //public static double[][] GenerateSphere(int numPoints)
        //{
        //    var coordMapper = new CoordinateMapper();
        //    var sphereRadii = GenerateSphereRadi(numPoints);
        //    List<double[][]> circles = new List<double[][]>();

        //    int currentVert = 0;
        //    double[][] vertices = new double[((sphereRadii.Length - 1) * numPoints) * 4][];

        //    int nVertices = ((sphereRadii.Length - 1) * numPoints) * 4;

        //    int currentFace = 0;
        //    int[][] faceVertMap = new int[(sphereRadii.Length - 1) * numPoints][];


        //    for(int i = 0;  i < sphereRadii.Length; i++)
        //    {
        //        circles.Add(GenerateCirclePoints3D(sphereRadii[i], numPoints));
        //    }
        //    List<double[][]> faces = new List<double[][]>();
        //    for (int i = 0; i < circles.Count - 1; i++)
        //    {

        //        for(int j = 0; j< numPoints; j++)
        //        {
        //            double[][] face = new double[4][];
        //            int[] faceVertices = new int[4];




        //            face[0] = circles[i][j];
        //            faceVertices[0] = coordMapper.GetOrAdd(i, j);

        //            face[1] = circles[(i + 1)][j];
        //            faceVertices[1] = coordMapper.GetOrAdd((i + 1), j);

        //            face[2] = circles[(i + 1)][(j + 1) % numPoints];
        //            faceVertices[2] = coordMapper.GetOrAdd(((i + 1)), ((j + 1) % numPoints));

        //            face[3] = circles[i][(j + 1) % numPoints];
        //            faceVertices[3] = coordMapper.GetOrAdd(i, ((j + 1) % numPoints));

        //            vertices[currentVert++] = circles[i][j];
        //            vertices[currentVert++] = circles[(i + 1)][j];
        //            vertices[currentVert++] = circles[(i + 1)][(j + 1) % numPoints];
        //            vertices[currentVert++] = circles[i][(j + 1) % numPoints];



        //            faceVertMap[currentFace++] = faceVertices;
        //            faces.Add(face);
        //        }
        //    }

        //    var exportModel = new ExportModel()
        //    {
        //        vertices = vertices,
        //        edges = new int[][] { new int[] { 0, 0 } },
        //        faces = faceVertMap,
        //        nVertices = nVertices,
        //        nEdges = 1,
        //        nFaces = faceVertMap.Length,
        //        color = new int[] { 255, 0, 128 }
        //    };

        //    Model.ExportModel("C:\\ModelExports\\sphereplz.json", exportModel);

        //    //double[numpoints][] spherePoints 

        //    return new double[3][];

        //}

        public static List<Point> GenerateCirclePoints(float radius, int numPoints = 0)
        {
            if (numPoints == 0)
                numPoints = (int)(radius * 2 * Math.PI); // Reasonable approximation for number of points

            List<Point> points = new List<Point>();

            const double threshold = 1e-15; // Values below this (in absolute terms) are treated as 0.0.

            for (int i = 0; i < numPoints; i++)
            {
                float theta =  (float)(2 * Math.PI * i / numPoints);
                float x = (float)(radius * Math.Cos(theta));
                float y = (float)(-radius * Math.Sin(theta));

                float pointX = Math.Abs(x) < threshold ? (float)0.0 : x;
                float pointY = Math.Abs(y) < threshold ? (float)0.0 : y;

                // Correcting tiny values close to zero
                points.Add(new Point(pointX, pointY));
            }

            return points;
        }


        public static List<Vector3> GenerateCirclePoints3D(float radius, int numPoints = 0)
        {
            if (numPoints == 0)
                numPoints = (int)(radius * 2 * Math.PI); // Reasonable approximation for number of points

            List<Vector3> points = new List<Vector3>();

            float increaseAmmount = (float)1.0 / 16;
            float currentZ = 0; 

            const double threshold = 1e-15; // Values below this (in absolute terms) are treated as 0.0.

            for (int i = 0; i < numPoints; i++)
            {
                float theta = (float)(2 * Math.PI * i / numPoints);
                float x = (float)(radius * Math.Cos(theta));
                float y = (float)(-radius * Math.Sin(theta));


                float pointX = Math.Abs(x) < threshold ? (float)0.0 : x;
                float pointY = Math.Abs(y) < threshold ? (float)0.0 : y;
                float pointZ = (currentZ += increaseAmmount);



                // Correcting tiny values close to zero
                points.Add(new Vector3(pointX, pointY, pointZ));
            }

            return points;
        }




        public static List<Vector3> GenerateQuarterCirclePoints(float radius, int numPoints)
        {
            if (numPoints == 0)
                numPoints = (int)(radius * 2 * Math.PI); // Reasonable approximation for number of points

            List<Vector3> points = new List<Vector3>();

            //const float threshold = 1e-15f; // Values below this (in absolute terms) are treated as 0.0.

            for (int i = 0; i < numPoints; i++)
            {
                float theta = (float)((Math.PI / 2) * i / numPoints);
                float x = (float)(radius * Math.Cos(theta));
                float y = (float)(-radius * Math.Sin(theta));

                // Correcting tiny values close to zero
                //x = Math.Abs(x) < threshold ? 0.0f : x;
                //y = Math.Abs(y) < threshold ? 0.0f : y;

                points.Add(new Vector3(x, y, 0));
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
