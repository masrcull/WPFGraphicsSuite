using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

            GCC.CreateQuarterCircleModel("C:\\ModelExports\\quarter_circle.json", 16);

           // GCC.DirectionalLightEnabled = false;

            var sphereRadi = ShapeHelper.GenerateSphereRadi(16);
            var sphereVerticalSpace = ShapeHelper.GenerateSphereVerticalSpace(16);

            //var sphereModel = new Model("C:\\ModelExports\\sphereplz.json", new double[] { 0, 0,4 });
            var quarterModel1 = new Model("C:\\ModelExports\\quarter_circle.json", new Vector3( 0, 0, 4 ));

            
            var plusModel = new Model("..\\..\\..\\Models\\plus_model.json", new Vector3(0, 0, 13));
            var plusModel2 = new Model("..\\..\\..\\Models\\plus_model.json", new Vector3(3, 0, 13));
            //var plusModel = new Model("..\\..\\..\\Models\\plus_model.json");

            //GCC.AddModels(new List<Model> { sphereModel } );

            bool redIncrease = true;
            bool greenIncrease = true;
            bool blueIncrease = true;

            byte red = 64;
            byte green = 128;
            byte blue = 200;


            var models = new List<Model>();
            var cubeys = new List<Model>();
            float verticalSpace = (float)(1 /64.0);

            for (int i = 0; i < sphereRadi.Length; i++)
            {
                var hellowurld = verticalSpace * i;
                Model tempModel = new Model("C:\\ModelExports\\circley.json", new Vector3( 0, sphereVerticalSpace[i], 4 ));
                Model tempModel2 = new Model("C:\\ModelExports\\circley.json", new Vector3(0, (verticalSpace * i), 4));
                tempModel.RotateX(LinearAlgebra.DegreeToRadians(90));
                tempModel2.RotateX(LinearAlgebra.DegreeToRadians(270));
                tempModel.Scale(sphereRadi[i]);
                tempModel2.Scale(sphereRadi[i]);
                models.Add(tempModel);
                //models.Add(tempModel2);
            }

            var cubeModel = new Model("..\\..\\..\\Models\\cube_model.json", models[0].Vertices[0]);
            var cubeModel2 = new Model("..\\..\\..\\Models\\cube_model.json", models[0].Vertices[1]);

            //cubeModel.Scale(.05f);
            //cubeModel2.Scale(.05f);

            cubeModel.SetColor(0,0,200);

            cubeys.Add(cubeModel);
            //cubeys.Add(cubeModel2);

            //List<List<Vector3>> Faces = new List<List<Vector3>>();
            var coordMap = new CoordinateMapper();
            int faceMapCount = 0;
            List <int[]> faceMap = new List<int[]>();

            List<Vector3> faces = new List<Vector3>();





            for (int i = 0; i < models.Count - 1; i++)
            {

                for (int j = 0; j < models[i].nVertices; j++)
                {
                    var currentFaceMap = new int[4];

                    //if (i == models.Count - 2)
                    //{
                    //    currentFaceMap = new int[3];

                    //    AddUnique(faces, new Vector3(models.Last().Centroid.X, models.Last().Centroid.Y, models.Last().Centroid.Z));
                    //    currentFaceMap[0] = coordMap.GetOrAdd(models.Count + 1, 0);

                    //    AddUnique(faces, models[i + 1].Vertices[(j + 1) % models[i + 1].Vertices.Count]);
                    //    currentFaceMap[1] = coordMap.GetOrAdd(i + 1, j);

                    //    AddUnique(faces, models.Last().Vertices[j]);
                    //    currentFaceMap[2] = coordMap.GetOrAdd(i, j);

                        

                        


                    //    faceMap.Add(currentFaceMap);
                    //}
                    //else
                    //{
                        

                        AddUnique(faces, models[i + 1].Vertices[j]);
                        currentFaceMap[0] = coordMap.GetOrAdd(i + 1, j);

                        AddUnique(faces, models[i].Vertices[j]);
                        currentFaceMap[1] = coordMap.GetOrAdd(i, j);

                        AddUnique(faces, models[i].Vertices[(j + 1) % models[i].Vertices.Count]);
                        currentFaceMap[2] = coordMap.GetOrAdd(i, (j + 1) % models[i].Vertices.Count);

                        AddUnique(faces, models[i + 1].Vertices[(j + 1) % models[i + 1].Vertices.Count]);
                        currentFaceMap[3] = coordMap.GetOrAdd(i + 1, (j + 1) % models[i + 1].Vertices.Count);


                        faceMap.Add(currentFaceMap);
                    //}

                    

                }
                
            }



            //for (int i = 0; i < models.Count - 4; i++)
            //{

            //    for (int j = 0; j < models[i].Vertices.Count; j++)
            //    {
            //        var currentFaceMap = new int[4];

            //        faces.Add(models[i].Vertices[((j + 1) % models[i].Vertices.Count)]);
            //        currentFaceMap[0] = coordMap.GetOrAdd((i), ((j + 1) % models[i].Vertices.Count));

            //        faces.Add(models[i].Vertices[j]);
            //        currentFaceMap[1] = coordMap.GetOrAdd((i), j);

            //        faces.Add(models[(i + 1)].Vertices[j]);
            //        currentFaceMap[2] = coordMap.GetOrAdd((i + 1), j);

            //        faces.Add(models[(i + 1)].Vertices[(j + 1) % models[i + 1].Vertices.Count]);
            //        currentFaceMap[3] = coordMap.GetOrAdd((i + 1), (j + 1) % models[i + 1].Vertices.Count);





            //        faceMap.Add(currentFaceMap);

            //    }

            //    //Faces.Add(currentFace);
            //}

            var exportModel = new ExportModel
            {
                nEdges = 1,
                edges = new int[][] { new int[] { 0, 0 } },
                nVertices = coordMap.Count,
                vertices = ArrayHelper.Vec3ToFloat2DArray(faces),
                nFaces = faceMap.Count,
                faces = faceMap.ToArray(),
                color = new int[] { 128, 0, 255}

            };

            Model.ExportModel("C:\\ModelExports\\spherical.json", exportModel);
            Model spherical = new Model("C:\\ModelExports\\spherical.json");
            Model spherical2 = new Model("C:\\ModelExports\\spherical.json");

            spherical2.Translate(0,-1f,0);
            spherical2.RotateX(Math.PI );

            models.Add(spherical);
            models.Add(spherical2);

            var coicles = GCC.AddModels(models);



            //var cubeyMesh = GCC.AddModels(cubeys);


            //coicles.RotateX(.1);
            //coicles.RotateY(2.5);

            coicles.Scale(4,3.7f,4);

            GCC.AddMethod( () =>
            {
                red = ColorHelper.IncrementRgbByte(red, (byte)4, ref redIncrease);
                blue = ColorHelper.IncrementRgbByte(blue, (byte)16, ref blueIncrease);
                green = ColorHelper.IncrementRgbByte(green, (byte)8, ref greenIncrease);

                //sphereModel.RotateY(.1);
                //cubeyMesh.RotateY(.02);
                coicles.RotateX(.01);
                coicles.RotateY(.04);
            });
            GCC.Start();

            
            var one = 1;
        }

        public void AddUnique(List<Vector3> list, Vector3 item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }

    }
    
}
