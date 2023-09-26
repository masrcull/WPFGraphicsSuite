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


            var tableBottom = GCC.AddModel("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", 0, 0, -15);
            var tableSideRight = GCC.AddModel("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", 0, 0, 0 - 15);
            tableSideRight.Scale(0.3, 2, 2.5);
            tableSideRight.Translate(-2.5, -1, 0);
            tableBottom.Scale(4.5, 0.3, 2.5);
            //tableBottom.RotateX(10);
            //tableSideRight.RotateX(10);

            tableSideRight.color = new int[] { 0, 255, 0 };

            tableSideRight.RotateX(10);
            tableBottom.RotateX(10);

            bool redIncrease = true;
            bool greenIncrease = true;
            bool blueIncrease = true;

            byte red = 64;
            byte green = 128;
            byte blue = 200;

            GCC.AddMethod(() =>
            {
                red = ColorHelper.IncrementRgbByte(red, (byte)4, ref redIncrease);
                blue = ColorHelper.IncrementRgbByte(blue, (byte)16, ref blueIncrease);
                green = ColorHelper.IncrementRgbByte(green, (byte)8, ref greenIncrease);

                tableBottom.color = new int[] { red, green, blue };

                tableBottom.RotateX(.1);
            });

            GCC.Start();

            

            //tableSideRight.DrawFaces(GCC);
            //tableBottom.DrawFaces(GCC);
            


        }
    }
}
