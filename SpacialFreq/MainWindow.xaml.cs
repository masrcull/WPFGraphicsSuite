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

namespace SpacialFreq
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        GraphicContextControl GCC = new GraphicContextControl();
        public double WhiteLineMultiplier;
        public double GrayAmount;

        List<Model> models = new List<Model>();

        public Model cubeModel;
        public Model greyCube;

        public MainWindow()
        {
            InitializeComponent();

            MainStage.Children.Add(GCC);

            GCC.ClearOnDraw = false;

            WhiteLineMultiplier = lineAmountSlider.Value;
            GrayAmount = 127;

            cubeModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json",new double[] { 0, 0, 13 });
            cubeModel.Scale(new double[] { 35, WhiteLineMultiplier, 0 });
            cubeModel.Translate(new double[] { 0, 6.7, 0 });

            greyCube = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", new double[] { 0, 0, 13 });
            greyCube.Scale(new double[] { 35, 17, 0 });
            greyCube.Translate(new double[] { 0, -9, 0 });

            greyCube.SetColor(128, 128, 128);
            cubeModel.SetColor(255,0,0); 

            GCC.AddModels(new List<Model> { cubeModel });
            GCC.AddModels(new List<Model> { greyCube });

            double currentY = 6.7;

            while (cubeModel.Centroid[1] > 0)
            {
                currentY = -(currentY - (1 * 2 * WhiteLineMultiplier));
                var fuckoff = cubeModel.Centroid[1];
                var tempModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", new double[] { 0, 0, 13 });
                tempModel.Scale(new double[] { 35, WhiteLineMultiplier, 0 });
                tempModel.Translate(0, currentY, 0);
                cubeModel.Translate(0, currentY, 0);
                models.Add(tempModel);
            }

            GCC.AddModels(models);

            GCC.Start();
        }


        public void DrawWhiteLines()
        {
            if (GCC != null)
            {
                //GCC.Clear();
            }
            cubeModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", new double[] { 0, 0, 13 });
            cubeModel.Scale(new double[] { 35, WhiteLineMultiplier, 0 });
            cubeModel.Translate(new double[] { 0, 17.4, 0 });

            while (cubeModel.Centroid[1] < 1)
            {
                cubeModel.Translate(0, (1 * 2 * WhiteLineMultiplier), 0);
            }
            //if(greyCube != null)
            //    greyCube.DrawFaces(new SolidColorBrush(Color.FromRgb((byte)GrayAmount, (byte)GrayAmount, (byte)GrayAmount)), mainStage, Eye);
        }

        public void DrawGreyBox()
        {
            //if (greyCube != null)
            //{
            //    greyCube.DrawFaces(new SolidColorBrush(Color.FromRgb((byte)GrayAmount, (byte)GrayAmount, (byte)GrayAmount)), mainStage, Eye);
            //}
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
            //DrawGreyBox();
        }
    }
}
