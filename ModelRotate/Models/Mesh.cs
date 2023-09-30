using GraphicsCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;

namespace ModelRender.Models
{
    public class Mesh
    {
        public List<Model> Models;

        private double[] _centroid
        {
            get { return CalculateCentroid(); }
        }

        public Mesh()
        {
            Models = new List<Model>();
        }

        public Mesh(List<Model> models)
        {
            Models = models;
        }

        public Model AddModel(Model model)
        {
            Models.Add(model);
            return model;
        }

        public void DrawMesh(GraphicContextControl contextControl)
        {
            foreach(var model in Models)
            {
                model.DrawFaces(contextControl);
            }
        }

        public double[] CalculateCentroid()
        {
            double sumx = 0;
            double sumy = 0;
            double sumz = 0;
            foreach (Model model in  Models)
            {
                sumx += model.Centroid[0];
                sumy += model.Centroid[1];
                sumz += model.Centroid[2];
            }
            return new double[] { (sumx/Models.Count), (sumy/Models.Count), (sumz/Models.Count) };
        }

        public void Translate(double X, double Y, double Z)
        {
            
        }

        public void RotateX(double radians)
        {
            var initialCentroid = CalculateCentroid();
            foreach (Model model in Models)
            {
                model.RotateX(radians, initialCentroid);
            }
        }
        public void RotateY(double radians)
        {
            var initialCentroid = CalculateCentroid();
            foreach (Model model in Models)
            {
                model.RotateY(radians, initialCentroid);
            }
        }
        public void RotateZ(double radians)
        {
            var initialCentroid = CalculateCentroid();
            foreach (Model model in Models)
            {
                model.RotateZ(radians, initialCentroid);
            }
        }

        public void SetColorAllModels(int R, int G, int B)
        {
            foreach(Model model in Models)
            {
                model.SetColor(R, G, B);  
            }
        }

    }
}
