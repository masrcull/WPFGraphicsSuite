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
using System.Windows.Media.Animation;
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

        List<Ellipse> circles = new List<Ellipse>();

        double circleDiameter = 20;

        const double plusRenderMin = -11;
        const double plusRenderMax = 11;

        double rotationSpeed = 1;

        public MainWindow()
        {
            InitializeComponent();
            mainStage = this.FindName("MainStage") as Canvas;
            double[] Eye = new double[3]
            { 0, 0, 0 };

            circleBrush = Brushes.Yellow;

            circles.Add(ShapeHelper.CreateCircle(circleDiameter, 400, 100, circleBrush));
            circles.Add(ShapeHelper.CreateCircle(circleDiameter, 400, 700, circleBrush));
            circles.Add(ShapeHelper.CreateCircle(circleDiameter, 200, 300, circleBrush));


            mainStage.Children.Add(circles[0]);


            




            var plusModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\plus_model_flat.json", mainStage, new double[] { plusRenderMin, plusRenderMax, 13.0 });
            plusModel.Scale(new double[] { 0, 0, 1 });
            //plusModel.Scale(new double[] {1,1,0});
            //plusModel2.Scale(new double[] { 1, 1, 0 });
            //plusModel.DrawFaces(Brushes.Red, mainStage, Eye);

            Model currentModel;

            for (double i = plusRenderMin; i < plusRenderMax; i += distanceSlider.Value)
            {
                for (double j = plusRenderMax; j > plusRenderMin; j -= distanceSlider.Value)
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

                

                foreach (Model model in models)
                {
                    
                    model.RotateZ((rotationSpeed * Math.PI) / 180, false);
                    model.DrawFaces(Brushes.Red, mainStage, Eye);
                }

                foreach(Ellipse circle in circles)
                {
                    circle.Fill = circleBrush;
                    mainStage.Children.Add(circle);
                }
            };
            timer.Start();


        }

        private void distanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Model currentModel;

            models = new List<Model>();

            for (double i = plusRenderMin; i < plusRenderMax; i += distanceSlider.Value)
            {
                for (double j = plusRenderMax; j > plusRenderMin; j -= distanceSlider.Value)
                {
                    currentModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\plus_model_flat.json", mainStage, new double[] { i, -j, 13.0 });
                    models.Add(currentModel);
                }
            }
        }

        private void speedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rotationSpeed = speedSlider.Value;
        }

        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (RedSlider != null && GreenSlider != null && BlueSlider != null)
            {
                circleBrush = ColorHelper.CreateColorBrush((byte)RedSlider.Value, (byte)GreenSlider.Value, (byte)BlueSlider.Value);
            }
        }

        private void diameterSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach(Ellipse circle in circles)
            {
                circle.Width = diameterSlider.Value;
                circle.Height = diameterSlider.Value;
            }
        }
    }
}
