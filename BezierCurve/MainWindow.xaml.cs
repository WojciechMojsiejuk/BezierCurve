using System;
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

namespace BezierCurve
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Curve bezierCurve = new Curve();
        public BezierPoint SelectPoint;

        public MainWindow()
        {
            InitializeComponent();
            imageCanvas.Children.Add(bezierCurve.ControlPointsPath);
            imageCanvas.Children.Add(bezierCurve.BezierCurvePath);
            listLandmarks.ItemsSource = bezierCurve.Landmarks;
            //listLandmarks.SelectedItem = SelectPoint;
        }

        private void SelectOperation(object sender, RoutedEventArgs e)
        {
            try
            {
                ICommandSource option = (ICommandSource)e.OriginalSource;
                System.Diagnostics.Debug.WriteLine(option.CommandParameter);
                switch (option.CommandParameter)
                {
                    case "Select":
                        Operation.option = Operation.Option.Select;
                        break;
                    case "Move":
                        Operation.option = Operation.Option.Move;
                        break;
                    case "Create":
                        Operation.option = Operation.Option.Create;
                        break;
                }
            }
            catch (InvalidCastException ice)
            {
                System.Diagnostics.Debug.WriteLine(ice);
            }
        }


        public void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (Operation.option)
            {
                case (Operation.Option.Select):
                    SelectPoint = null;
                    foreach (var elem in bezierCurve.Landmarks)
                    {
                        if (elem.selectMouseDown(sender, e) != null)
                        {
                            SelectPoint = elem;
                            XValue.Value = elem.PointX;
                            YValue.Value = elem.PointY;
                            break;
                        }
                    }
                    break;
                case (Operation.Option.Move):
                    if (SelectPoint != null)
                    {
                        SelectPoint.moveMouseDown(sender, e);
                    }
                    break;
                case (Operation.Option.Create):
                    bezierCurve.createMouseDown(sender, e);
                    imageCanvas.UpdateLayout();
                    break;
            }
        }

        public void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            switch (Operation.option)
            {
                case (Operation.Option.Move):
                    if (SelectPoint != null)
                    {
                        SelectPoint.moveMouseMove(sender, e);
                        XValue.Value = SelectPoint.PointX;
                        YValue.Value = SelectPoint.PointY;
                        bezierCurve.DrawBezierCurveControlPath();
                        bezierCurve.DrawBezierCurve();
                    }
                    break;
            }
        }

        private void listLandmarks_Selected(object sender, RoutedEventArgs e)
        {
            var list = (ListView)sender;
            var elem = (BezierPoint)list.SelectedItem;
            XValue.Value = elem.PointX;
            YValue.Value = elem.PointY;
        }

        private void UpdatePoint_Click(object sender, RoutedEventArgs e)
        {
            var elem = (BezierPoint)(listLandmarks.SelectedItem);
            if (elem != null)
            {
                elem.PointX = (double)XValue.Value;
                elem.PointY = (double)YValue.Value;
                bezierCurve.DrawBezierCurveControlPath();
                bezierCurve.DrawBezierCurve();
            }
        }

        private void CreateNewPoint_Click(object sender, RoutedEventArgs e)
        {
            BezierPoint elem = new BezierPoint();
            imageCanvas.Children.Add(elem.ellipse);
            bezierCurve.Landmarks.Add(elem);
        }
    }
}
