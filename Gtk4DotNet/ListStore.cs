using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ListStore
{
    public static IListModel New()
        => New(GObject.Type());

    public static IListModel New(GTypeHandle type)
        => new ListModelHandle(_New(type));

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static nint _New(GTypeHandle type);
}
        