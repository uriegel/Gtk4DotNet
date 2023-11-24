using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct TextIter 
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst=80)] 
    byte[] phantom; 
}
