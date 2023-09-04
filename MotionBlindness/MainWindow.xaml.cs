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



namespace MotionBlindness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Canvas mainStage;
        List<Model> models = new List<Model>();

        SolidColorBrush circleBrush;

        double circleHeight = 20;
        double circleWidth = 20;

        double rotationSpeed = 1;

        public MainWindow()
        {
            InitializeComponent();
            mainStage = this.FindName("MainStage") as Canvas;
            double[] Eye = new double[3]
            { 0, 0, 0 };

            circleBrush = Brushes.Yellow;

            Ellipse ellipse = new Ellipse();

            // Set the properties of the ellipse
            ellipse.Width = circleWidth;   // example width
            ellipse.Height = circleHeight;  // example height
            ellipse.Fill = circleBrush; // example fill color

            // Set its position on the Canvas
            Canvas.SetLeft(ellipse, 100); // X position
            Canvas.SetTop(ellipse, 50);  // Y position

            // Add the ellipse to the canvas
            mainStage.Children.Add(ellipse);







            var plusModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\plus_model_flat.json", mainStage, new double[] { -5.5, 5.5, 13.0 });
            plusModel.Scale(new double[] { 0, 0, 1 });
            //plusModel.Scale(new double[] {1,1,0});
            //plusModel2.Scale(new double[] { 1, 1, 0 });
            //plusModel.DrawFaces(Brushes.Red, mainStage, Eye);

            Model currentModel;

            for (double i = -5.5; i < 5.5; i += distanceSlider.Value)
            {
                for (double j = 5.5; j > -5.5; j -= distanceSlider.Value)
                {
                    currentModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\plus_model_flat.json", mainStage, new double[] { i, -j, 13.0 });
                    models.Add(currentModel);
                }
            }

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16);  // roughly 60 FPS
            timer.Tick += (s, e) =>
            {

                mainStage.Children.Clear();

                mainStage.Children.Add(ellipse);

                foreach (Model model in models)
                {
                    
                    model.RotateZ((rotationSpeed * Math.PI) / 180, false);
                    model.DrawFaces(Brushes.Red, mainStage, Eye);
                }
            };
            timer.Start();


        }

        private void distanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Model currentModel;

            models = new List<Model>();

            for (double i = -5.5; i < 5.5; i += distanceSlider.Value)
            {
                for (double j = 5.5; j > -5.5; j -= distanceSlider.Value)
                {
                    currentModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\plus_model_flat.json", mainStage, new double[] { i, -j, 13.0 });
                    models.Add(currentModel);
                }
            }
        }
    }
}
