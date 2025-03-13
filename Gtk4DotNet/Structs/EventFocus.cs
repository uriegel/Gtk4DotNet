namespace GtkDotNet;

using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

[StructLayout(LayoutKind.Sequential)]
struct GdkEventFocus
{
    public EventType Type;
    public WindowHandle Window;
    public byte SendEvent;
    public short In;
}