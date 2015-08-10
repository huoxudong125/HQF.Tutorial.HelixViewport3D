using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace WpfApplication1
{
    public class SampleImageVisual3D : ModelVisual3D
    {
        // Using a DependencyProperty as the backing store for SectionPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SectionPositionProperty =
            DependencyProperty.Register("SectionPosition", typeof (double), typeof (SampleImageVisual3D),
                new PropertyMetadata(0.0, ModelChanged));

        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof (double),
            typeof (SampleImageVisual3D), new PropertyMetadata(0.0, ModelChanged));

        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof (double),
            typeof (SampleImageVisual3D), new PropertyMetadata(0.0, ModelChanged));

        public static readonly DependencyProperty ZProperty = DependencyProperty.Register("Z", typeof (double),
            typeof (SampleImageVisual3D), new PropertyMetadata(0.0, ModelChanged));

        public static readonly DependencyProperty DirectionDependencyProperty = DependencyProperty.Register("Direction"
            , typeof (Direction), typeof (SampleImageVisual3D), new PropertyMetadata(Direction.Front, ModelChanged));

        private readonly ModelVisual3D visualChild;
        private double _maxLength;

        public SampleImageVisual3D()
        {
            SectionBrush = new SolidColorBrush(Colors.BlueViolet);
            visualChild = new ModelVisual3D();
            Children.Add(visualChild);
        }

        public double X
        {
            get { return (double) GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public double Y
        {
            get { return (double) GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public double Z
        {
            get { return (double) GetValue(ZProperty); }
            set { SetValue(ZProperty, value); }
        }

        public double SectionPosition
        {
            get { return (double) GetValue(SectionPositionProperty); }
            set { SetValue(SectionPositionProperty, value); }
        }

        public Brush SectionBrush { get; set; }

        private static void ModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sampleImageVisual3D = d as SampleImageVisual3D;

            if (sampleImageVisual3D != null)
            {
                sampleImageVisual3D._maxLength = Math.Max(Math.Max(sampleImageVisual3D.X, sampleImageVisual3D.Y),
                    sampleImageVisual3D.Z);
                sampleImageVisual3D.visualChild.Content = sampleImageVisual3D.CreateModel();
            }
        }

        private Model3D CreateModel()
        {
            var plotModel = new Model3DGroup();

            var axesMeshBuilder = new MeshBuilder();
            axesMeshBuilder.AddBoundingBox(GetSampleImageBox(), 0.01);

            plotModel.Children.Add(new GeometryModel3D(axesMeshBuilder.ToMesh(), Materials.Yellow));

            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, Y/_maxLength, SectionPosition),
                    new Point3D(X/_maxLength, Y/_maxLength, SectionPosition),
                    new Point3D(0, 0, SectionPosition),
                    new Point3D(X/_maxLength, 0, SectionPosition)
                }, 2);


            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(X/_maxLength, Y/_maxLength, SectionPosition),
                    new Point3D(0, Y/_maxLength, SectionPosition),
                    new Point3D(X/_maxLength, 0, SectionPosition),
                    new Point3D(0, 0, SectionPosition)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(SectionBrush)));

            return plotModel;
        }

        private Rect3D GetSampleImageBox()
        {
            var sampleImageBox = new Rect3D(0, 0, 0, X/_maxLength, Y/_maxLength, Z/_maxLength);
            return sampleImageBox;
        }
    }

    internal enum Direction
    {
        Front,
        Left,
        Top
    }
}