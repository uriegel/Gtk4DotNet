using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class MultiSorter : Sorter
{
    public MultiSorter()
    {
        SetInternalHandle(New());
        CheckDiagnostics();
    }

    public MultiSorter Append(Sorter sorter)
    {
        Append(this, sorter);
        sorter.AutoDestroyed = true;
        return this;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_multi_sorter_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_multi_sorter_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void Append(MultiSorter multiSorter, Sorter sorter);
}

