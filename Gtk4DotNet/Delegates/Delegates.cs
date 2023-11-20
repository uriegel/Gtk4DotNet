//[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void OnePointerDelegate(IntPtr p);
delegate void TwoPointerDelegate(IntPtr p, IntPtr pp);