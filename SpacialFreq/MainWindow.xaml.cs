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
        public int GrayAmount;

        List<Model> models = new List<Model>();

        double lineStart = 8;

        public Model greyCube;

        private bool GreyUpdated = false;

        public MainWindow()
        {
            InitializeComponent();

            MainStage.Children.Add(GCC);

            GCC.ClearOnDraw = false;

            WhiteLineMultiplier = lineAmountSlider.Value;
            GrayAmount = 127;



            greyCube = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", new double[] { 0, 0, 13 });
            greyCube.Scale(new double[] { 35, 17, 0 });
            greyCube.Translate(new double[] { 0, -9, 0 });

            greyCube.SetColor(GrayAmount, GrayAmount, GrayAmount);


            GCC.AddModels(new List<Model> { greyCube });

            //double currentY = lineStart;

            //while (currentY > 0)
            //{
            //    var tempModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", new double[] { 0, 0, 13 });
            //    tempModel.Scale(new double[] { 35, WhiteLineMultiplier, 0 });
            //    tempModel.Translate(0, currentY, 0);
            //    cubeModel.Translate(0, currentY, 0);
            //    models.Add(tempModel);
            //    currentY = currentY - (2 * WhiteLineMultiplier);
            //}

            //GCC.AddModels(models);

            GCC.Start();
        }


        public void DrawScene()
        {
            if (GCC != null && !GreyUpdated)
            {
                GCC.Clear();
                GCC.Meshes.Clear();
            }

            models = new List<Model>();
            double[] currentCentroid;
            var currentY = lineStart;

            var primerModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", new double[] { 0, 0, 13 });
            primerModel.SetColor(1, 1, 50);
            primerModel.Scale(new double[] { 35, WhiteLineMultiplier, 0 });
            primerModel.Translate(0, currentY, 0);
            currentCentroid = primerModel.Centroid;
            models.Add(primerModel);

            currentY = (currentY - WhiteLineMultiplier * 2);

            while (currentCentroid[1] > 0)
            {
                var tempModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", new double[] { 0, 0, 13 });
                tempModel.SetColor(200,200,200);
                tempModel.Scale(new double[] { 35, WhiteLineMultiplier, 0 });
                tempModel.Translate(0, currentY, 0);
                currentY = (currentY - WhiteLineMultiplier * 2);
                models.Add(tempModel);
                currentCentroid = tempModel.Centroid; 
            }

            if (greyCube != null)
            {
                if (GreyUpdated)
                {
                    greyCube.SetColor(GrayAmount, GrayAmount, GrayAmount);
                    GreyUpdated = false;
                }
                models.Add(greyCube);
                GCC.AddModels(models);
            }

        }


        private void lineAmountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {   
            if(lblFrequency != null)
                lblFrequency.Content = ((double)e.NewValue).ToString();
            WhiteLineMultiplier = (double)e.NewValue;
            DrawScene();
        }

        private void grayScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GrayAmount = (int)e.NewValue;
            GreyUpdated = true;
            DrawScene();
        }
    }
}
