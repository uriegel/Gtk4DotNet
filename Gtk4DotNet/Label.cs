using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class Label
{
    public static LabelHandle New() => New(0);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static LabelHandle New(string text);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    public static LabelHandle Set(this LabelHandle label, string? text)
        => label.SideEffect(l => l._Set(text));

    public static LabelHandle SetSelectable(this LabelHandle label, bool selectable)
        => label.SideEffect(l => l._SetSelectable(selectable));

    public static LabelHandle UseUnderline(this LabelHandle label)
        => label.SideEffect(l => l.SetUseUnderline(true));

    public static LabelHandle MnemonicWidget(this LabelHandle label, WidgetHandle widget)
        => label.SideEffect(l => l.SetMnemonicWidget(widget));

    public static LabelHandle MnemonicWidget<THandle>(this LabelHandle label, ObjectRef<THandle> widget)
        where THandle : WidgetHandle, new()
        => label.SideEffect(l => widget.SetHandle<THandle>(w => l.SetMnemonicWidget(w)));

    public static LabelHandle XAlign(this LabelHandle label, float xalign)
        => label.SideEffect(l => l.SetXAlign(xalign));

    public static LabelHandle Ellipsize(this LabelHandle label, EllipsizeMode mode)
        => label.SideEffect(l => l.SetEllipsize(mode));

    public static string? GetLabel(this LabelHandle label)
        => Marshal.PtrToStringUTF8(_GetLabel(label));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_get_label", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetLabel(this LabelHandle label);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_label", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Set(this LabelHandle label, string? text);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_selectable", CallingConvention = CallingConvention.Cdecl)]
    extern static void _SetSelectable(this LabelHandle label, bool selectable);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_use_underline", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetUseUnderline(this LabelHandle label, bool underline);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_mnemonic_widget", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetMnemonicWidget(this LabelHandle label, WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_xalign", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetXAlign(this LabelHandle label, float xalign);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_new", CallingConvention = CallingConvention.Cdecl)]
    extern static LabelHandle New(nint _);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_ellipsize", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetEllipsize(this LabelHandle label, EllipsizeMode mode);
}


