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
using GraphicsCommon;
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

            var cubeModel = new Model("..\\..\\..\\Models\\cube_model.json", new double[] { 0, 0, 13 });
            var plusModel = new Model("..\\..\\..\\Models\\plus_model.json", new double[] { 0, 0, 13 });
            var plusModel2 = new Model("..\\..\\..\\Models\\plus_model.json", new double[] { 3, 0, 13 });
            //var plusModel = new Model("..\\..\\..\\Models\\plus_model.json");

            GCC.AddModels(new List<Model> { cubeModel } );

            bool redIncrease = true;
            bool greenIncrease = true;
            bool blueIncrease = true;

            byte red = 64;
            byte green = 128;
            byte blue = 200;

            var models = new List<Model> { plusModel2, plusModel };
            


            

            GCC.AddMethod( () =>
            {
                red = ColorHelper.IncrementRgbByte(red, (byte)4, ref redIncrease);
                blue = ColorHelper.IncrementRgbByte(blue, (byte)16, ref blueIncrease);
                green = ColorHelper.IncrementRgbByte(green, (byte)8, ref greenIncrease);

                cubeModel.RotateY((5 * Math.PI) / 180);
            });
            GCC.Start();

            
            var one = 1;
        }


    }
    
}
