using System;
using System.Windows;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using sysMedia = System.Windows.Media;

namespace WpfApplication1
{
    public class Section3DVisual3D : ModelVisual3D
    {
        // Using a DependencyProperty as the backing store for Rectangle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RectangleProperty =
            DependencyProperty.Register("Rectangle", typeof(Rect3D), typeof(Section3DVisual3D),
                new PropertyMetadata(new Rect3D(0, 0, 0, 1, 1, 1), ModelChanged));

        // Using a DependencyProperty as the backing store for SectionBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SectionBrushProperty =
            DependencyProperty.Register("SectionBrush", typeof(sysMedia.Brush), typeof(Section3DVisual3D),
                new PropertyMetadata(sysMedia.Brushes.Blue, ModelChanged));

        // Using a DependencyProperty as the backing store for SectionPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SectionPositionProperty =
            DependencyProperty.Register("SectionPosition", typeof(double), typeof(Section3DVisual3D),
                new PropertyMetadata(0.0, ModelChanged));

        public static readonly DependencyProperty CurrentDirectionProperty =
            DependencyProperty.Register("CurrentDirection", typeof(FrameDirection), typeof(Section3DVisual3D),
                new PropertyMetadata(FrameDirection.Left, ModelChanged));

        private readonly ModelVisual3D _visualChild;
        private double _maxLength;

        public Section3DVisual3D()
        {
            _visualChild = new ModelVisual3D();
            Children.Add(_visualChild);
        }

        public Rect3D Rectangle
        {
            get { return (Rect3D)GetValue(RectangleProperty); }
            set
            {
                SetValue(RectangleProperty, value);
                _maxLength = Math.Max(Math.Max(value.SizeX, value.SizeY), value.SizeZ);
            }
        }

        public double SectionPosition
        {
            get { return (double)GetValue(SectionPositionProperty); }
            set { SetValue(SectionPositionProperty, value); }
        }

        public sysMedia.Brush SectionBrush
        {
            get { return (sysMedia.Brush)GetValue(SectionBrushProperty); }
            set { SetValue(SectionBrushProperty, value); }
        }

        public FrameDirection CurrentDirection
        {
            get { return (FrameDirection)GetValue(CurrentDirectionProperty); }
            set { SetValue(CurrentDirectionProperty, value); }
        }

        private static void ModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Section3DVisual3D)d).UpdateModel();
        }

        private void UpdateModel()
        {
            _visualChild.Content = CreateModel();
        }

        private Model3D CreateModel()
        {
            var plotModel = new Model3DGroup();

            var axesMeshBuilder = new MeshBuilder();

            axesMeshBuilder.AddBoundingBox(GetSampleImageBox(), 0.01);

            plotModel.Children.Add(new GeometryModel3D(axesMeshBuilder.ToMesh(), Materials.Yellow));


            switch (CurrentDirection)
            {
                case FrameDirection.Top:
                    CreateTopSlice(plotModel);
                    break;
                case FrameDirection.Front:
                    CreateFrontSlice(plotModel);
                    break;
                case FrameDirection.Left:
                    CreateLeftSlice(plotModel);
                    break;
                default:
                    throw new InvalidOperationException("Can not process the direction.");
            }


            return plotModel;
        }

        private void CreateFrontSlice(Model3DGroup plotModel)
        {
            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(SectionPosition, 0, Rectangle.SizeZ/_maxLength),
                    new Point3D(SectionPosition, Rectangle.SizeY/_maxLength, Rectangle.SizeZ/_maxLength),
                    new Point3D(SectionPosition, 0, 0),
                    new Point3D(SectionPosition, Rectangle.SizeY/_maxLength, 0)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(GetSectionImageBrsh())));


            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(SectionPosition, Rectangle.SizeY/_maxLength, Rectangle.SizeZ/_maxLength),
                    new Point3D(SectionPosition, 0, Rectangle.SizeZ/_maxLength),
                    new Point3D(SectionPosition, Rectangle.SizeY/_maxLength, 0),
                    new Point3D(SectionPosition, 0, 0)
                }, 2);

            var bgBrush = FilpImageBrush();
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));
        }

        private void CreateLeftSlice(Model3DGroup plotModel)
        {
            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, SectionPosition, Rectangle.SizeZ/_maxLength),
                    new Point3D(Rectangle.SizeX/_maxLength, SectionPosition, Rectangle.SizeZ/_maxLength),
                    new Point3D(0, SectionPosition, 0),
                    new Point3D(Rectangle.SizeX/_maxLength, SectionPosition, 0)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(GetSectionImageBrsh())));


            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(Rectangle.SizeX/_maxLength, SectionPosition, Rectangle.SizeZ/_maxLength),
                    new Point3D(0, SectionPosition, Rectangle.SizeZ/_maxLength),
                    new Point3D(Rectangle.SizeX/_maxLength, SectionPosition, 0),
                    new Point3D(0, SectionPosition, 0)
                }, 2);

            var bgBrush = FilpImageBrush();
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));
        }

        private void CreateTopSlice(Model3DGroup plotModel)
        {
            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, Rectangle.SizeY/_maxLength, SectionPosition),
                    new Point3D(Rectangle.SizeX/_maxLength, Rectangle.SizeY/_maxLength, SectionPosition),
                    new Point3D(0, 0, SectionPosition),
                    new Point3D(Rectangle.SizeX/_maxLength, 0, SectionPosition)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(GetSectionImageBrsh())));


            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(Rectangle.SizeX/_maxLength, Rectangle.SizeY/_maxLength, SectionPosition),
                    new Point3D(0, Rectangle.SizeY/_maxLength, SectionPosition),
                    new Point3D(Rectangle.SizeX/_maxLength, 0, SectionPosition),
                    new Point3D(0, 0, SectionPosition)
                }, 2);

            var bgBrush = FilpImageBrush();
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));
        }

        private sysMedia.Brush FilpImageBrush()
        {
            var bgBrush = GetSectionImageBrsh().Clone();
            var transform = sysMedia.Matrix.Identity;
            transform.RotateAt(180, 0.5, 0.5);
            transform.ScaleAt(1, -1, 0.5, 0.5);
            if (bgBrush is sysMedia.ImageBrush)
            {
                bgBrush.RelativeTransform = new sysMedia.MatrixTransform(transform);
            }
            return bgBrush;
        }

        private sysMedia.Brush GetSectionImageBrsh()
        {
            return SectionBrush;
        }

        private Rect3D GetSampleImageBox()
        {
            _maxLength = Math.Max(Math.Max(Rectangle.SizeX, Rectangle.SizeY), Rectangle.SizeZ);
            var sampleImageBox = new Rect3D(0, 0, 0, Rectangle.SizeX / _maxLength, Rectangle.SizeY / _maxLength,
                Rectangle.SizeZ / _maxLength);
            return sampleImageBox;
        }
    }

    public enum FrameDirection
    {
        Front,
        Top,
        Left
    }
}