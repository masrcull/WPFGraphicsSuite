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
using System.Xml.Serialization;
using GraphicsCommon;
using ModelRender.Helpers;
using ModelRender.Models;

namespace ModelRender.Models
{
    /// <summary>
    /// Interaction logic for GraphicContextControl.xaml
    /// </summary>
    public partial class GraphicContextControl : UserControl
    {
        private DispatcherTimer timer;
        //public Canvas MainStage;
        public Vector3 Eye;
        public Vector3 DirectionLight;
        public List<Mesh> Meshes;

        public int Width;
        public int Height;

        public bool DirectionalLightEnabled = false;

        GraphicWindow GraphicWindow = new GraphicWindow();

        public SolidColorBrush Background;

        private DispatcherTimer _timer;
        private List<Action> _actions;

        public bool ClearOnDraw { get; set; } = true;

        private void OnTick(object sender, EventArgs e)
        {
            if (ClearOnDraw)
                Clear();
            foreach (var action in _actions)
            {
                action.Invoke();
            }
            DrawMeshes();
        }

        public void AddMethod(Action action)
        {
            _actions.Add(action);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }


        public GraphicContextControl()
        {
            InitializeComponent();
            //MainStage = new Canvas();
            Eye = new Vector3( 0, 0, 0 );
            DirectionLight = new Vector3( 0, 0, 1 );
            Meshes = new List<Mesh>();
            Width = 800;
            Height = 800;
            MainStage.Width = Width;
            MainStage.Height = Height;
            MainStage.Background = new SolidColorBrush(Colors.White);

            _actions = new List<Action>();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(16);
            _timer.Tick += OnTick;


        }

        public Model AddModel(string filePath, float X, float Y, float Z)
        {
            var model = new Model(filePath, new Vector3( X, Y, Z ));
            var mesh = new Mesh(new List<Model> { model });
            Meshes.Add(mesh);
            return model;
        }

        public Mesh AddModels(List<Model> models)
        {
            var mesh = new Mesh(models);
            Meshes.Add(mesh);
            return mesh;
        }

        public Mesh AddMesh(Mesh mesh)
        {
            Meshes.Add(mesh);
            return mesh;
        }

        public void DrawMeshes()
        {
            //Meshes.Models = Meshes.OrderByDescending(obj => obj.CalculateCentroid()[2]).ToList();
            foreach(var mesh in Meshes)
            {
                mesh.Models = mesh.Models.OrderByDescending(obj => obj.Centroid.Z).ToList();
                mesh.DrawMesh(this);
            }
        }

        public void DrawCircle(Point[] points)
        {
            ShapeHelper.DrawPolygon(points, new SolidColorBrush(Colors.HotPink), this);
        }

        public void CreateCircleModel(string filePath, float radius, int R = 255, int G = 255, int B = 255)
        {
            var points = ShapeHelper.GenerateCirclePoints(radius, 16);
            List<Vector3> vertices = new List<Vector3>();
            int[] face = new int[points.Count];

            for(int i= 0; i < points.Count; i++)
            {
                float pointX = (float)points[i].X;
                float pointY = (float)points[i].Y;
                float pointZ = 0;

                vertices.Add(new Vector3(pointX, pointY, pointZ));
            }

            for(int i = 0;  i < points.Count; i++)
            {
                face[i] = i;
            }

            var exportModel = new ExportModel
            {
                vertices = ArrayHelper.Vec3ToFloat2DArray(vertices),
                edges = new int[][] { new int[] { 0, 0 } },
                faces = new int[][] { face },
                nVertices = (vertices.Count / 3),
                nEdges = 1,
                nFaces = 1,
                color = new int[] { R, G, B}

            };

            Model.ExportModel(filePath, exportModel);


        }

        public void CreateQuarterCircleModel(string filePath, float radius, int R = 255, int G = 255, int B = 255)
        {
            var points = ShapeHelper.GenerateQuarterCirclePoints(radius, 4);
            List<Vector3> vertices = new List<Vector3>();
            int[] face = new int[points.Count];

            for (int i = 0; i < points.Count; i++)
            {
                float pointX = points[i].X;
                float pointY = points[i].Y;
                float pointZ = 0;

                vertices.Add(new Vector3(pointX, pointY, pointZ));
            }

            for (int i = 0; i < points.Count; i++)
            {
                face[i] = i;
            }

            var exportModel = new ExportModel
            {
                vertices = ArrayHelper.Vec3ToFloat2DArray(vertices),
                edges = new int[][] { new int[] { 0, 0 } },
                faces = new int[][] { face },
                nVertices = (vertices.Count / 3),
                nEdges = 1,
                nFaces = 1,
                color = new int[] { R, G, B }

            };

            Model.ExportModel(filePath, exportModel);


        }

        public void CreateTriangleModel(string filePath, float radius, int R = 255, int G = 255, int B = 255)
        {
            List<Vector3> vertices = new List<Vector3>();
            int[] face = new int[] { 0, 1, 2 } ;

            vertices.Add(new Vector3((float)0, radius, (float)0));
            vertices.Add(new Vector3(radius, -radius, (float)0));
            vertices.Add(new Vector3(-radius, -radius, (float)0));

            var exportModel = new ExportModel
            {
                vertices = ArrayHelper.Vec3ToFloat2DArray(vertices),
                edges = new int[][] { new int[] { 0, 0 } },
                faces = new int[][] { face },
                nVertices = (3),
                nEdges = 1,
                nFaces = 1,
                color = new int[] { R, G, B }

            };

            Model.ExportModel(filePath, exportModel);

        }

        public void CreateGradientTriangle()
        {
            Polygon triangle = new Polygon();

            // Define the triangle's points
            triangle.Points.Add(new Point(50, 0));
            triangle.Points.Add(new Point(0, 100));
            triangle.Points.Add(new Point(100, 100));

            // Create a RadialGradientBrush
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.5, 0);
            brush.Center = new Point(0.5, 0);
            brush.RadiusX = 0.5;
            brush.RadiusY = 1.0;

            // Define GradientStops for the brush
            brush.GradientStops.Add(new GradientStop(Colors.Red, 0.0));
            brush.GradientStops.Add(new GradientStop(Colors.Green, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Blue, 1.0));

            // Apply the brush to the triangle's Fill
            triangle.Fill = brush;

            // Add the triangle to a parent container (e.g., a Grid named 'LayoutRoot')
            MainStage.Children.Add(triangle);
        }

        public void Clear()
        {
            MainStage.Children.Clear();
        }


        
    }
}
