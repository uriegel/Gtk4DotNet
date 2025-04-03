using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Entry
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_entry_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static EntryHandle New();

    public static EntryHandle Text(this EntryHandle textview, string text)
    {
        textview.SetText(text);
        return textview;
    }

    public static EntryHandle SelectRegion(this EntryHandle textView, int startPos, int endPos)
    {
        _SelectRegion(textView, startPos, endPos);
        return textView;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_entry_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    public static string? GetText(this EntryHandle textview)
        => textview._GetText().PtrToString(false);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_get_text", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetText(this EntryHandle textView);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_set_text", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetText(this EntryHandle textView, string text);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_select_region", CallingConvention = CallingConvention.Cdecl)]
    extern static void _SelectRegion(this EntryHandle textView, int startPos, int endPos);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_set_position", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetPosition(this EntryHandle textView, int position);
    
}
