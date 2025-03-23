using System.Runtime.InteropServices;

namespace GtkDotNet;

[StructLayout(LayoutKind.Sequential)]
public struct GValueStruct
{
    public nint g_type;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public uint[] data;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public uint[] v_data;
}