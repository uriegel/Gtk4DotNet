using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class Builder
{
    public static BuilderHandle FromResource(string path)
    {
        // TODO: Memoize
        Application.RegisterResources();
        return _FromResource(path);        
    } 

    public static BuilderHandle GetObject<THandle>(this BuilderHandle builder, string objectName, Action<WindowHandle> withObject)
        where THandle : WindowHandle
            => builder.SideEffect(b => withObject(b.GetWindowObject(objectName)));

    public static BuilderHandle GetObject<THandle>(this BuilderHandle builder, string objectName, Action<ButtonHandle> withObject)
        where THandle : ButtonHandle
            => builder.SideEffect(b => withObject(b.GetButtonObject(objectName)));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_builder_new_from_resource", CallingConvention = CallingConvention.Cdecl)]
    extern static BuilderHandle _FromResource(string path);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    extern static WindowHandle GetWindowObject(this BuilderHandle builder, string objectName);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    extern static ButtonHandle GetButtonObject(this BuilderHandle builder, string objectName);

    // public static int AddFromFile(this IntPtr builder, string file) => AddFromFile(builder, file, IntPtr.Zero);

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_builder_new", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr New();


    // [DllImport(Libs.LibGtk, EntryPoint="gtk_builder_connect_signals_full", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr ConnectSignals(this IntPtr builder, ConnectDelegate onConnection);

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_builder_add_from_file", CallingConvention = CallingConvention.Cdecl)]
    // extern static int AddFromFile(this IntPtr builder, string file, IntPtr nil);
}

