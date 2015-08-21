using System;
using System.Windows;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using sysMedia = System.Windows.Media;

namespace HQF.HelixViewport3D.Cube
{
    public class Section3DVisual3D : ModelVisual3D
    {
        private readonly ModelVisual3D _visualChild;

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

            _realRect3D = GetSampleImageBox();
            axesMeshBuilder.AddBoundingBox(_realRect3D, 0.01);

            plotModel.Children.Add(new GeometryModel3D(axesMeshBuilder.ToMesh(), Materials.Yellow));

            switch (CurrentDirection)
            {
                case FrameDirection.Top:
                    CreateTopSelectionSlice(plotModel, SectionBrush);
                    CreateTopMaxSlice(plotModel);
                    CreateTopPartSlice(plotModel, SectionPosition);
                    break;

                case FrameDirection.Front:
                    CreateFrontSelectionSlice(plotModel, SectionBrush);
                    CreateFrontMaxSlice(plotModel);
                    CreateFrontPartSlice(plotModel, SectionPosition);

                    break;

                case FrameDirection.Left:
                    CreateLeftSelectionSlice(plotModel, SectionBrush);
                    CreateLeftMaxSlice(plotModel);
                    CreateLeftPartSlice(plotModel, SectionPosition);

                    break;

                default:
                    throw new InvalidOperationException("Can not process the direction.");
            }

            return plotModel;
        }

        private void CreateFrontPartSlice(Model3DGroup plotModel, double currentPosition)
        {
            var position = (1 - currentPosition) * _realRect3D.SizeX;
            if (position < 0.0)
                return;

            #region Left

            var leftImageBrush = ClipImageBrush(LeftBrush, new Rect(0, 0, 1 - currentPosition, 1));
            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, 0, _realRect3D.SizeZ),
                    new Point3D(position, 0, _realRect3D.SizeZ),
                    new Point3D(0, 0, 0),
                    new Point3D(position, 0, 0)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(leftImageBrush)));

            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(position, _realRect3D.SizeY, _realRect3D.SizeZ),
                    new Point3D(0, _realRect3D.SizeY, _realRect3D.SizeZ),
                    new Point3D(position, _realRect3D.SizeY, 0),
                    new Point3D(0, _realRect3D.SizeY, 0)
                }, 2);

            leftImageBrush = ClipImageBrush(RightBrush, new Rect(0, 0, 1 - currentPosition, 1));
            var bgBrush = FilpAndRotateImageBrush(leftImageBrush);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));

            #endregion Left

            #region Top

            var topImageBrush = ClipImageBrush(TopBrush, new Rect(0, 0, 1, 1 - currentPosition));
            topImageBrush = RotateImageBrush(topImageBrush, 270);
            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, _realRect3D.SizeY, _realRect3D.SizeZ),
                    new Point3D(position, _realRect3D.SizeY, _realRect3D.SizeZ),
                    new Point3D(0, 0, _realRect3D.SizeZ),
                    new Point3D(position, 0, _realRect3D.SizeZ)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(topImageBrush)));

            sectionMeshBuilder = new MeshBuilder();
            //The 
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(position, 0, 0),
                    new Point3D(position, _realRect3D.SizeY, 0),
                    new Point3D(0, 0, 0),
                    new Point3D(0, _realRect3D.SizeY, 0)
                }, 2);

            topImageBrush = ClipImageBrush(BottomBrush, new Rect(0, 0, 1, 1 - currentPosition));
            bgBrush = FilpImageBrush(topImageBrush);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));

            #endregion Top
        }

        private void CreateTopPartSlice(Model3DGroup plotModel, double currentPosition)
        {
            var position = (1 - currentPosition) * _realRect3D.SizeZ;

            if (position < 0.0)
                return;

            #region Left

            var leftImageBrush = ClipImageBrush(LeftBrush,
                new Rect(0, currentPosition, 1, 1 - currentPosition));

            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, 0, position),
                    new Point3D(_realRect3D.SizeX, 0, position),
                    new Point3D(0, 0, 0),
                    new Point3D(_realRect3D.SizeX, 0, 0)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(leftImageBrush)));

            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(_realRect3D.SizeX, _realRect3D.SizeY, position),
                    new Point3D(0, _realRect3D.SizeY, position),
                    new Point3D(_realRect3D.SizeX, _realRect3D.SizeY, 0),
                    new Point3D(0, _realRect3D.SizeY, 0)
                }, 2);

            leftImageBrush = ClipImageBrush(RightBrush,
                new Rect(0, currentPosition, 1, 1 - currentPosition));
            var bgBrush = FilpAndRotateImageBrush(leftImageBrush);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));

            #endregion Left

            #region Front

            var frontImageBrush = ClipImageBrush(FrontBrush,
                new Rect(0, currentPosition, 1, 1 - currentPosition));
            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(_realRect3D.SizeX, 0, position),
                    new Point3D(_realRect3D.SizeX, _realRect3D.SizeY, position),
                    new Point3D(_realRect3D.SizeX, 0, 0),
                    new Point3D(_realRect3D.SizeX, _realRect3D.SizeY, 0)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(frontImageBrush)));

            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, _realRect3D.SizeY, position),
                    new Point3D(0, 0, position),
                    new Point3D(0, _realRect3D.SizeY, 0),
                    new Point3D(0, 0, 0)
                }, 2);

            frontImageBrush = ClipImageBrush(BackBrush,
                new Rect(0, currentPosition, 1, 1 - currentPosition));
            bgBrush = FilpAndRotateImageBrush(frontImageBrush);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));

            #endregion Front
        }

        private void CreateLeftPartSlice(Model3DGroup plotModel, double currentPosition)
        {
            #region Top

            var position = currentPosition * _realRect3D.SizeY;
            var topImageBrush = ClipImageBrush(TopBrush, new Rect(currentPosition, 0
                , 1 - currentPosition, 1));

            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, position, _realRect3D.SizeZ),
                    new Point3D(0, _realRect3D.SizeY, _realRect3D.SizeZ),
                    new Point3D(_realRect3D.SizeX, position, _realRect3D.SizeZ),
                    new Point3D(_realRect3D.SizeX, _realRect3D.SizeY, _realRect3D.SizeZ)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(topImageBrush)));

            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(_realRect3D.SizeX, position, 0),
                    new Point3D(_realRect3D.SizeX, _realRect3D.SizeY, 0),
                    new Point3D(0, position, 0),
                    new Point3D(0, _realRect3D.SizeY, 0)
                }, 2);

            topImageBrush = ClipImageBrush(BottomBrush, new Rect(currentPosition, 0
                , 1 - currentPosition, 1));

            var bgBrush = topImageBrush;
            bgBrush = FilpImageBrush(bgBrush);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));

            #endregion Top

            #region Front

            var frontImageBrush = ClipImageBrush(FrontBrush, new Rect(currentPosition, 0
                , (1 - currentPosition), 1));

            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(_realRect3D.SizeX, position, _realRect3D.SizeZ),
                    new Point3D(_realRect3D.SizeX, _realRect3D.SizeY, _realRect3D.SizeZ),
                    new Point3D(_realRect3D.SizeX, position, 0),
                    new Point3D(_realRect3D.SizeX, _realRect3D.SizeY, 0)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(frontImageBrush)));

            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, _realRect3D.SizeY, _realRect3D.SizeZ),
                    new Point3D(0, position, _realRect3D.SizeZ),
                    new Point3D(0, _realRect3D.SizeY, 0),
                    new Point3D(0, position, 0)
                }, 2);

            frontImageBrush = ClipImageBrush(BackBrush, new Rect(currentPosition, 0
                , 1 - currentPosition, 1));

            bgBrush = FilpAndRotateImageBrush(frontImageBrush);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));

            #endregion Front
        }

        #region denpendcy propeties

        // Using a DependencyProperty as the backing store for _realRect3D.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RectangleProperty =
            DependencyProperty.Register("Rectangle", typeof(Rect3D), typeof(Section3DVisual3D),
                new PropertyMetadata(new Rect3D(0, 0, 0, 1, 1, 1), ModelChanged));

        // Using a DependencyProperty as the backing store for SectionPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SectionPositionProperty =
            DependencyProperty.Register("SectionPosition", typeof(double), typeof(Section3DVisual3D),
                new PropertyMetadata(0.0, ModelChanged));

        // Using a DependencyProperty as the backing store for SectionBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SectionBrushProperty =
            DependencyProperty.Register("SectionBrush", typeof(sysMedia.Brush), typeof(Section3DVisual3D),
                new PropertyMetadata(sysMedia.Brushes.Red, ModelChanged));

        // Using a DependencyProperty as the backing store for SectionBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftBrushProperty =
            DependencyProperty.Register("LeftBrush", typeof(sysMedia.Brush), typeof(Section3DVisual3D),
                new PropertyMetadata(sysMedia.Brushes.Blue, ModelChanged));

        public static readonly DependencyProperty TopBrushProperty =
            DependencyProperty.Register("TopBrush", typeof(sysMedia.Brush), typeof(Section3DVisual3D),
                new PropertyMetadata(sysMedia.Brushes.Green, ModelChanged));

        public static readonly DependencyProperty FrontBrushProperty =
            DependencyProperty.Register("FrontBrush", typeof(sysMedia.Brush), typeof(Section3DVisual3D),
                new PropertyMetadata(sysMedia.Brushes.DarkCyan, ModelChanged));

        public static readonly DependencyProperty RightBrushProperty =
            DependencyProperty.Register("RightBrush", typeof(sysMedia.Brush), typeof(Section3DVisual3D),
                new PropertyMetadata(sysMedia.Brushes.DarkOliveGreen, ModelChanged));

        public static readonly DependencyProperty BackBrushProperty =
            DependencyProperty.Register("BackBrush", typeof(sysMedia.Brush), typeof(Section3DVisual3D),
                new PropertyMetadata(sysMedia.Brushes.DodgerBlue, ModelChanged));

        public static readonly DependencyProperty BottomBrushProperty =
            DependencyProperty.Register("BottomBrush", typeof(sysMedia.Brush), typeof(Section3DVisual3D),
                new PropertyMetadata(sysMedia.Brushes.DarkSalmon, ModelChanged));

        public static readonly DependencyProperty CurrentDirectionProperty =
            DependencyProperty.Register("CurrentDirection", typeof(FrameDirection), typeof(Section3DVisual3D),
                new PropertyMetadata(FrameDirection.Left, ModelChanged));

        private double _maxCount;
        private Rect3D _realRect3D;

        #endregion

        #region properties

        public Section3DVisual3D()
        {
            _visualChild = new ModelVisual3D();
            Children.Add(_visualChild);
        }

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

        public sysMedia.Brush SectionBrush
        {
            get { return (sysMedia.Brush)GetValue(SectionBrushProperty); }
            set { SetValue(SectionBrushProperty, value); }
        }

        public sysMedia.Brush LeftBrush
        {
            get { return (sysMedia.Brush)GetValue(LeftBrushProperty); }
            set { SetValue(LeftBrushProperty, value); }
        }

        public sysMedia.Brush TopBrush
        {
            get { return (sysMedia.Brush)GetValue(TopBrushProperty); }
            set { SetValue(TopBrushProperty, value); }
        }

        public sysMedia.Brush FrontBrush
        {
            get { return (sysMedia.Brush)GetValue(FrontBrushProperty); }
            set { SetValue(FrontBrushProperty, value); }
        }

        public sysMedia.Brush RightBrush
        {
            get { return (sysMedia.Brush)GetValue(RightBrushProperty); }
            set { SetValue(RightBrushProperty, value); }
        }

        public sysMedia.Brush BackBrush
        {
            get { return (sysMedia.Brush)GetValue(BackBrushProperty); }
            set { SetValue(BackBrushProperty, value); }
        }

        public sysMedia.Brush BottomBrush
        {
            get { return (sysMedia.Brush)GetValue(BottomBrushProperty); }
            set { SetValue(BottomBrushProperty, value); }
        }

        public FrameDirection CurrentDirection
        {
            get { return (FrameDirection)GetValue(CurrentDirectionProperty); }
            set { SetValue(CurrentDirectionProperty, value); }
        }

        #endregion

        #region common functions

        private sysMedia.Brush FilpAndRotateImageBrush(sysMedia.Brush brush)
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

        private sysMedia.Brush RotateImageBrush(sysMedia.Brush brush, int degree)
        {
            var bgBrush = brush.Clone();
            var transform = sysMedia.Matrix.Identity;
            transform.RotateAt(degree, 0.5, 0.5);
            if (bgBrush is sysMedia.ImageBrush)
            {
                bgBrush.RelativeTransform = new sysMedia.MatrixTransform(transform);
            }
            return bgBrush;
        }

        private sysMedia.Brush FilpImageBrush(sysMedia.Brush brush)
        {
            var bgBrush = brush.Clone();
            var transform = sysMedia.Matrix.Identity;
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
                imgBrush.Viewbox = regionRect;//ToDo: need to consider the image is not a square.
                return imgBrush;
            }

            return new sysMedia.SolidColorBrush(sysMedia.Colors.Transparent);
        }

        private Rect3D GetSampleImageBox()
        {
            var maxLength = Math.Max(Math.Max(Rectangle.SizeX, Rectangle.SizeY), Rectangle.SizeZ);

            var sampleImageBox = new Rect3D(0, 0, 0, Rectangle.SizeX / maxLength, Rectangle.SizeY / maxLength,
                Rectangle.SizeZ / maxLength);

            return sampleImageBox;
        }

        #endregion

        #region selection

        private void CreateFrontSelectionSlice(Model3DGroup plotModel, sysMedia.Brush brush)
        {
            if (SectionPosition == 0.0)
            {
                CreateFrontMinSlice(plotModel);
            }
            else
            {
                CreateFrontSlice(plotModel, brush, SectionPosition);
            }
        }

        private void CreateLeftSelectionSlice(Model3DGroup plotModel, sysMedia.Brush brush)
        {
            if (SectionPosition == 0.0)
            {
                CreateLeftMinSlice(plotModel);
            }
            else
            {
                CreateLeftSlice(plotModel, brush, SectionPosition);
            }
        }

        private void CreateTopSelectionSlice(Model3DGroup plotModel, sysMedia.Brush brush)
        {
            if (SectionPosition == 0.0)
            {
                CreateTopMinSlice(plotModel);
            }
            else
            {
                CreateTopSlice(plotModel, brush, SectionPosition);
            }
        }

        #endregion selection

        #region Create all facets

        private void CreateFrontMinSlice(Model3DGroup plotModel)
        {
            CreateFrontSlice(plotModel, FrontBrush, 0); //z=0 for helix top ,Raw Front==Helix Top;
        }

        private void CreateLeftMinSlice(Model3DGroup plotModel)
        {
            CreateLeftSlice(plotModel, LeftBrush, 0);
        }

        private void CreateTopMinSlice(Model3DGroup plotModel)
        {
            CreateTopSlice(plotModel, TopBrush, 0);
        }

        private void CreateFrontMaxSlice(Model3DGroup plotModel)
        {
            CreateFrontSlice(plotModel, BackBrush, 1);
        }

        private void CreateTopMaxSlice(Model3DGroup plotModel)
        {
            CreateTopSlice(plotModel, BottomBrush, 1);
        }

        private void CreateLeftMaxSlice(Model3DGroup plotModel)
        {
            CreateLeftSlice(plotModel, RightBrush, 1);
        }

        /// <summary>
        ///     Raw Front==Helix Top;
        /// </summary>
        /// <param name="plotModel"></param>
        /// <param name="brush"></param>
        /// <param name="currentPosition"></param>
        private void CreateFrontSlice(Model3DGroup plotModel, sysMedia.Brush brush, double currentPosition)
        {
            var position = _realRect3D.SizeX * (1 - currentPosition);
            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(position, 0, _realRect3D.SizeZ),
                    new Point3D(position, _realRect3D.SizeY, _realRect3D.SizeZ),
                    new Point3D(position, 0, 0),
                    new Point3D(position, _realRect3D.SizeY, 0)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(brush)));

            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(position, _realRect3D.SizeY, _realRect3D.SizeZ),
                    new Point3D(position, 0, _realRect3D.SizeZ),
                    new Point3D(position, _realRect3D.SizeY, 0),
                    new Point3D(position, 0, 0)
                }, 2);

            var bgBrush = FilpAndRotateImageBrush(brush);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));
        }


        /// <summary>
        ///     Raw Top==Helix left
        /// </summary>
        /// <param name="plotModel"></param>
        /// <param name="brush"></param>
        /// <param name="currentPosition"></param>
        private void CreateTopSlice(Model3DGroup plotModel, sysMedia.Brush brush, double currentPosition)
        {
            var position = _realRect3D.SizeZ * (1 - currentPosition);
            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, 0, position),
                    new Point3D(0, _realRect3D.SizeY, position),
                    new Point3D(_realRect3D.SizeX, 0, position),
                    new Point3D(_realRect3D.SizeX, _realRect3D.SizeY, position)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(brush)));

            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, _realRect3D.SizeY, position),
                    new Point3D(0, 0, position),
                    new Point3D(_realRect3D.SizeX, _realRect3D.SizeY, position),
                    new Point3D(_realRect3D.SizeX, 0, position)
                }, 2);

            var bgBrush = FilpAndRotateImageBrush(brush);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));
        }

        /// <summary>
        ///     Raw  Left==Helix Front
        /// </summary>
        /// <param name="plotModel"></param>
        /// <param name="brush"></param>
        /// <param name="currentPosition"></param>
        private void CreateLeftSlice(Model3DGroup plotModel, sysMedia.Brush brush, double currentPosition)
        {
            var position = currentPosition * _realRect3D.SizeY;
            var sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(0, position, _realRect3D.SizeZ),
                    new Point3D(_realRect3D.SizeX, position, _realRect3D.SizeZ),
                    new Point3D(0, position, 0),
                    new Point3D(_realRect3D.SizeX, position, 0)
                }, 2);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(brush)));

            sectionMeshBuilder = new MeshBuilder();
            sectionMeshBuilder.AddRectangularMesh(
                new[]
                {
                    new Point3D(_realRect3D.SizeX, position, _realRect3D.SizeZ),
                    new Point3D(0, position, _realRect3D.SizeZ),
                    new Point3D(_realRect3D.SizeX, position, 0),
                    new Point3D(0, position, 0)
                }, 2);

            var bgBrush = FilpAndRotateImageBrush(brush);
            plotModel.Children.Add(new GeometryModel3D(sectionMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(bgBrush)));
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