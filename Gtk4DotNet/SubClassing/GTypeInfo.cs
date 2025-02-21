using System.Runtime.InteropServices;

namespace GtkDotNet.SubClassing;

[StructLayout(LayoutKind.Sequential)]
    public struct GTypeInfo
    {
        /* interface types, classed types, instantiated types */
        public ushort classSize;
        public IntPtr baseInit;
        public IntPtr baseFinalize;
        public IntPtr classInit;
        public IntPtr classFinalize;
        public IntPtr classData;
        /* instantiated types */
        public ushort instanceSize;
        public ushort nPreallocs;
        public IntPtr instanceInit;
        /* value handling */
        public IntPtr valueTable;
    }