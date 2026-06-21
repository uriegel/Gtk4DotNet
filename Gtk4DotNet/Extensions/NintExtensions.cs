using System.Runtime.InteropServices;

namespace Gtk4DotNet.Extensions;

static class NintExtensions
{
    public static string? PtrToString(this nint obj, bool free)
    {
        if (obj == 0)
            return null;
        var val = Marshal.PtrToStringUTF8(obj);
        if (free)
            GObject.Free(obj);
        return val;
    }
}
