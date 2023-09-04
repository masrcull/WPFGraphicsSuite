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

namespace Smoothy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double currentX;
        private double currentY;

        private List<Point> points;
        private List<Point> newPoints;

        private bool isSubdivised;
        

        public MainWindow()
        {
            InitializeComponent();
            currentX = 0;
            currentY = 0;
            points = new List<Point>();
            newPoints = new List<Point>();
            isSubdivised = false;
        }


        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!isSubdivised)
            {
                // Retrieve the click position relative to the Canvas
                Point position = e.GetPosition(myCanvas);

                myCanvas.Children.Clear();

                // Use the position
                currentX = position.X;
                currentY = position.Y;

                points.Add(position);


                myCanvas.Children.Add(new Polygon { Points = new PointCollection(points), Stroke = Brushes.Black, Fill = Brushes.Transparent });
            }
        }

        private void DualPolygon(object sender, RoutedEventArgs e)
        {
            isSubdivised = true;
            var n = points.Count;
            for (int i = 0; i < n; i += 2)
            {
                int nexti = (i + 1) % n; // index of next point.
                int lasti = (i + (n - 1)) % n; // index of last point, probably.

                newPoints.Add(new Point(((1f / 2f) * points[i].X + (1f / 2f) * points[lasti].X), ((1f / 2f) * points[i].Y + (1f / 2f) * points[lasti].Y)));
                if (i != points.Count - 1)
                {
                    newPoints.Add(new Point(((1f / 2f) * points[i].X + (1f / 2f) * points[nexti].X), ((1f / 2f) * points[i].Y + (1f / 2f) * points[nexti].Y)));
                }
            }

            
          

            //myCanvas.Children.Clear();

            myCanvas.Children.Add(new Polygon { Points = new PointCollection(points), Stroke = Brushes.Red, Fill = new SolidColorBrush(GetRandomColor()) });
            myCanvas.Children.Add(new Polygon { Points = new PointCollection(newPoints), Stroke = Brushes.Black, Fill = new SolidColorBrush(GetRandomColor()) });

            points = newPoints;
            newPoints = new List<Point>();

        }

        public Color GetRandomColor()
        {
            Random rnd = new Random();
            byte red = (byte)rnd.Next(256);   // Generate a random value between 0 and 255 for the red component
            byte green = (byte)rnd.Next(256); // Do the same for green
            byte blue = (byte)rnd.Next(256);  // And blue

            return Color.FromArgb(255, red, green, blue); // Assuming fully opaque color
        }

        private void Subdivide(object sender, RoutedEventArgs e)
        {
            isSubdivised = true;
            var n = points.Count;
            for(int i = 0; i < n; i+=2)
            {
                int nexti = (i + 1) % n; // index of next point.
                int lasti = (i + (n - 1)) % n; // index of last point, probably.

                var vert1 = new Point(((1f / 3f) * points[i].X + (2f / 3f) * points[lasti].X), ((1f / 3f) * points[i].Y + (2f / 3f) * points[lasti].Y));
                var vert2 = new Point(((1f / 3f) * points[lasti].X + (2f / 3f) * points[i].X), ((1f / 3f) * points[lasti].Y + (2f / 3f) * points[i].Y));
                var vert3 = new Point(((1f / 3f) * points[nexti].X + (2f / 3f) * points[i].X), ((1f / 3f) * points[nexti].Y + (2f / 3f) * points[i].Y));
                var vert4 = new Point(((1f / 3f) * points[i].X + (2f / 3f) * points[nexti].X), ((1f / 3f) * points[i].Y + (2f / 3f) * points[nexti].Y));



                newPoints.Add(new Point(((1f / 3f) * points[i].X + (2f / 3f) * points[lasti].X), ((1f / 3f) * points[i].Y + (2f / 3f) * points[lasti].Y)));
                newPoints.Add(new Point(((1f / 3f) * points[lasti].X + (2f / 3f) * points[i].X), ((1f / 3f) * points[lasti].Y + (2f / 3f) * points[i].Y)));
                if (i != points.Count - 1)
                {
                    newPoints.Add(new Point(((1f / 3f) * points[nexti].X + (2f / 3f) * points[i].X), ((1f / 3f) * points[nexti].Y + (2f / 3f) * points[i].Y)));
                    newPoints.Add(new Point(((1f / 3f) * points[i].X + (2f / 3f) * points[nexti].X), ((1f / 3f) * points[i].Y + (2f / 3f) * points[nexti].Y)));
                }
            }

            myCanvas.Children.Clear();

            myCanvas.Children.Add(new Polygon { Points = new PointCollection(points), Stroke = Brushes.Red, Fill = Brushes.Transparent });
            myCanvas.Children.Add(new Polygon { Points = new PointCollection(newPoints), Stroke = Brushes.Black, Fill = Brushes.Transparent });

            points = newPoints;
            newPoints = new List<Point>();
        }

        private void ClickTest(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(";)");
        }

        private void ClearPolygon(object sender, RoutedEventArgs e)
        {
            isSubdivised = false;
            myCanvas.Children.Clear();
            points = new List<Point>();
            newPoints = new List<Point>();
        }

    }
}
