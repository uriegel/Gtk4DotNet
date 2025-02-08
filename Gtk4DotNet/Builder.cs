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

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    public extern static WidgetHandle GetWidget(this BuilderHandle builder, string objectName);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    public extern static WindowHandle GetWindow(this BuilderHandle builder, string objectName);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    public extern static WebViewHandle GetWebView(this BuilderHandle builder, string objectName);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    public extern static RevealerHandle GetRevealer(this BuilderHandle builder, string objectName);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    public extern static ButtonHandle GetButton(this BuilderHandle builder, string objectName);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    public extern static ProgressBarHandle GetProgressBar(this BuilderHandle builder, string objectName);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    public extern static TextViewHandle GetTextView(this BuilderHandle builder, string objectName);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    public extern static PopoverHandle GetPopover(this BuilderHandle builder, string objectName);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    public extern static MenuItemHandle GetMenuItem(this BuilderHandle builder, string objectName);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    public extern static MenuHandle GetMenu(this BuilderHandle builder, string objectName);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    public extern static LabelHandle GetLabel(this BuilderHandle builder, string objectName);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    public extern static HeaderBarHandle GetHeaderBar(this BuilderHandle builder, string objectName);

    public static BuilderHandle FromString(string ui)
        => _FromString(ui, -1);

    public static BuilderHandle GetObject<THandle>(this BuilderHandle builder, string objectName, Action<WindowHandle> withObject)
            where THandle : WindowHandle
                => builder.SideEffect(b => withObject((b.GetWidget(objectName) as WindowHandle)!));

    public static BuilderHandle GetObject<THandle>(this BuilderHandle builder, string objectName, Action<ButtonHandle> withObject)
        where THandle : ButtonHandle
            => builder.SideEffect(b => withObject((b.GetWidget(objectName) as ButtonHandle)!));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_new_from_resource", CallingConvention = CallingConvention.Cdecl)]
    extern static BuilderHandle _FromResource(string path);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_new_from_string", CallingConvention = CallingConvention.Cdecl)]
    extern static BuilderHandle _FromString(string ui, int length);

    // public static int AddFromFile(this IntPtr builder, string file) => AddFromFile(builder, file, IntPtr.Zero);

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_builder_new", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr New();


    // [DllImport(Libs.LibGtk, EntryPoint="gtk_builder_connect_signals_full", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr ConnectSignals(this IntPtr builder, ConnectDelegate onConnection);

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_builder_add_from_file", CallingConvention = CallingConvention.Cdecl)]
    // extern static int AddFromFile(this IntPtr builder, string file, IntPtr nil);
}

