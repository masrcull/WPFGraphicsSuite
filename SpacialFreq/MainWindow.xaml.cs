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

        public MainWindow()
        {
            InitializeComponent();
            mainStage = FindName("MainStage") as Canvas;
            var Eye = new double[] { 0, 0, 0 };

            var cubeModel = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", mainStage, new double[] { 0, 0, 13 });
            cubeModel.Scale(new double[] { 35, 1, 0 });
            cubeModel.Translate(new double[] { 0, 16.4, 0 });

            var greyCube = new Model("..\\..\\..\\..\\ModelRotate\\Models\\cube_model.json", mainStage, new double[] { 0, 0, 13 });
            greyCube.Scale(new double[] { 35, 17, 0 });
            
            greyCube.Translate(new double[] { 0, -9, 0 });

            greyCube.DrawFaces(Brushes.Gray, mainStage, Eye);
            cubeModel.DrawFaces(Brushes.White, mainStage, Eye);
            

        }
    }
}
