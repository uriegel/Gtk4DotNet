using System.Runtime.InteropServices;

namespace GtkDotNet.Extensions;

static class IntPtrExtensions
{
    // TODO free if it has to be freed
    public static string? PtrToString(this IntPtr obj, bool free)
    {
        var val = Marshal.PtrToStringUTF8(obj);
        if (free)
            obj.Free();
        return val ?? "";
    }
        
}
