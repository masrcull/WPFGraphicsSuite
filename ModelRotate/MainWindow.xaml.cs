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
using ModelRender.Models;

namespace ModelRotate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GraphicContextControl GCC = new GraphicContextControl();

        public MainWindow()
        {
            InitializeComponent();
            MainStage.Children.Add(GCC);
            GCC.DirectionalLightEnabled = true;

            GCC.DirectionalLightEnabled = false;

            var sphereRadi = ShapeHelper.GenerateSphere(8);

            var sphereModel = new Model("C:\\ModelExports\\sphereplz.json", new double[] { 0, 0,4 });
            var quarterModel1 = new Model("C:\\ModelExports\\quarter_circle.json", new double[] { 0, 0, 4 });

            var cubeModel = new Model("..\\..\\..\\Models\\cube_model.json", new double[] { 0, 0, 13 });
            var plusModel = new Model("..\\..\\..\\Models\\plus_model.json", new double[] { 0, 0, 13 });
            var plusModel2 = new Model("..\\..\\..\\Models\\plus_model.json", new double[] { 3, 0, 13 });
            //var plusModel = new Model("..\\..\\..\\Models\\plus_model.json");

            GCC.AddModels(new List<Model> { sphereModel } );

            bool redIncrease = true;
            bool greenIncrease = true;
            bool blueIncrease = true;

            byte red = 64;
            byte green = 128;
            byte blue = 200;


            //int horizontalResolution = 8;
            //int verticalResolution = 4;
            //var models = new List<Model>();
            //double verticalSpace = 1.0 / 16.0;

            //for (int i = 0; i < sphereRadi.Length - 1; i++)
            //{
            //    var hellowurld = verticalSpace * i;
            //    Model tempModel = new Model("C:\\ModelExports\\circley.json", new double[] { 0, (verticalSpace * i), 4 });
            //    Model tempModel2 = new Model("C:\\ModelExports\\circley.json", new double[] { 0, (verticalSpace * i), 4 });
            //    tempModel.RotateX(LinearAlgebra.DegreeToRadians(90));
            //    tempModel2.RotateX(LinearAlgebra.DegreeToRadians(270));
            //    tempModel.Scale(sphereRadi[i]);
            //    tempModel2.Scale(sphereRadi[i]);
            //    models.Add(tempModel);
            //    models.Add(tempModel2);
            //}
            //var coicles = GCC.AddModels(models);

            


            GCC.AddMethod( () =>
            {
                red = ColorHelper.IncrementRgbByte(red, (byte)4, ref redIncrease);
                blue = ColorHelper.IncrementRgbByte(blue, (byte)16, ref blueIncrease);
                green = ColorHelper.IncrementRgbByte(green, (byte)8, ref greenIncrease);

                sphereModel.RotateY(.1);

                //coicles.RotateX(.1);
            });
            GCC.Start();

            
            var one = 1;
        }


    }
    
}
