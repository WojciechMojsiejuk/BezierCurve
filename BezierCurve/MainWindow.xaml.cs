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
        Curve bezierCurve = new Curve();
        BezierPoint point;

        public MainWindow()
        {
            InitializeComponent();
            imageCanvas.Children.Add(bezierCurve.ControlPointsPath);
            imageCanvas.Children.Add(bezierCurve.BezierCurvePath);
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
                    point = null;
                    foreach (var elem in bezierCurve.Landmarks)
                    {
                        if (elem.selectMouseDown(sender, e) != null)
                        {
                            point = elem;
                            break;
                        }
                    }
                    break;
                case (Operation.Option.Move):
                    if (point != null)
                    {
                        point.moveMouse(sender, e);
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
                    if (point != null)
                    {
                        point.moveMouse(sender, e);
                    }
                    break;
            }
        }
    }
}
