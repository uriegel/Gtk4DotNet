using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class GdkSeat
{
    [DllImport(Libs.LibGtk, EntryPoint = "gdk_seat_get_pointer", CallingConvention = CallingConvention.Cdecl)]
    public extern static DeviceHandle GetDevice(this GdkSeatHandle seat);

    [DllImport(Libs.LibGtk, EntryPoint = "gdk_seat_get_keyboard", CallingConvention = CallingConvention.Cdecl)]
    public extern static DeviceHandle GetKeyboard(this GdkSeatHandle seat);
}
