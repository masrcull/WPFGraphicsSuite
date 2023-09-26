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

namespace ShadowTrajectory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Canvas mainStage = null;
        double[] Eye = new double[] { 0, 0, 0 };

        public MainWindow()
        {
            InitializeComponent();
            mainStage = this.FindName("MainStage") as Canvas;

            var tableBottom = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", mainStage, new double[] {0,0,-15});
            var tableSideRight = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", mainStage, new double[] { 0, 0, -15 });
            tableSideRight.Scale(0.3, 2, 2.5);
            tableSideRight.Translate(-2.5, -1, 0);
            tableBottom.Scale(4.5, 0.3, 2.5);
            //tableBottom.RotateX(10);
            //tableSideRight.RotateX(10);

            var mesh = new Mesh(new List<Model> { tableBottom, tableSideRight });

            tableSideRight.DrawFaces(new SolidColorBrush(Colors.Blue), mainStage, Eye);
            tableBottom.DrawFaces(new SolidColorBrush(Colors.Red), mainStage, Eye);
            


        }
    }
}
