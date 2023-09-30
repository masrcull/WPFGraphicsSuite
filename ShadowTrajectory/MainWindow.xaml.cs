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
            GCC.CreateTriangleModel("C:\\ModelExports\\triangley.json", 1, 0, 128, 0);

            var TriangleMan = new Model("C:\\ModelExports\\triangley.json", 0, 0, 15);
            CircleShadowModel = new Model("C:\\ModelExports\\circley.json", 0, 0, 15);
            CircleModel = new Model("C:\\ModelExports\\circley.json", 0, 1, 15);
            var tableBottom = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", 0, 0, 15);
            var tableSideRightFront = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", 2.0, -1.1, 14.2);
            var tableSideRightBack = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", 2.0, -1.1, 16);
            var tableSideLeftFront = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", -2.0, -1.1, 14.2);
            var tableSideLeftBack = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", -2.0, -1.1, 16);


            //CircleModel.Scale(3,3,3);
            CircleShadowModel.Scale(1, 1, 1);

            tableSideLeftFront.Scale(0.3, 2, .2);
            tableSideLeftBack.Scale(0.3, 2, .2);


            tableSideRightFront.Scale(0.3, 2, .2);
            tableSideRightBack.Scale(0.3, 2, .2);



            tableBottom.Scale(4.5, 0.3, 2.5);


            var table = GCC.AddModels( new List<Model> { tableSideRightFront, tableSideRightBack, tableSideLeftFront, tableSideLeftBack, tableBottom } );
            var shadow = GCC.AddModels(new List<Model> { CircleShadowModel });
            //var triangleMan = GCC.AddModels(new List<Model> { TriangleMan });

            table.SetColorAllModels(66, 66, 66);
            CircleShadowModel.SetColor(33, 33, 33);

            //var table = GCC.AddModels(new List<Model> { tableSideRight, tableSideLeft, tableBottom });

            //var table = GCC.AddModels(new List<Model> {  CircleModel, CircleModel2 });

            var circleMesh = GCC.AddModels(new List<Model> { CircleModel });

            //tableBottom.RotateX(10);
            //tableSideRight.RotateX(10);



            //circleMesh.RotateY(3.14);

            bool redIncrease = true;
            bool greenIncrease = true;
            bool blueIncrease = true;

            byte red = 64;
            byte green = 128;
            byte blue = 200;

            CircleShadowModel.RotateX(-Math.PI / 2);

            table.RotateX(-Math.PI/8);
            table.RotateY(-Math.PI / 8);
            CircleShadowModel.RotateX(Math.PI / 8);
            CircleShadowModel.RotateY(Math.PI / 8);
            //table.RotateX(-.6);


            GCC.AddMethod(() =>
            {
                red = ColorHelper.IncrementRgbByte(red, (byte)4, ref redIncrease);
                blue = ColorHelper.IncrementRgbByte(blue, (byte)16, ref blueIncrease);
                green = ColorHelper.IncrementRgbByte(green, (byte)8, ref greenIncrease);

                //tableBottom.color = new int[] { red, green, blue };

                //circles.RotateX(.1);

                //table.RotateY(.1);
                //CircleModel.RotateY(.1);

                //tableBottom.RotateX(.1);
            });

            GCC.Start();

            

            //tableSideRight.DrawFaces(GCC);
            //tableBottom.DrawFaces(GCC);
            


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

            //var centroid = CircleModel.Centroid;
            //CircleModel.Translate(-centroid[0], -centroid[1], -centroid[2]);
            //CircleModel.Translate(e.NewValue, 0, 0);
            //CircleModel.Translate(centroid[0], centroid[1], centroid[2]);
        }

        private void posYSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double move = -.2;
            if (e.NewValue > e.OldValue)
            {
                move = .2;
            }
            CircleShadowModel.Translate(0, move, move);

            //var centroid = CircleModel.Centroid;
            //CircleModel.Translate(-centroid[0], -centroid[1], -centroid[2]);
            //CircleModel.Translate(e.NewValue, 0, 0);
            //CircleModel.Translate(centroid[0], centroid[1], centroid[2]);
        }

    }
}
