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
using ModelRender.Helpers;
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

            var sphereRadi = ShapeHelper.GenerateSphereRadi(8);

            //var sphereModel = new Model("C:\\ModelExports\\sphereplz.json", new double[] { 0, 0,4 });
            var quarterModel1 = new Model("C:\\ModelExports\\quarter_circle.json", new double[] { 0, 0, 4 });

            var cubeModel = new Model("..\\..\\..\\Models\\cube_model.json", new double[] { 0, 0, 13 });
            var plusModel = new Model("..\\..\\..\\Models\\plus_model.json", new double[] { 0, 0, 13 });
            var plusModel2 = new Model("..\\..\\..\\Models\\plus_model.json", new double[] { 3, 0, 13 });
            //var plusModel = new Model("..\\..\\..\\Models\\plus_model.json");

            //GCC.AddModels(new List<Model> { sphereModel } );

            bool redIncrease = true;
            bool greenIncrease = true;
            bool blueIncrease = true;

            byte red = 64;
            byte green = 128;
            byte blue = 200;


            int horizontalResolution = 8;
            int verticalResolution = 4;
            var models = new List<Model>();
            double verticalSpace = 1.0 / 16.0;

            for (int i = 0; i < sphereRadi.Length - 1; i++)
            {
                var hellowurld = verticalSpace * i;
                Model tempModel = new Model("C:\\ModelExports\\circley.json", new double[] { 0, (verticalSpace * i), 4 });
                Model tempModel2 = new Model("C:\\ModelExports\\circley.json", new double[] { 0, (verticalSpace * i), 4 });
                tempModel.RotateX(LinearAlgebra.DegreeToRadians(90));
                tempModel2.RotateX(LinearAlgebra.DegreeToRadians(270));
                tempModel.Scale(sphereRadi[i]);
                tempModel2.Scale(sphereRadi[i]);
                models.Add(tempModel);
                models.Add(tempModel2);
            }

            double[,] face = new double[4,3];

            face[0, 0] = models[0].Vertices[0, 0];
            face[0, 1] = models[0].Vertices[0, 1];
            face[0, 2] = models[0].Vertices[0, 2];

            face[1, 0] = models[0].Vertices[1, 0];
            face[1, 1] = models[0].Vertices[1, 1];
            face[1, 2] = models[0].Vertices[1, 2];

            face[2, 0] = models[2].Vertices[1, 0];
            face[2, 1] = models[2].Vertices[1, 1];
            face[2, 2] = models[2].Vertices[1, 2];

            face[3, 0] = models[2].Vertices[0, 0];
            face[3, 1] = models[2].Vertices[0, 1];
            face[3, 2] = models[2].Vertices[0, 2];

            var exportModel = new ExportModel()
            {
                faces = new int[1][] { new int[] { 0, 1, 2, 3 } },
                vertices = ArrayHelper.ToJaggedArray(face),
                edges = new int[1][] { new int[] { 0, 0 } },
                nEdges = 1,
                nVertices = 4,
                nFaces = 1,
                color = new int[] { 255, 0 , 128 }
            };

            Model.ExportModel("C:\\ModelExports\\sphere_panel.json", exportModel);
            Model spherePanel = new Model("C:\\ModelExports\\sphere_panel.json", new double[] {0, 0, 0});
            //spherePanel.Scale(3, 3, 3);
            models.Add(spherePanel);

            var coicles = GCC.AddModels(models);




            GCC.AddMethod( () =>
            {
                red = ColorHelper.IncrementRgbByte(red, (byte)4, ref redIncrease);
                blue = ColorHelper.IncrementRgbByte(blue, (byte)16, ref blueIncrease);
                green = ColorHelper.IncrementRgbByte(green, (byte)8, ref greenIncrease);

                //sphereModel.RotateY(.1);

                coicles.RotateX(.01);
                coicles.RotateY(.04);
            });
            GCC.Start();

            
            var one = 1;
        }


    }
    
}
