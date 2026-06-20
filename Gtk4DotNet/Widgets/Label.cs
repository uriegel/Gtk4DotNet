using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

// TODO Release ready

public class Label : Widget
{
    public string? Text
    {
        get => _GetLabel(this).PtrToString(false);
        set => _Set(this, value);
    }

    public bool Selectable
    {
        get => GetSelectable(this);
        set => SetSelectable(this, value);
    }

    public static Label New()
    {
        var res = New(0);
        res.CheckDiagnostics();
        return res;
    }

    public static Label New(string? text)
    {
        var res = _New(text ?? "");
        res.CheckDiagnostics();
        return res;
    }

    public Label() : base() { }

    public Label(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_new", CallingConvention = CallingConvention.Cdecl)]
    extern static Label _New(string text);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_new", CallingConvention = CallingConvention.Cdecl)]
    extern static Label New(nint nil);

    // TODO implement

    // public static LabelHandle SetSelectable(this LabelHandle label, bool selectable)
    //     => label.SideEffect(l => l._SetSelectable(selectable));

    // public static LabelHandle UseUnderline(this LabelHandle label)
    //     => label.SideEffect(l => l.SetUseUnderline(true));

    // public static LabelHandle MnemonicWidget(this LabelHandle label, WidgetHandle widget)
    //     => label.SideEffect(l => l.SetMnemonicWidget(widget));

    // public static LabelHandle MnemonicWidget<THandle>(this LabelHandle label, ObjectRef<THandle> widget)
    //     where THandle : WidgetHandle, new()
    //     => label.SideEffect(l => widget.SetHandle<THandle>(w => l.SetMnemonicWidget(w)));

    // public static LabelHandle XAlign(this LabelHandle label, float xalign)
    //     => label.SideEffect(l => l.SetXAlign(xalign));

    // public static LabelHandle Ellipsize(this LabelHandle label, EllipsizeMode mode)
    //     => label.SideEffect(l => l.SetEllipsize(mode));

    // public static string? GetLabel(this LabelHandle label)
    //     => Marshal.PtrToStringUTF8(_GetLabel(label));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_get_label", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetLabel(Label label);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_label", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Set(Label label, string? text);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_selectable", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSelectable(Label label, bool selectable);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_get_selectable", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetSelectable(Label label);

    // [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_use_underline", CallingConvention = CallingConvention.Cdecl)]
    // extern static void SetUseUnderline(this LabelHandle label, bool underline);

    // [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_mnemonic_widget", CallingConvention = CallingConvention.Cdecl)]
    // extern static void SetMnemonicWidget(this LabelHandle label, WidgetHandle widget);

    // [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_xalign", CallingConvention = CallingConvention.Cdecl)]
    // extern static void SetXAlign(this LabelHandle label, float xalign);

    // [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_new", CallingConvention = CallingConvention.Cdecl)]
    // extern static LabelHandle New(nint _);

    // [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_ellipsize", CallingConvention = CallingConvention.Cdecl)]
    // extern static void SetEllipsize(this LabelHandle label, EllipsizeMode mode);
}

public static class LabelExtensions
{
   public static THandle Selectable<THandle>(this THandle widget, bool value = true)
        where THandle : Label
        => widget.SideEffect(w => widget.Selectable = value);
}
