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
using ModelRender.Models;

namespace MotionBlindness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public Canvas mainStage;
        //List<Model> models = new List<Model>();

        GraphicContextControl GCC = new GraphicContextControl();

        SolidColorBrush circleBrush;

        Mesh plusMesh = new Mesh();

        List<Ellipse> circles = new List<Ellipse>();

        double circleDiameter = 20;

        Mesh circleMesh = new Mesh();

        const double plusRenderMin = -8.5;
        const double plusRenderMax = 8.5;

        double rotationSpeed = 1;

        public MainWindow()
        {
            InitializeComponent();

            MainStage.Children.Add(GCC);

            circleBrush = Brushes.Yellow;

            GCC.CreateCircleModel("C:\\ModelExports\\circley.json", .2, 255, 0, 0);
  
            Model circleModelNorth = new Model("C:\\ModelExports\\circley.json", 0, 4.5, 12);
            Model circleModelEast = new Model("C:\\ModelExports\\circley.json", 4.5, 0, 12);
            Model circleModelSouth = new Model("C:\\ModelExports\\circley.json", 0, -4.5, 12);
            Model circleModelWest = new Model("C:\\ModelExports\\circley.json", -4.5, 0, 12);
            
            

            var plusModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\plus_model_flat.json", new double[] { 6, 6, 13.0 });

            renderPlusses();
            circleMesh = GCC.AddModels(new List<Model> { circleModelNorth, circleModelEast, circleModelSouth, circleModelWest });
            circleMesh.SetColorAllModels((int)RedSlider.Value, (int)GreenSlider.Value, (int)BlueSlider.Value);


            GCC.AddMethod(() =>
            {
                plusMesh.RotateZ(.1);
                //circleMesh.RotateY(.1);
            });


            GCC.Start();









            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromMilliseconds(16);  // roughly 60 FPS
            //timer.Tick += (s, e) =>
            //{

            //    //mainStage.Children.Clear();

                

            //    foreach (Model model in models)
            //    {
                    
            //        model.RotateZ((rotationSpeed * Math.PI) / 180, null, false);
            //        //model.DrawFaces(Brushes.Red, mainStage, Eye);
            //    }

            //    foreach(Ellipse circle in circles)
            //    {
            //        circle.Fill = circleBrush;
            //        mainStage.Children.Add(circle);
            //    }
            //};
            //timer.Start();


        }

        private void renderCircles()
        {

        }

        private void renderPlusses()
        {
            GCC.Meshes = new List<Mesh>();
            var models = new List<Model>();
            for (double i = plusRenderMin; i < plusRenderMax; i += distanceSlider.Value)
            {
                for (double j = plusRenderMax; j > plusRenderMin; j -= distanceSlider.Value)
                {
                    models.Add(new Model("..\\..\\..\\..\\ModelRotate\\Models\\plus_model_flat.json", new double[] { i, -j, 13.0 }));
                }
            }
            plusMesh = GCC.AddModels(models);
            GCC.AddMesh(circleMesh);
        }

        private void distanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            renderPlusses();

        }

        private void speedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rotationSpeed = speedSlider.Value;
        }

        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            if (RedSlider != null && GreenSlider != null && BlueSlider != null)
            {
                circleMesh.SetColorAllModels((int)RedSlider.Value, (int)GreenSlider.Value, (int)BlueSlider.Value);
            }
        }

        private void diameterSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double scalar = .8;
            if (e.NewValue > e.OldValue)
                scalar += .4;
            foreach (var model in circleMesh.Models)
            {
                model.Scale(scalar, scalar, scalar);
            }
        }
    }
}
