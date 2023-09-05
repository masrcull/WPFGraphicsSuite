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
        public Canvas MainStage;

        public MainWindow()
        {
            InitializeComponent();
            MainStage = FindName("MainStage") as Canvas;
            var Eye = new double[] { 0, 0, 0 };

            var cubeModel = new Model("", MainStage, new double[] { 0, 0, 13 });
            //cubeModel.DrawFaces''

        }
    }
}
