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

namespace SpacialFreq
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Canvas mainStage;

        public double WhiteLineMultiplier;
        public double GrayAmount;
        public double[] Eye;

        public Model cubeModel;
        public Model greyCube;

        public MainWindow()
        {
            InitializeComponent();
            mainStage = FindName("MainStage") as Canvas;
            Eye = new double[] { 0, 0, 0 };
            WhiteLineMultiplier = lineAmountSlider.Value;
            GrayAmount = 127;

            cubeModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", mainStage, new double[] { 0, 0, 13 });
            cubeModel.Scale(new double[] { 35, WhiteLineMultiplier, 0 });
            cubeModel.Translate(new double[] { 0, 16.4, 0 });

            greyCube = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", mainStage, new double[] { 0, 0, 13 });
            greyCube.Scale(new double[] { 35, 17, 0 });
            
            greyCube.Translate(new double[] { 0, -9, 0 });

            greyCube.DrawFaces(Brushes.Gray, mainStage, Eye);
            cubeModel.DrawFaces(Brushes.White, mainStage, Eye);
            //cubeModel.Translate(0, (-1 * 2 * WhiteLineMultiplier), 0);
            cubeModel.DrawFaces(Brushes.White, MainStage, Eye);

            var foo2 = WhiteLineMultiplier; 

            while (cubeModel.Centroid[1] > 0)
            {
                cubeModel.Translate(0, (-1 * 2 * WhiteLineMultiplier), 0);
                cubeModel.DrawFaces(Brushes.White, MainStage, Eye);
            }

            var foo = LinearAlgebra.CalculateCentroid(cubeModel.Vertices);
            Console.WriteLine("fuck");

        }


        public void DrawWhiteLines()
        {
            if (mainStage != null)
            {
                mainStage.Children.Clear();
            }
            cubeModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", mainStage, new double[] { 0, 0, 13 });
            cubeModel.Scale(new double[] { 35, WhiteLineMultiplier, 0 });
            cubeModel.Translate(new double[] { 0, 17.4, 0 });

            while (cubeModel.Centroid[1] > 0)
            {
                cubeModel.Translate(0, (-1 * 2 * WhiteLineMultiplier), 0);
                cubeModel.DrawFaces(Brushes.White, MainStage, Eye);
            }
            if(greyCube != null)
                greyCube.DrawFaces(new SolidColorBrush(Color.FromRgb((byte)GrayAmount, (byte)GrayAmount, (byte)GrayAmount)), mainStage, Eye);
        }

        public void DrawGreyBox()
        {
            if (greyCube != null)
            {
                greyCube.DrawFaces(new SolidColorBrush(Color.FromRgb((byte)GrayAmount, (byte)GrayAmount, (byte)GrayAmount)), mainStage, Eye);
            }
        }

        private void lineAmountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {   
            if(lblFrequency != null)
                lblFrequency.Content = ((double)e.NewValue).ToString();
            WhiteLineMultiplier = (double)e.NewValue;
            DrawWhiteLines();
        }

        private void grayScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GrayAmount = (double)e.NewValue;
            DrawGreyBox();
        }
    }
}
