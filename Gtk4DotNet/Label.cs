using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class Label
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_label_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static LabelHandle New(string text);

    public static LabelHandle Set(this LabelHandle label, string text)
        => label.SideEffect(l => l._Set(text));
    
    public  static LabelHandle SetSelectable(this LabelHandle label, bool selectable)
        => label.SideEffect(l => l._SetSelectable(selectable));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_label_set_label", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Set(this LabelHandle label, string text);
    
    [DllImport(Libs.LibGtk, EntryPoint="gtk_label_set_selectable", CallingConvention = CallingConvention.Cdecl)]
    extern static void _SetSelectable(this LabelHandle label, bool selectable);
}


