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


        Mesh plusMesh = new Mesh();

        Mesh circleNorthMesh = new Mesh();



        bool diameterChanged = true;
        bool plusChange = false;

        double circleScalar = .2;

        const double plusRenderMin = -8.5;
        const double plusRenderMax = 8.5;

        double[,] OGVerts;

        double rotationSpeed = -.05;

        public MainWindow()
        {
            InitializeComponent();

            MainStage.Children.Add(GCC);

            GCC.CreateCircleModel("C:\\ModelExports\\circley.json", 1, 255, 0, 0);

            var dataCircleNorth = Model.GetModelData("C:\\ModelExports\\circley.json", 0, 4.5, 12);
            var dataCircleEast = Model.GetModelData("C:\\ModelExports\\circley.json", 4.5, 0, 12);
            var dataCircleSouth = Model.GetModelData("C:\\ModelExports\\circley.json", 0, -4.5, 12);
            var dataCircleWest = Model.GetModelData("C:\\ModelExports\\circley.json", -4.5, 0, 12);





            //GCC.AddMesh(circleNorthMesh);


            var plusModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\plus_model_flat.json", new double[] { 6, 6, 13.0 });
            renderPlusses();
            //circleMesh = GCC.AddModels(new List<Model> { circleModelNorth, circleModelEast, circleModelSouth, circleModelWest });
            circleNorthMesh.SetColorAllModels((int)RedSlider.Value, (int)GreenSlider.Value, (int)BlueSlider.Value);


            GCC.AddMethod(() =>
            {
                plusMesh.RotateZ(rotationSpeed);

                if (diameterChanged)
                {
                    circleScalar = diameterSlider.Value;
                    var modelNorth = new Model(dataCircleNorth, 0, 4.5, 12);
                    var modelEast = new Model(dataCircleEast, 4.5, 0, 12);
                    var modelSouth = new Model(dataCircleSouth, 0, -4.5, 12);
                    var modelWest = new Model(dataCircleWest, -4.5, 0, 12);

                    modelNorth.Scale(circleScalar);
                    modelEast.Scale(circleScalar);
                    modelSouth.Scale(circleScalar);
                    modelWest.Scale(circleScalar);


                    circleNorthMesh = new Mesh(new List<Model> { modelNorth, modelEast, modelSouth, modelWest });

                    if (RedSlider != null && GreenSlider != null && BlueSlider != null)
                    {
                        circleNorthMesh.SetColorAllModels((int)RedSlider.Value, (int)GreenSlider.Value, (int)BlueSlider.Value);
                    }

                    diameterChanged = false;
                    plusChange = true;
                }

                if(plusChange)
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
                    GCC.AddMesh(circleNorthMesh);
                    plusChange = false;

                }

                //renderPlusses();

            });


            GCC.Start();

        }





        private void renderPlusses()
        {


            plusChange = true;
            
        }

        private void distanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            renderPlusses();

        }

        private void speedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rotationSpeed = -speedSlider.Value;
        }

        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            if (RedSlider != null && GreenSlider != null && BlueSlider != null)
            {
                circleNorthMesh.SetColorAllModels((int)RedSlider.Value, (int)GreenSlider.Value, (int)BlueSlider.Value);
            }
        }

        private void diameterSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //circleScalar = diameterSlider.Value;
            diameterChanged = true;
        }
    }
}
