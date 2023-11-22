//[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void OnePointerDelegate(IntPtr p);
delegate void TwoPointerDelegate(IntPtr p, IntPtr pp);
delegate bool TwoPointerBoolRetDelegate(IntPtr p, IntPtr pp);
delegate void DrawFunctionDelegate(IntPtr drawingArea, IntPtr cairo, int width, int height, IntPtr data);
delegate void DrawingAreaResizeDelegate(IntPtr drawingArea, int width, int height, IntPtr data);
delegate void PressedGestureDelegate(IntPtr _, int pressCount, double x, double y, IntPtr __);
delegate void DragGestureDelegate(IntPtr _, double x, double y, IntPtr __);