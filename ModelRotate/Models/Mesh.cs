using GraphicsCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Numerics;

namespace ModelRender.Models
{
    public class Mesh
    {
        public List<Model> Models;

        private Vector3 _centroid
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

        public Vector3 CalculateCentroid()
        {
            float sumx = 0;
            float sumy = 0;
            float sumz = 0;
            foreach (Model model in  Models)
            {
                sumx += model.Centroid.X;
                sumy += model.Centroid.Y;
                sumz += model.Centroid.Z;
            }
            return new Vector3 ( (sumx/Models.Count), (sumy/Models.Count), (sumz/Models.Count) );
        }

        public void Translate(double X, double Y, double Z)
        {
            
        }

        public void Scale(float X, float Y, float Z)
        {
            foreach (var model in Models)
            {
                model.Scale(X, Y, Z);
            }
        }

        public async void Scale(float scalar)
        {
            foreach (var model in Models)
            {
                model.Scale(scalar, scalar, scalar);
            }
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
