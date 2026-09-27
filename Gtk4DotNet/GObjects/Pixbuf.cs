using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class Pixbuf : GObject, IPaintable
{
    public Pixbuf(string fileName)
    {
        var file = NewFromFile(fileName, 0);
        SetInternalHandle(file);
        CheckDiagnostics();
    }

    public void Rotate(PixbufRotation angle)
    {
        var newPic = Rotate(this, angle);
        var oldHandle = GetInternalHandle();
        Unref(oldHandle);
        SetInternalHandle(newPic);
        ResetDiagnostics();
        CheckDiagnostics();
    }

    public void ApplyEmbeddedOrientation()
    {
        var newPic = ApplyEmbeddedOrientation(this);
        var oldHandle = GetInternalHandle();
        Unref(oldHandle);
        SetInternalHandle(newPic);
        ResetDiagnostics();
        CheckDiagnostics();
    }
    
    nint IPaintable.GetRaw() => GetInternalHandle();

    [DllImport(Libs.LibGtk, EntryPoint = "gdk_pixbuf_new_from_file", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewFromFile(string filename, nint _);

    [DllImport(Libs.LibGtk, EntryPoint = "gdk_pixbuf_rotate_simple", CallingConvention = CallingConvention.Cdecl)]
    extern static nint Rotate(Pixbuf pic, PixbufRotation angle);
    [DllImport(Libs.LibGtk, EntryPoint = "gdk_pixbuf_apply_embedded_orientation", CallingConvention = CallingConvention.Cdecl)]
    extern static nint ApplyEmbeddedOrientation(Pixbuf pic);
}
