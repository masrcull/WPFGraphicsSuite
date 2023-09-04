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

        public MainWindow()
        {
            InitializeComponent();
            gp = this.FindName("Paper") as Canvas;


            var cubeModel = new Model("..\\..\\..\\Models\\cube_model.json");
            var plusModel = new Model("..\\..\\..\\Models\\plus_model.json");

            double[] Eye = new double[3]
            { 0, 0, 0 };

            bool redIncrease = true;
            bool greenIncrease = true;
            bool blueIncrease = true;

            byte red = 64;
            byte green = 128;
            byte blue = 200;

            cubeModel.Scale(new double[] { 17, 17, 17 });
            plusModel.Translate(new double[] { 0, 0, 3 });
            plusModel.Scale(new double[] { 1,1,0 });

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16);  // roughly 60 FPS
            timer.Tick += (s, e) =>
            {
                red = ColorHelper.IncrementRgbByte(red, (byte)4, ref redIncrease);
                blue = ColorHelper.IncrementRgbByte(blue, (byte)16, ref blueIncrease);
                green = ColorHelper.IncrementRgbByte(green, (byte)8, ref greenIncrease);

                gp.Children.Clear();

                cubeModel.RotateY((5 * Math.PI) / 180);
                //cubeModel.RotateZ((5 * Math.PI) / 180);
                plusModel.RotateZ((5 * Math.PI) / 180);
                //plusModel.RotateZ((5 * Math.PI) / 180);
                //cubeModel.RotateX((5 * Math.PI) / 180);
                //cubeModel.DrawFaces(ColorHelper.CreateColorBrush(red, green, blue), gp, Eye);
                plusModel.DrawFaces(Brushes.Red, gp, Eye);



            };
            timer.Start();

            ready = true; // Now we're ready to have sliders and buttons influence the display.
            var one = 1;
        }


    }
    
}
