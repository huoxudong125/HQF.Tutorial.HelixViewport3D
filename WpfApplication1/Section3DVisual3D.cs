using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApplication1
{
    public class Section3DVisual3D : ModelVisual3D
    {


        public Rect3D Rectangle
        {
            get { return (Rect3D)GetValue(RectangleProperty); }
            set { SetValue(RectangleProperty, value); }
        }

        public double SectionPosition
        {
            get { return (double)GetValue(SectionPositionProperty); }
            set { SetValue(SectionPositionProperty, value); }
        }

        public Brush SectionBrush
        {
            get { return (Brush)GetValue(SectionBrushProperty); }
            set { SetValue(SectionBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rectangle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RectangleProperty =
            DependencyProperty.Register("Rectangle", typeof(Rect3D), typeof(Section3DVisual3D), new PropertyMetadata(new Rect3D(0, 0, 0, 1, 1, 1), ModelChanged));

        // Using a DependencyProperty as the backing store for SectionBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SectionBrushProperty =
            DependencyProperty.Register("SectionBrush", typeof(Brush), typeof(Section3DVisual3D), new PropertyMetadata(Brushes.Blue, ModelChanged));


        // Using a DependencyProperty as the backing store for SectionPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SectionPositionProperty =
            DependencyProperty.Register("SectionPosition", typeof(double), typeof(Section3DVisual3D), new PropertyMetadata(0.0, ModelChanged));

        private ModelVisual3D visualChild;

        public Section3DVisual3D()
        {
            visualChild = new ModelVisual3D();
            Children.Add(visualChild);
        }

        private static void ModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Section3DVisual3D)d).UpdateModel();
        }

        private void UpdateModel()
        {
            visualChild.Content = CreateModel();
        }

        private Model3D CreateModel()
        {
            var plotModel = new Model3DGroup();

            var axesMeshBuilder = new MeshBuilder();

            axesMeshBuilder.AddBoundingBox(Rectangle, 0.01);

            plotModel.Children.Add(new GeometryModel3D(axesMeshBuilder.ToMesh(), Materials.Yellow));

            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new Point3D[] {
                    new Point3D(0,1,SectionPosition),
                    new Point3D(1,1,SectionPosition),
                    new Point3D(0,0,SectionPosition),
                    new Point3D(1,0,SectionPosition)
                },
                2);

            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(), MaterialHelper.CreateMaterial(SectionBrush)));

            return plotModel;
        }
    }
}
