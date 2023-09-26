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

        public void DrawMeshes()
        {
            foreach(var mesh in Meshes)
            {
                mesh.DrawMesh(this);
            }
        }

        public void Clear()
        {
            MainStage.Children.Clear();
        }


        
    }
}
