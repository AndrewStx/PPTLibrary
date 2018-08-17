namespace ShapesLibrary
{
    public static partial class COMExtensions
    {
        /// <summary>
        // Releases specified COM object by decrementing to zero the reference count of the Runtime Callable Wrapper (RCW) associated with this object.
        /// </summary>
        public static void ReleaseCOM(this object obj)
        {
            if (obj != null)
            {
                while (System.Runtime.InteropServices.Marshal.ReleaseComObject(obj) != 0) ;
            }
        }
    }
}
