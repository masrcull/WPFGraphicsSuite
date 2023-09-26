using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ModelRender.Models
{
    public class GraphicContext
    {
        public Canvas MainStage;
        public double[] Eye;
        public List<Mesh> Meshes;

        public int width;
        public int height;

        GraphicWindow GraphicWindow = new GraphicWindow();

        public SolidColorBrush Background;

        GraphicContext()
        {
            MainStage = new Canvas();
            Eye = new double[] { 0, 0, 0 };
            Meshes = new List<Mesh>();
            width = 800;
            height = 800;
            MainStage.Width = width; 
            MainStage.Height = height;
            Background = new SolidColorBrush(Colors.Black);

        }



    }
}
