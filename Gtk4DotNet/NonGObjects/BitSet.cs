using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class BitSet : BaseHandle
{
    protected override bool ReleaseHandle()
    {
        Unref(handle);
        return true;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_bitset_unref", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Unref(nint bitset);
}