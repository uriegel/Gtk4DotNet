using System.Runtime.InteropServices;

namespace GtkDotNet.SubClassing;

[StructLayout(LayoutKind.Sequential)]
public struct GTypeQuery
{
    public IntPtr type;
    public IntPtr typeName;
    public ushort classSize;
    public ushort instanceSize;
}