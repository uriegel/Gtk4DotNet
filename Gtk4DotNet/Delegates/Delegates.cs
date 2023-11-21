//[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void OnePointerDelegate(IntPtr p);
delegate void TwoPointerDelegate(IntPtr p, IntPtr pp);

delegate void DrawFunctionDelegate(IntPtr drawingArea, IntPtr cairo, int width, int height, IntPtr data);