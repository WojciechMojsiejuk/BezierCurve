using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BezierCurve
{
    public class BezierPoint : INotifyPropertyChanged
    {
        public bool IsSelected { get; set; } = false;
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
                OnPropertyChanged("Point");
                Canvas.SetLeft(ellipse, point.X - 3);
                Canvas.SetTop(ellipse, point.Y - 3);
            }
        }

        public Ellipse ellipse;

        public BezierPoint()
        {
            this.ellipse = new Ellipse();
            ellipse.StrokeThickness = 1;
            ellipse.Width = 6;
            ellipse.Height = 6;
            ellipse.Fill = new SolidColorBrush(Colors.Black);

            ellipse.MouseEnter += new MouseEventHandler(onHover);
            ellipse.MouseLeave += new MouseEventHandler(onHoverEnd);

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

        public void moveMouse(object sender, MouseEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            Point = e.GetPosition(canvas);
            return;
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
    }
}
