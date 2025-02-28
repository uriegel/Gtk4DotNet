using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ListStore
{
    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ListModelHandle New(GTypeHandle type);

    public static ListModelHandle Append(this ListModelHandle model, ObjectHandle obj)

    {

        model._Append(obj);
        obj.Dispose();
        return model;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_append", CallingConvention = CallingConvention.Cdecl)]
    public extern static void _Append(this ListModelHandle model, ObjectHandle obj);
}
        