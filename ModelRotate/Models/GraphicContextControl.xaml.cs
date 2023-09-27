﻿using System;
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
        public double[] Eye;
        public List<Mesh> Meshes;

        public int Width;
        public int Height;

        GraphicWindow GraphicWindow = new GraphicWindow();

        public SolidColorBrush Background;

        private DispatcherTimer _timer;
        private List<Action> _actions;

        

        private void OnTick(object sender, EventArgs e)
        {
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
            Eye = new double[] { 0, 0, 0 };
            Meshes = new List<Mesh>();
            Width = 800;
            Height = 800;
            MainStage.Width = Width;
            MainStage.Height = Height;
            Background = new SolidColorBrush(Colors.Black);

            _actions = new List<Action>();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(16);
            _timer.Tick += OnTick;


        }

        public Model AddModel(string filePath, double X, double Y, double Z)
        {
            var model = new Model(filePath, new double[] { X, Y, Z });
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

        public void DrawMeshes()
        {
            //Meshes.Models = Meshes.OrderByDescending(obj => obj.CalculateCentroid()[2]).ToList();
            foreach(var mesh in Meshes)
            {
                mesh.Models = mesh.Models.OrderByDescending(obj => obj.Centroid[2]).ToList();
                mesh.DrawMesh(this);
            }
        }

        public void DrawCircle(Point[] points)
        {
            ShapeHelper.DrawPolygon(points, new SolidColorBrush(Colors.HotPink), this);
        }

        public void CreateCircleModel(string filePath, double radius, int R = 255, int G = 255, int B = 255)
        {
            var points = ShapeHelper.GenerateCirclePoints(radius, 16);
            double[,] vertices = new double[points.Length,3];
            int[] face = new int[points.Length];

            for(int i= 0; i < points.Length; i++)
            {
                vertices[i,0] = points[i][0];
                vertices[i, 1] = points[i][1];
                vertices[i, 2] = 0;
            }

            for(int i = 0;  i < points.Length; i++)
            {
                face[i] = i;
            }

            var exportModel = new ExportModel
            {
                vertices = ArrayHelper.ToJaggedArray(vertices),
                edges = new int[][] { new int[] { 0, 0 } },
                faces = new int[][] { face },
                nVertices = (vertices.Length / 3),
                nEdges = 1,
                nFaces = 1,
                color = new int[] { R, G, B}

            };

            Model.ExportModel(filePath, exportModel);


        }

        public void Clear()
        {
            MainStage.Children.Clear();
        }


        
    }
}
