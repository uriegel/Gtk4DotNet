using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class Builder
{
    public static BuilderHandle FromResource(string path)
    {
        // TODO: Memoize
        Application.RegisterResources();
        return _FromResource(path);
    }

    public static BuilderHandle FromDotNetResource(string path)
    {
        var ui = new StreamReader(Resources.Get(path)!).ReadToEnd();
        return _FromString(ui, -1);
    }

    public static THandle GetWidget<THandle>(this BuilderHandle builder, string objectName)
            where THandle : WidgetHandle, new()
    {
        var p = builder._GetWidget(objectName);
        var res = new THandle();
        res.SetInternalHandle(p);
        return res;
    }

    public static BuilderHandle FromString(string ui) => _FromString(ui, -1);

    public static BuilderHandle GetObject<THandle>(this BuilderHandle builder, string objectName, Action<THandle> withObject)
            where THandle : WidgetHandle, new()
        => builder.SideEffect(b => withObject(b.GetWidget<THandle>(objectName)));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_new_from_resource", CallingConvention = CallingConvention.Cdecl)]
    extern static BuilderHandle _FromResource(string path);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_new_from_string", CallingConvention = CallingConvention.Cdecl)]
    extern static BuilderHandle _FromString(string ui, int length);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetWidget(this BuilderHandle builder, string objectName);

    // public static int AddFromFile(this IntPtr builder, string file) => AddFromFile(builder, file, IntPtr.Zero);

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_builder_connect_signals_full", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr ConnectSignals(this IntPtr builder, ConnectDelegate onConnection);

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_builder_add_from_file", CallingConvention = CallingConvention.Cdecl)]
    // extern static int AddFromFile(this IntPtr builder, string file, IntPtr nil);
}

