using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BezierCurve 
{
    static class ListExtension
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self) => self.Select((item, index) => (item, index));
    }

    public class Curve : INotifyPropertyChanged
    {
        ObservableCollection<BezierPoint> landmarks;

        PathFigure controlPointsFigure = new PathFigure();
        PathFigure bezierFigure = new PathFigure();

        public Path ControlPointsPath = new Path();
        public Path BezierCurvePath = new Path();

        public Grid grid = new Grid();

        const int STEP = 100;

        public Curve()
        {
            Landmarks = new ObservableCollection<BezierPoint>();
            Landmarks.CollectionChanged += this.OnCollectionChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<BezierPoint> Landmarks
        {
            get { return landmarks; }
            set
            {
                landmarks = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Landmarks"));
                }
            }
        }


        public static long GetBinCoeff(long N, long K)
        {
            // This function gets the total number of unique combinations based upon N and K.
            // N is the total number of items.
            // K is the size of the group.
            // Total number of unique combinations = N! / ( K! (N - K)! ).
            // This function is less efficient, but is more likely to not overflow when N and K are large.

            long r = 1;
            long d;
            if (K > N) return 0;
            for (d = 1; d <= K; d++)
            {
                r *= N--;
                r /= d;
            }
            return r;
        }

        public static double BernsteinPolynomial(double t, int i, int n)
        {
            return GetBinCoeff(n, i) * Math.Pow(t, i) * Math.Pow((1 - t), (n - i));
        }



        public void createMouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            BezierPoint elem = new BezierPoint();
            canvas.Children.Add(elem.ellipse);
            elem.Point = e.GetPosition(canvas);
            Landmarks.Add(elem);
            return;
        }



        //public override CGObject selectMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    foreach (var elem in landmarks)
        //    {
        //        if (elem.selectMouseDown(sender, e) != null)
        //            return elem;
        //    }
        //    return null;
        //}

        void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("Point of bezier curve changed");
            DrawBezierCurve();
            DrawBezierCurveControlPath();
            
        }

        public void DrawBezierCurveControlPath()
        {
            controlPointsFigure.StartPoint = Landmarks[0].Point;


            PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();

            for (int index = 1; index < Landmarks.Count; index++)
            {
                LineSegment myLineSegment = new LineSegment();
                myLineSegment.Point = Landmarks[index].Point;
                myPathSegmentCollection.Add(myLineSegment);
            }

            controlPointsFigure.Segments = myPathSegmentCollection;

            PathFigureCollection myPathFigureCollection = new PathFigureCollection();
            myPathFigureCollection.Add(controlPointsFigure);

            PathGeometry myPathGeometry = new PathGeometry();
            myPathGeometry.Figures = myPathFigureCollection;

            ControlPointsPath.Stroke = Brushes.BlanchedAlmond;
            ControlPointsPath.StrokeThickness = 1;
            ControlPointsPath.StrokeDashArray = new DoubleCollection(new List<double>() { 4, 4 });
            ControlPointsPath.Data = myPathGeometry;
        }

        public void DrawBezierCurve()
        {
            bezierFigure.StartPoint = Landmarks[0].Point;


            PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();

            foreach (var elem in BezierCurveRange())
            {
                LineSegment myLineSegment = new LineSegment();
                myLineSegment.Point = elem;
                myPathSegmentCollection.Add(myLineSegment);
            }

            bezierFigure.Segments = myPathSegmentCollection;

            PathFigureCollection myPathFigureCollection = new PathFigureCollection();
            myPathFigureCollection.Add(bezierFigure);

            PathGeometry myPathGeometry = new PathGeometry();
            myPathGeometry.Figures = myPathFigureCollection;

            BezierCurvePath.Stroke = Brushes.Black;
            BezierCurvePath.StrokeThickness = 1;
            BezierCurvePath.Data = myPathGeometry;
        }

        Point Bezier(double t)
        {
            int n = Landmarks.Count - 1;
            Point point = new Point();
            point.X = 0;
            point.Y = 0;

            foreach (var (item, index) in Landmarks.WithIndex())
            {
                double bern = BernsteinPolynomial(t, index, n);
                point.X += item.Point.X * bern;
                point.Y += item.Point.Y * bern;
            }
            return point;
        }

        IEnumerable<Point> BezierCurveRange()
        {
            foreach (int i in Enumerable.Range(0, STEP))
            {
                double t = i / (double)(STEP - 1);
                yield return Bezier(t);
            }
        }
    }
}