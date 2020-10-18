using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezierCurve
{
    class Operation
    {
        public enum Option
        {
            Select,
            Move,
            Resize,
            Rotate,
            Create
        }
        public static Option option;
    }
}
