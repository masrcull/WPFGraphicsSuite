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
        Canvas mainStage = null;
        double[] Eye = new double[] { 0, 0, 0 };

        GraphicContextControl GCC = new GraphicContextControl();

        public MainWindow()
        {
            InitializeComponent();
            //mainStage = this.FindName("MainStage") as Canvas;

            MainGrid.Children.Add(GCC);

            GCC.CreateCircleModel("C:\\ModelExports\\circley.json", 1, 255, 0, 0);
            

            var CircleModel = new Model("C:\\ModelExports\\circley.json", 0, 0, -15);
            var tableBottom = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", 0, 0, -15);
            var tableSideRight = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", 0, 0, -15);
            var tableSideLeft = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", 0, 0, -15);

            //CircleModel.Scale(3,3,3);

            tableSideLeft.Scale(0.3, 2, 2.5);
            tableSideLeft.Translate(2.5, .8, 0);

            tableSideRight.Scale(0.3, 2, 2.5);
            tableSideRight.Translate(-2.5, .8, 0);
            tableBottom.Scale(4.5, 0.3, 2.5);

            tableSideRight.color = new int[] { 0, 255, 0 };

            var table = GCC.AddModels( new List<Model> { tableSideRight, tableSideLeft, tableBottom/*, CircleModel */} );
            var circle = GCC.AddModels(new List<Model> {  CircleModel });


            //tableBottom.RotateX(10);
            //tableSideRight.RotateX(10);





            bool redIncrease = true;
            bool greenIncrease = true;
            bool blueIncrease = true;

            byte red = 64;
            byte green = 128;
            byte blue = 200;

            table.RotateY(90);
            table.RotateX(-.6);
            

            GCC.AddMethod(() =>
            {
                red = ColorHelper.IncrementRgbByte(red, (byte)4, ref redIncrease);
                blue = ColorHelper.IncrementRgbByte(blue, (byte)16, ref blueIncrease);
                green = ColorHelper.IncrementRgbByte(green, (byte)8, ref greenIncrease);

                //tableBottom.color = new int[] { red, green, blue };
               table.RotateY(.1);
                circle.RotateX(.1);
                

                //tableBottom.RotateX(.1);
            });

            GCC.Start();

            

            //tableSideRight.DrawFaces(GCC);
            //tableBottom.DrawFaces(GCC);
            


        }
    }
}
