namespace Gtk4DotNet;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
struct GObjectStruct
{
    public nint GTypeInstance;
    public int RefCount;
}