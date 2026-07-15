using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class Picture : Widget
{
    public ContentFit ContentFit
    {
        get => GetContentFit(this);
        set => SetContentFit(this, value);
    }

    public bool CanShrink
    {
        get => GetCanShrink(this);
        set => SetCanShrink(this, value);
    }

    public bool KeepAspectRatio
    {
        get => GetKeepAspectRatio(this);
        set => SetKeepAspectRatio(this, value);
    }

    public static Picture New()
    {
        var res = _New();
        res.CheckDiagnostics();
        return res;
    }

    public static Picture NewForFile(GFile file)
    {
        var res = _NewForFile(file);
        res.CheckDiagnostics();
        return res;
    }

    public static Picture NewForFileName(string file)
    {
        var res = _NewForFileName(file);
        res.CheckDiagnostics();
        return res;
    }

    public void SetFileName(string file) => SetFileName(this, file);

    public void SetPaintable(IPaintable? paintable) => SetPaintable(this, paintable?.GetRaw() ?? 0);

    public Picture() : base() { }

    public Picture(Builder builder, string? name = null) : base(builder, name) { }

    public Picture(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_picture_new", CallingConvention = CallingConvention.Cdecl)]
    extern static Picture _New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_picture_new_for_file", CallingConvention = CallingConvention.Cdecl)]
    extern static Picture _NewForFile(GFile file);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_picture_new_for_filename", CallingConvention = CallingConvention.Cdecl)]
    extern static Picture _NewForFileName(string file);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_picture_set_file", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetFile(Picture picture, string file);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_picture_set_filename", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetFileName(Picture picture, string file);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_picture_set_paintable", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetPaintable(Picture picture, nint paintable);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_picture_get_content_fit", CallingConvention = CallingConvention.Cdecl)]
    extern static ContentFit GetContentFit(Picture picture);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_picture_set_content_fit", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetContentFit(Picture picture, ContentFit fit);

    [DllImport(Libs.LibGtk, EntryPoint = "get_can_shrink", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetCanShrink(Picture picture);

    [DllImport(Libs.LibGtk, EntryPoint = "set_can_shrink", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetCanShrink(Picture picture, bool shrink);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_picture_get_keep_aspect_ratio", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetKeepAspectRatio(Picture picture);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_picture_set_keep_aspect_ratio", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetKeepAspectRatio(Picture picture, bool aspectRatio);
}

