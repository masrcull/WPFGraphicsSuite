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
using GraphicsCommon;
using ModelRender.Models;

namespace ShadowTrajectory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        GraphicContextControl GCC = new GraphicContextControl();

        Model CircleShadowModel;
        Model CircleModel;

        public MainWindow()
        {
            InitializeComponent();
            //mainStage = this.FindName("MainStage") as Canvas;

            MainStage.Children.Add(GCC);

            GCC.CreateCircleModel("C:\\ModelExports\\circley.json", .2, 128, 0, 0);
            GCC.CreateTriangleModel("C:\\ModelExports\\triangley.json", .2, 0, 128, 0);

            var TriangleMan = new Model("C:\\ModelExports\\triangley.json", 0, 0, 14);
            CircleShadowModel = new Model("C:\\ModelExports\\circley.json", 0, 0, 14);
            CircleShadowModel.Scale(1, 1, 1);
            CircleShadowModel.SetColor(33, 33, 33);
            CircleShadowModel.RotateX(-Math.PI / 2);
            CircleShadowModel.RotateX(Math.PI / 8);
            CircleShadowModel.RotateY(Math.PI / 8);

            CircleModel = new Model("C:\\ModelExports\\circley.json", 0, 1, 15);
            var tableBottom = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", 0, 0, 15);

            var tableSideRightFront = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", 2.0, -1.1, 14.2);
            tableSideRightFront.Scale(0.3, 2, .2);

            var tableSideRightBack = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", 2.0, -1.1, 16);
            tableSideRightBack.Scale(0.3, 2, .2);

            var tableSideLeftFront = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", -2.0, -1.1, 14.2);
            tableSideLeftFront.Scale(0.3, 2, .2);

            var tableSideLeftBack = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", -2.0, -1.1, 16);
            tableSideLeftBack.Scale(0.3, 2, .2);

            
            

            tableBottom.Scale(4.5, 0.3, 2.5);


            var table = GCC.AddModels( new List<Model> { tableSideRightFront, tableSideRightBack, tableSideLeftFront, tableSideLeftBack, tableBottom } );
            table.SetColorAllModels(66, 66, 66);
            table.RotateX(-Math.PI / 8);
            table.RotateY(-Math.PI / 8);

            var shadow = GCC.AddModels(new List<Model> { CircleShadowModel });
            var circleMesh = GCC.AddModels(new List<Model> { CircleModel });


            GCC.AddMethod(() =>
            {
                //table.RotateY(.1);
                //shadow.RotateY(-.1);
                //circleMesh.RotateY(.1);
            });

            GCC.Start();
        }

        private void posXSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double move = .2;
            if(e.NewValue>e.OldValue)
            {
                move = -.2;    
            }
            CircleShadowModel.Translate(move, 0, 0);
            CircleModel.Translate(move, move/2, 0);

        }

        private void posYSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double move = -.2;
            if (e.NewValue > e.OldValue)
            {
                move = .2;
            }
            CircleShadowModel.Translate(0, move/2, move);

        }

        private void OnChecked(object sender, RoutedEventArgs e)
        {


            GCC.Meshes.RemoveAt(2);
            GCC.Meshes.RemoveAt(1);



            RadioButton checkedRadioButton = sender as RadioButton;

            if (checkedRadioButton != null)
            {
                switch (checkedRadioButton.Content)
                {
                    case "Disc":
                        CircleShadowModel = new Model("C:\\ModelExports\\circley.json", CircleShadowModel.Centroid[0], CircleShadowModel.Centroid[1], CircleShadowModel.Centroid[2]);
                        break;
                    case "Triangle":
                        CircleShadowModel = new Model("C:\\ModelExports\\triangley.json", CircleShadowModel.Centroid[0], CircleShadowModel.Centroid[1], CircleShadowModel.Centroid[2]);
                        break;
                }
                CircleShadowModel.Scale(1, 1, 1);
                CircleShadowModel.SetColor(33, 33, 33);
                CircleShadowModel.RotateX(-Math.PI / 2);
                CircleShadowModel.RotateX(Math.PI / 8);
                CircleShadowModel.RotateY(Math.PI / 8);
                GCC.AddModels(new List<Model> { CircleShadowModel });
                GCC.AddModels(new List<Model> { CircleModel });
            }
        }

    }
}
