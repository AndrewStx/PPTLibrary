using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesLibrary
{
    internal static partial class Extensions2
    {
        public static void ReleaseCOM(this object obj)
        {
            if (obj != null)
                while (System.Runtime.InteropServices.Marshal.ReleaseComObject(obj) != 0);
        }
    }
}
