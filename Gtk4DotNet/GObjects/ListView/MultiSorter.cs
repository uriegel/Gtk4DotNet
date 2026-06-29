using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class MultiSorter : Sorter
{
    public static MultiSorter New()
    {
        var res = _New();
        res.CheckDiagnostics();
        return res;
    }

    public MultiSorter Append(Sorter sorter)
    {
        Append(this, sorter);
        sorter.AutoDestroyed = true;
        return this;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_multi_sorter_new", CallingConvention = CallingConvention.Cdecl)]
    extern static MultiSorter _New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_multi_sorter_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void Append(MultiSorter multiSorter, Sorter sorter);
}

