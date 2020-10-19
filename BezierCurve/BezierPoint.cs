using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace BezierCurve
{
    public class BezierPoint : INotifyPropertyChanged
    {
        public bool IsSelected { get; set; } = false;
        public bool IsProcessed { get; set; } = false;

        private Point point;

        public Point Point
        {
            get
            {
                return point;
            }
            set
            {
                point = value;
                OnPropertyChanged("PointX");
                OnPropertyChanged("PointY");
                OnPropertyChanged("Landmarks");
                Canvas.SetLeft(ellipse, point.X - 3);
                Canvas.SetTop(ellipse, point.Y - 3);
            }
        }

        public double PointX
        {

            get { return Point.X; }
            set
            {
                if (Point.X != value)
                {
                    Point = new Point(value, Point.Y);
                }
            }
        }
        public double PointY
        {

            get { return Point.Y; }
            set
            {
                if (Point.Y != value)
                {
                    Point = new Point(Point.X, value);
                }
            }
        }

        public Ellipse ellipse;


        public Label PointXLabel = new Label();
        public DoubleUpDown PointXValue = new DoubleUpDown();
        public Label PointYLabel = new Label();
        public DoubleUpDown PointYValue = new DoubleUpDown();

        public Binding PointXBinding = new Binding("PointX");
        public Binding PointYBinding = new Binding("PointY");

        public Grid grid = new Grid();
        public ColumnDefinition gridCol1 = new ColumnDefinition();
        public ColumnDefinition gridCol2 = new ColumnDefinition();
        public ColumnDefinition gridCol3 = new ColumnDefinition();
        public ColumnDefinition gridCol4 = new ColumnDefinition();
        public RowDefinition gridRow1 = new RowDefinition();



        public BezierPoint()
        {
            this.ellipse = new Ellipse();
            ellipse.StrokeThickness = 1;
            ellipse.Width = 6;
            ellipse.Height = 6;
            ellipse.Fill = new SolidColorBrush(Colors.Black);

            ellipse.MouseEnter += new MouseEventHandler(onHover);
            ellipse.MouseLeave += new MouseEventHandler(onHoverEnd);

            GenerateGrid();
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(PropertyName));
        }

        public void createMouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            Point = e.GetPosition(canvas);
            canvas.Children.Add(ellipse);
            return;
        }

        public void moveMouseDown(object sender, MouseEventArgs e)
        {
            IsProcessed = !IsProcessed;
        }

        public void moveMouseMove(object sender, MouseEventArgs e)
        {
            if(!IsProcessed)
                return;
            Canvas canvas = (Canvas)sender;
            Point = e.GetPosition(canvas);
        }

        public void onHover(object sender, MouseEventArgs e)
        {
            ellipse.Fill = new SolidColorBrush(Colors.Red);
            IsSelected = true;
        }

        public void onHoverEnd(object sender, MouseEventArgs e)
        {
            ellipse.Fill = new SolidColorBrush(Colors.Black);
            IsSelected = false;
        }

        public virtual BezierPoint selectMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsSelected)
                return this;
            else
                return null;
        }

        void GenerateGrid()
        {

            PointXLabel.Content = "X: ";
            PointYLabel.Content = "Y: ";
            PointXLabel.Width = 30;
            PointYLabel.Width = 30;

            grid.ColumnDefinitions.Add(gridCol1);
            grid.ColumnDefinitions.Add(gridCol2);
            grid.ColumnDefinitions.Add(gridCol3);
            grid.ColumnDefinitions.Add(gridCol4);

            grid.RowDefinitions.Add(gridRow1);

            gridRow1.Height = new GridLength(30);

            PointXValue.Height = 30;
            PointYValue.Height = 30;

            PointXBinding.Source = this;
            PointYBinding.Source = this;
            PointXBinding.Mode = BindingMode.TwoWay;
            PointYBinding.Mode = BindingMode.TwoWay;
            PointXBinding.NotifyOnSourceUpdated = true;
            PointYBinding.NotifyOnSourceUpdated = true;
            PointXBinding.NotifyOnTargetUpdated = true;
            PointYBinding.NotifyOnTargetUpdated = true;

            
            PointYValue.SetBinding(DoubleUpDown.ValueProperty, PointYBinding);
            PointXValue.SetBinding(DoubleUpDown.ValueProperty, PointXBinding);

            Grid.SetRow(PointXLabel, 0);
            Grid.SetRow(PointYLabel, 0);
            Grid.SetRow(PointXValue, 0);
            Grid.SetRow(PointYValue, 0);
            

            Grid.SetColumn(PointXLabel, 0);
            Grid.SetColumn(PointYLabel, 2);
            Grid.SetColumn(PointXValue, 1);
            Grid.SetColumn(PointYValue, 3);

            grid.Children.Add(PointXLabel);
            grid.Children.Add(PointYLabel);
            grid.Children.Add(PointXValue);
            grid.Children.Add(PointYValue);
        }
    }
}
