using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class MountOperation
{
    [DllImport(Libs.LibGio, EntryPoint = "g_mount_operation_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static MountOperationHandle New();
}
