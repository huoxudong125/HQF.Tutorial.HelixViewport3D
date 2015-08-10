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
            DependencyProperty.Register("Rectangle", typeof (Rect3D), typeof (Section3DVisual3D),
                new PropertyMetadata(new Rect3D(0, 0, 0, 1, 1, 1), ModelChanged));

        // Using a DependencyProperty as the backing store for SectionPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SectionPositionProperty =
            DependencyProperty.Register("SectionPosition", typeof (double), typeof (Section3DVisual3D),
                new PropertyMetadata(0.0, ModelChanged));

        // Using a DependencyProperty as the backing store for SectionBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SectionBrushProperty =
            DependencyProperty.Register("SectionBrush", typeof (sysMedia.Brush), typeof (Section3DVisual3D),
                new PropertyMetadata(sysMedia.Brushes.Red, ModelChanged));

        // Using a DependencyProperty as the backing store for SectionBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftBrushProperty =
            DependencyProperty.Register("LeftBrush", typeof (sysMedia.Brush), typeof (Section3DVisual3D),
                new PropertyMetadata(sysMedia.Brushes.Blue, ModelChanged));

        public static readonly DependencyProperty TopBrushProperty =
            DependencyProperty.Register("TopBrushBrush", typeof (sysMedia.Brush), typeof (Section3DVisual3D),
                new PropertyMetadata(sysMedia.Brushes.Green, ModelChanged));

        public static readonly DependencyProperty FrontBrushProperty =
            DependencyProperty.Register("FrontBrush", typeof (sysMedia.Brush), typeof (Section3DVisual3D),
                new PropertyMetadata(sysMedia.Brushes.DarkCyan, ModelChanged));

        public static readonly DependencyProperty RightBrushProperty =
            DependencyProperty.Register("RightBrush", typeof (sysMedia.Brush), typeof (Section3DVisual3D),
                new PropertyMetadata(sysMedia.Brushes.DarkOliveGreen, ModelChanged));

        public static readonly DependencyProperty BackBrushProperty =
            DependencyProperty.Register("BackBrush", typeof (sysMedia.Brush), typeof (Section3DVisual3D),
                new PropertyMetadata(sysMedia.Brushes.DodgerBlue, ModelChanged));

        public static readonly DependencyProperty BottomBrushProperty =
            DependencyProperty.Register("BottomBrush", typeof (sysMedia.Brush), typeof (Section3DVisual3D),
                new PropertyMetadata(sysMedia.Brushes.DarkSalmon, ModelChanged));

        public static readonly DependencyProperty CurrentDirectionProperty =
            DependencyProperty.Register("CurrentDirection", typeof (FrameDirection), typeof (Section3DVisual3D),
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
            get { return (Rect3D) GetValue(RectangleProperty); }
            set
            {
                SetValue(RectangleProperty, value);
                _maxLength = Math.Max(Math.Max(value.SizeX, value.SizeY), value.SizeZ);
            }
        }

        public double SectionPosition
        {
            get { return (double) GetValue(SectionPositionProperty); }
            set { SetValue(SectionPositionProperty, value); }
        }

        public sysMedia.Brush SectionBrush
        {
            get { return (sysMedia.Brush) GetValue(SectionBrushProperty); }
            set { SetValue(SectionBrushProperty, value); }
        }

        public sysMedia.Brush LeftBrush
        {
            get { return (sysMedia.Brush) GetValue(LeftBrushProperty); }
            set { SetValue(LeftBrushProperty, value); }
        }

        public sysMedia.Brush TopBrush
        {
            get { return (sysMedia.Brush) GetValue(TopBrushProperty); }
            set { SetValue(TopBrushProperty, value); }
        }

        public sysMedia.Brush FrontBrush
        {
            get { return (sysMedia.Brush) GetValue(FrontBrushProperty); }
            set { SetValue(FrontBrushProperty, value); }
        }

        public sysMedia.Brush RightBrush
        {
            get { return (sysMedia.Brush) GetValue(RightBrushProperty); }
            set { SetValue(RightBrushProperty, value); }
        }

        public sysMedia.Brush BackBrush
        {
            get { return (sysMedia.Brush) GetValue(BackBrushProperty); }
            set { SetValue(BackBrushProperty, value); }
        }

        public sysMedia.Brush BottomBrush
        {
            get { return (sysMedia.Brush) GetValue(BottomBrushProperty); }
            set { SetValue(BottomBrushProperty, value); }
        }

        public FrameDirection CurrentDirection
        {
            get { return (FrameDirection) GetValue(CurrentDirectionProperty); }
            set { SetValue(CurrentDirectionProperty, value); }
        }

        private static void ModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Section3DVisual3D) d).UpdateModel();
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
                    CreateTopSelectionSlice(plotModel, SectionBrush);
                    CreateTopMinSlice(plotModel, TopBrush);

                    CreateFrontPartSlice(plotModel, FrontBrush, SectionPosition);
                    CreateLeftPartSlice(plotModel, LeftBrush, SectionPosition);
                    break;

                case FrameDirection.Front:
                    CreateFrontSelectionSlice(plotModel, SectionBrush);
                    CreateFrontMinSlice(plotModel, FrontBrush);
                    CreateFrontPartSlice(plotModel, FrontBrush, SectionPosition);
                  
                    break;

                case FrameDirection.Left:
                    CreateLeftSelectionSlice(plotModel, SectionBrush);
                    CreateLeftMinSlice(plotModel, LeftBrush);

                    CreateFrontPartSlice(plotModel, FrontBrush, SectionPosition);
                    CreateTopPartSlice(plotModel, TopBrush, SectionPosition);
                    break;
                default:
                    throw new InvalidOperationException("Can not process the direction.");
            }

            return plotModel;
        }

        private sysMedia.Brush FilpImageBrush(sysMedia.Brush brush)
        {
            var bgBrush = brush.Clone();
            var transform = sysMedia.Matrix.Identity;
            transform.RotateAt(180, 0.5, 0.5);
            transform.ScaleAt(1, -1, 0.5, 0.5);
            if (bgBrush is sysMedia.ImageBrush)
            {
                bgBrush.RelativeTransform = new sysMedia.MatrixTransform(transform);
            }
            return bgBrush;
        }

        private sysMedia.Brush ClipImageBrush(sysMedia.Brush brush, Rect regionRect)
        {
            var imgBrush = brush.Clone() as sysMedia.ImageBrush;

            if (imgBrush != null)
            {
                imgBrush.Viewbox = regionRect;
                return imgBrush;
            }

            return new sysMedia.SolidColorBrush(sysMedia.Colors.Transparent);
        }

        private Rect3D GetSampleImageBox()
        {
            _maxLength = Math.Max(Math.Max(Rectangle.SizeX, Rectangle.SizeY), Rectangle.SizeZ);
            var sampleImageBox = new Rect3D(0, 0, 0, Rectangle.SizeX/_maxLength, Rectangle.SizeY/_maxLength,
                Rectangle.SizeZ/_maxLength);
            return sampleImageBox;
        }

        #region selection

        private void CreateFrontSelectionSlice(Model3DGroup plotModel, sysMedia.Brush brush)
        {
            CreateFrontSlice(plotModel, brush, SectionPosition);
        }

        private void CreateLeftSelectionSlice(Model3DGroup plotModel, sysMedia.Brush brush)
        {
            CreateLeftSlice(plotModel, brush, SectionPosition);
        }

        private void CreateTopSelectionSlice(Model3DGroup plotModel, sysMedia.Brush brush)
        {
            CreateTopSlice(plotModel, brush, SectionPosition);
        }

        #endregion selection

        #region Create all facets

        private void CreateFrontMinSlice(Model3DGroup plotModel, sysMedia.Brush brush)
        {
            CreateFrontSlice(plotModel, brush, 0); //z=0 for helix top ,Raw Front==Helix Top;
        }

        private void CreateLeftMinSlice(Model3DGroup plotModel, sysMedia.Brush brush)
        {
            CreateLeftSlice(plotModel, brush, 0);
        }

        private void CreateTopMinSlice(Model3DGroup plotModel, sysMedia.Brush brush)
        {
            CreateTopSlice(plotModel, brush, 0);
        }

        private void CreateFrontMaxSlice(Model3DGroup plotModel, sysMedia.Brush brush)
        {
            CreateFrontSlice(plotModel, brush, Rectangle.SizeZ/_maxLength);
        }

        private void CreateTopMaxSlice(Model3DGroup plotModel, sysMedia.Brush brush)
        {
            CreateTopSlice(plotModel, brush, Rectangle.SizeX/_maxLength);
        }

        private void CreateLeftMaxSlice(Model3DGroup plotModel, sysMedia.Brush brush)
        {
            CreateLeftSlice(plotModel, brush, Rectangle.SizeY/_maxLength);
        }

        /// <summary>
        ///     Raw Front==Helix Top;
        /// </summary>
        /// <param name="plotModel"></param>
        /// <param name="brush"></param>
        /// <param name="position"></param>
        private void CreateFrontSlice(Model3DGroup plotModel, sysMedia.Brush brush, double position)
        {
            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(position, 0, Rectangle.SizeZ/_maxLength),
                    new Point3D(position, Rectangle.SizeY/_maxLength, Rectangle.SizeZ/_maxLength),
                    new Point3D(position, 0, 0),
                    new Point3D(position, Rectangle.SizeY/_maxLength, 0)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(brush)));

            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(position, Rectangle.SizeY/_maxLength, Rectangle.SizeZ/_maxLength),
                    new Point3D(position, 0, Rectangle.SizeZ/_maxLength),
                    new Point3D(position, Rectangle.SizeY/_maxLength, 0),
                    new Point3D(position, 0, 0)
                }, 2);

            var bgBrush = FilpImageBrush(brush);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));
        }

        /// <summary>
        ///     Raw  Left==Helix Front
        /// </summary>
        /// <param name="plotModel"></param>
        /// <param name="brush"></param>
        /// <param name="position"></param>
        private void CreateLeftSlice(Model3DGroup plotModel, sysMedia.Brush brush, double position)
        {
            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, position, Rectangle.SizeZ/_maxLength),
                    new Point3D(Rectangle.SizeX/_maxLength, position, Rectangle.SizeZ/_maxLength),
                    new Point3D(0, position, 0),
                    new Point3D(Rectangle.SizeX/_maxLength, position, 0)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(brush)));

            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(Rectangle.SizeX/_maxLength, position, Rectangle.SizeZ/_maxLength),
                    new Point3D(0, position, Rectangle.SizeZ/_maxLength),
                    new Point3D(Rectangle.SizeX/_maxLength, position, 0),
                    new Point3D(0, position, 0)
                }, 2);

            var bgBrush = FilpImageBrush(brush);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));
        }

        /// <summary>
        ///     Raw Top==Helix left
        /// </summary>
        /// <param name="plotModel"></param>
        /// <param name="brush"></param>
        /// <param name="position"></param>
        private void CreateTopSlice(Model3DGroup plotModel, sysMedia.Brush brush, double position)
        {
            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, Rectangle.SizeY/_maxLength, position),
                    new Point3D(Rectangle.SizeX/_maxLength, Rectangle.SizeY/_maxLength, position),
                    new Point3D(0, 0, position),
                    new Point3D(Rectangle.SizeX/_maxLength, 0, position)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(brush)));

            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(Rectangle.SizeX/_maxLength, Rectangle.SizeY/_maxLength, position),
                    new Point3D(0, Rectangle.SizeY/_maxLength, position),
                    new Point3D(Rectangle.SizeX/_maxLength, 0, position),
                    new Point3D(0, 0, position)
                }, 2);

            var bgBrush = FilpImageBrush(brush);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));
        }

        private void CreateFrontPartSlice(Model3DGroup plotModel, sysMedia.Brush brush, double position)
        {
            var leftImageBrush = ClipImageBrush(brush, new Rect(0, 0, Rectangle.SizeX / _maxLength, position));
            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, 0, Rectangle.SizeZ/_maxLength),
                    new Point3D(position, 0, Rectangle.SizeZ/_maxLength),
                    new Point3D(0, 0, 0),
                    new Point3D(position, 0, 0)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(leftImageBrush)));

            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(position, Rectangle.SizeY/_maxLength, Rectangle.SizeZ/_maxLength),
                    new Point3D(0, Rectangle.SizeY/_maxLength, Rectangle.SizeZ/_maxLength),
                    new Point3D(position, Rectangle.SizeY/_maxLength, 0),
                    new Point3D(0, Rectangle.SizeY/_maxLength, 0)
                }, 2);

            var bgBrush = FilpImageBrush(leftImageBrush);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));


            var topImageBrush = ClipImageBrush(brush, new Rect(0, 0, Rectangle.SizeZ / _maxLength, position));
            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, Rectangle.SizeY/_maxLength, Rectangle.SizeZ/_maxLength),
                    new Point3D(position, Rectangle.SizeY/_maxLength, Rectangle.SizeZ/_maxLength),
                    new Point3D(0, 0, Rectangle.SizeZ/_maxLength),
                    new Point3D(position, 0, Rectangle.SizeZ/_maxLength)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(topImageBrush)));

            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(position, Rectangle.SizeY/_maxLength, 0),
                    new Point3D(0, Rectangle.SizeY/_maxLength, 0),
                    new Point3D(position, 0, 0),
                    new Point3D(0, 0, 0)
                }, 2);

            bgBrush = FilpImageBrush(topImageBrush);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));
          
        }

        private void CreateLeftPartSlice(Model3DGroup plotModel, sysMedia.Brush brush, double position)
        {
            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(position, 0, Rectangle.SizeZ/_maxLength),
                    new Point3D(position, Rectangle.SizeY/_maxLength, Rectangle.SizeZ/_maxLength),
                    new Point3D(position, 0, 0),
                    new Point3D(position, Rectangle.SizeY/_maxLength, 0)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(brush)));

            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(position, Rectangle.SizeY/_maxLength, Rectangle.SizeZ/_maxLength),
                    new Point3D(position, 0, Rectangle.SizeZ/_maxLength),
                    new Point3D(position, Rectangle.SizeY/_maxLength, 0),
                    new Point3D(position, 0, 0)
                }, 2);

            var bgBrush = FilpImageBrush(brush);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));
        }

        private void CreateTopPartSlice(Model3DGroup plotModel, sysMedia.Brush brush, double position)
        {
           
        }

        #endregion Create all facets
    }

    public enum FrameDirection
    {
        Front,
        Top,
        Left
    }
}