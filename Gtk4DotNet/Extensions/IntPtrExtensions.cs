using System.Runtime.InteropServices;

namespace GtkDotNet.Extensions;

static class IntPtrExtensions
{
    public static string? PtrToString(this IntPtr obj)
        => Marshal.PtrToStringUTF8(obj);
}
