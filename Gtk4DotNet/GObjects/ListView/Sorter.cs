using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class Sorter : GObject
{
    public void Changed(SorterChange change) => Changed(this, change);
    internal Sorter() {}
    internal Sorter(nint handle)
    {
        SetInternalHandle(handle);
        AutoDestroyed = true;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_sorter_changed", CallingConvention = CallingConvention.Cdecl)]
    extern static void Changed(Sorter sorter, SorterChange change);
}