using System.Runtime.InteropServices;
using CsTools.Extensions;

namespace Gtk4DotNet;

public class Builder : GObject
{
    public Builder() : base() { }
    
    public static Builder FromDotNetResource(string path)
    {
        var ui = new StreamReader(Resources.Get(path)!).ReadToEnd();
        return _FromString(ui, -1);
    }

    public void GetWindow(Window window, string objectName)
        => window.SetInternalHandle(_GetWidget(this, objectName));
    
    public THandle GetWidget<THandle>(string objectName)
            where THandle : Widget, new()
    {
        var p = _GetWidget(this, objectName);
        var res = new THandle();
        res.SetInternalHandle(p);
        return res;
    }

    public static Builder FromString(string ui) => _FromString(ui, -1);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_new_from_resource", CallingConvention = CallingConvention.Cdecl)]
    extern static Builder _FromResource(string path);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_new_from_string", CallingConvention = CallingConvention.Cdecl)]
    extern static Builder _FromString(string ui, int length);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_builder_get_object", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetWidget(Builder builder, string objectName);
}

