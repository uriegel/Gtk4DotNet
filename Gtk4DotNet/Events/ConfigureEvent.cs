using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

[StructLayout(LayoutKind.Sequential)]
public struct ConfigureEvent
{
    public int EventType;
    public IntPtr Window;
    public byte SendEvent;
    public int X;
    public int Y;
    public int Width;
    public int Height;            
}
