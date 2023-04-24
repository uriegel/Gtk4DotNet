using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void ConnectDelegate(IntPtr builder, IntPtr obj, string signal, string handleName, IntPtr connectObj, int flags);

public delegate void ResizeFunc(IntPtr widget, int w, int h, IntPtr zero);
public delegate void DragFunc(IntPtr gesture, double x, double y, IntPtr zero);
public delegate void PressedFunc(IntPtr gesture, int pressCount, double x, double y, IntPtr zero);