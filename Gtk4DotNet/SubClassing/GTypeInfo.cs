using System.Runtime.InteropServices;

namespace GtkDotNet.SubClassing;

[StructLayout(LayoutKind.Sequential)]
    public struct GTypeInfo
    {
        /* interface types, classed types, instantiated types */
        public ushort classSize;
        public nint baseInit;
        public nint baseFinalize;
        public nint classInit;
        public nint classFinalize;
        public nint classData;
        /* instantiated types */
        public ushort instanceSize;
        public ushort nPreallocs;
        public nint instanceInit;
        /* value handling */
        public nint valueTable;
    }