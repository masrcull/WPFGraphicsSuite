using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using GraphicsCommon;


namespace ModelRotate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Canvas gp = null;
        bool ready = false;

        // Flag for allowing sliders, etc., to influence display. 
        public MainWindow()
        {
            InitializeComponent();
            //InitializeCommands();
            // Now add some graphical items in the main Canvas, whose name is "GraphPaper"
            gp = this.FindName("Paper") as Canvas;


            var cubeModel = ModelData.ReadCoordinatesFromJson("..\\..\\..\\Models\\cube_model.json");

            double[] Eye = new double[3]
            { 0, 0, 0 };

            bool redIncrease = true;
            bool greenIncrease = true;
            bool blueIncrease = true;

            byte red = 64;
            byte green = 128;
            byte blue = 200;

            cubeModel.Vertices = ScaleModel(cubeModel.Vertices, new double[] {17, 17, 17 });

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16);  // roughly 60 FPS
            timer.Tick += (s, e) =>
            {

                //if (blueIncrease)
                //{

                //    blue += 16;
                //    if(blue == 255 || blue == 0)
                //    {
                //        blue = 255;
                //        blueIncrease = false;
                //    }
                //}
                //else
                //{
                //    blue-=16;
                //    if(blue == 255)
                //    {
                //        blue = 0;
                //        blueIncrease = true;
                //    }
                //}

                byte redIncrement = 4;
                byte greenIncrement = 8;
                byte blueIncrement = 16;

                blue = (byte)(blue + (blueIncrease ? (blue < 240 ? blueIncrement : ((blueIncrease = false) ? -blueIncrement : 0)) : (blue > 15 ? -blueIncrement : ((blueIncrease = true) ? blueIncrement : 0))));
                red = (byte)(red + (redIncrease ? (red < 240 ? redIncrement : ((redIncrease = false) ? -redIncrement : 0)) : (red > 15 ? -redIncrement : ((redIncrease = true) ? redIncrement : 0))));
                green = (byte)(green + (greenIncrease ? (red < 240 ? greenIncrement : ((greenIncrease = false) ? -greenIncrement : 0)) : (green > 15 ? -greenIncrement : ((greenIncrease = true) ? greenIncrement : 0))));



                //Console.WriteLine("Red: " + red + "   Blue: " + blue + "    Green: " + green);




                gp.Children.Clear();
                cubeModel.Vertices = RotateModelAroundY(cubeModel.Vertices);
                cubeModel.Vertices = RotateModelAroundZ(cubeModel.Vertices);



                DrawFaces(gp, cubeModel, Eye, CreateColorBrush(red, green, blue));
            };
            timer.Start();

            ready = true; // Now we're ready to have sliders and buttons influence the display.
            var one = 1;
        }

        public void initMainLoop()
        {

        }

        public void DrawFaces(Canvas gp, Model model, double[] eye, SolidColorBrush color)
        {


            double xmin = -0.5;
            double xmax = 0.5;
            double ymin = -0.5;
            double ymax = 0.5;

            var foo = model.Vertices.Length;

            Point[] pictureVertices = new Point[model.Vertices.Length];
            double scale = 300;
            for (int i = 0; i < model.Vertices.Length / 3; i++)
            {
                double x = model.Vertices[i, 0];
                double y = model.Vertices[i, 1];
                double z = model.Vertices[i, 2];
                double xprime = x / z;
                double yprime = y / z;
                var foo2 = (1 - (xprime - xmin) / (xmax - xmin));
                pictureVertices[i].X = scale * (1 - (xprime - xmin) / (xmax - xmin));
                pictureVertices[i].Y = scale * (yprime - ymin) / (ymax - ymin); // x / z

            }

            for (int i = 0; i < model.nFaces; i++)
            {

                int[] faceVertices = new int[model.Faces.GetLength(1)];

                for (int j = 0; j < model.Faces.GetLength(1); j++)
                {
                    faceVertices[j] = model.Faces[i, j];
                }



                if (IsFaceVisible(faceVertices, model.Vertices, eye))
                {
                    var points = new Point[faceVertices.Length];
                    for (int j = 0; j < faceVertices.Length - 1; j++)
                    {
                        points[j] = pictureVertices[model.Faces[i, j]];
                        //gp.Children.Add(new Line { X1 = pictureVertices[model.Faces[i, j]].X, Y1 = pictureVertices[model.Faces[i, j]].Y, X2 = pictureVertices[model.Faces[i, j + 1]].X, Y2 = pictureVertices[model.Faces[i, j + 1]].Y, Stroke = new SolidColorBrush(Colors.Black) });
                        if (j == faceVertices.Length - 2)
                        {
                            
                            //gp.Children.Add(new Line { X1 = pictureVertices[model.Faces[i, 0]].X, Y1 = pictureVertices[model.Faces[i, 0]].Y, X2 = pictureVertices[model.Faces[i, faceVertices.Length - 1]].X, Y2 = pictureVertices[model.Faces[i, faceVertices.Length - 1]].X, Stroke = new SolidColorBrush(Colors.Black) });
                            points[j + 1] = pictureVertices[model.Faces[i, faceVertices.Length - 1]];
                        }

                    }
                    DrawPolygon(points, color);
                }
            }




            //for (int i = 0; i < nEdges; i++)
            //{
            //    int n1 = etable[i, 0];
            //    int n2 = etable[i, 1];
            //    gp.Children.Add(new Segment(pictureVertices[n1], pictureVertices[n2]));
            //}
            //for (int i = 0; i < nPoints; i++)
            //    gp.Children.Add(new Dot(pictureVertices[i].X, pictureVertices[i].Y));

        }

        private void DrawPolygon(Point[] points, SolidColorBrush color)
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

        public SolidColorBrush CreateColorBrush(byte red, byte green, byte blue)
        {
            Color specifiedColor = Color.FromRgb(red, green, blue);
            return new SolidColorBrush(specifiedColor);
        }

        public double[,] RotateModelAroundY(double[,] cube)
        {
            var vtable = cube;
            var centroid = LinearAlgebra.CalculateCentroid(vtable);
            LinearAlgebra.TranslateVertices(vtable, new double[] { -centroid[0], -centroid[1], -centroid[2] });
            for (int i = 0; i < 8; i++)
            {
                var foo = LinearAlgebra.RotateAroundY(new double[] { vtable[i, 0], vtable[i, 1], vtable[i, 2] }, (5 * Math.PI) / 180);

                vtable[i, 0] = foo[0];
                vtable[i, 1] = foo[1];
                vtable[i, 2] = foo[2];
            }
            LinearAlgebra.TranslateVertices(vtable, new double[] { centroid[0], centroid[1], centroid[2] });
            return vtable;
        }

        public double[,] RotateModelAroundZ(double[,] cube)
        {
            var vtable = cube;
            var centroid = LinearAlgebra.CalculateCentroid(vtable);
            LinearAlgebra.TranslateVertices(vtable, new double[] { -centroid[0], -centroid[1], -centroid[2] });
            for (int i = 0; i < 8; i++)
            {
                var foo = LinearAlgebra.RotateAroundZ(new double[] { vtable[i, 0], vtable[i, 1], vtable[i, 2] }, (5 * Math.PI) / 180);

                vtable[i, 0] = foo[0];
                vtable[i, 1] = foo[1];
                vtable[i, 2] = foo[2];
            }
            LinearAlgebra.TranslateVertices(vtable, new double[] { centroid[0], centroid[1], centroid[2] });
            return vtable;
        }

        public double[,] ScaleModel(double[,] model, double[] scalars)
        {
            var vertices = model;
            var centroid = LinearAlgebra.CalculateCentroid(vertices);
            LinearAlgebra.TranslateVertices(vertices, new double[] { -centroid[0], -centroid[1], -centroid[2] });

            LinearAlgebra.ScaleVertices(vertices, new double[] { scalars[0], scalars[1], scalars[2] });

            LinearAlgebra.TranslateVertices(vertices, new double[] { centroid[0], centroid[1], centroid[2] });

            return vertices;
        }

        public bool IsFaceVisible(int[] faceVerts, double[,] verts, double[] Eye)
        {

            double[] p0 = new double[] { verts[faceVerts[0], 0], verts[faceVerts[0], 1], verts[faceVerts[0], 2] };
            double[] p1 = new double[] { verts[faceVerts[1], 0], verts[faceVerts[1], 1], verts[faceVerts[1], 2] };
            double[] p2 = new double[] { verts[faceVerts[2], 0], verts[faceVerts[2], 1], verts[faceVerts[2], 2] };

            double[] v = new double[] { (p1[0] - p0[0]), (p1[1] - p0[1]), (p1[2] - p0[2]) };
            double[] w = new double[] { (p2[0] - p1[0]), (p2[1] - p1[1]), (p2[2] - p1[2]) };

            var crossProduct = LinearAlgebra.CrossProduct(v, w);



            var foo = LinearAlgebra.DotProduct(crossProduct, LinearAlgebra.SubtractVectors(p0, Eye));
            var BAR = (LinearAlgebra.DotProduct(crossProduct, Eye) < 0);

            return (LinearAlgebra.DotProduct(crossProduct, LinearAlgebra.SubtractVectors(p0, Eye)) < 0);
        }
    }
    
}
