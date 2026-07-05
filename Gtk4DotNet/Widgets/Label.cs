using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public class Label : Widget
{
    /// <summary>
    /// The text of this label
    /// </summary>
    public string Text
    {
        get => _GetLabel(this).PtrToString(false) ?? "";
        set => _Set(this, value);
    }

    /// <summary>
    /// Is the label text selectable?
    /// </summary>
    public bool Selectable
    {
        get => GetSelectable(this);
        set => SetSelectable(this, value);
    }

    public float XAlign
    {
        get => GetXAlign(this);
        set => SetXAlign(this, value);
    }

    public float YAlign
    {
        get => GetYAlign(this);
        set => SetYAlign(this, value);
    }

    /// <summary>
    /// Is this label text underlined?
    /// </summary>
    public bool UseUnderline
    {
        get => GetUseUnderline(this);
        set => SetUseUnderline(this, value);
    }

    /// <summary>
    /// The ellipsization mode of the label.
    /// </summary>
    public EllipsizeMode Ellipsize
    {
        get => GetEllipsize(this);
        set => SetEllipsize(this, value);
    }

    /// <summary>
    /// If the label has been set so that it has an mnemonic key (using i.e. gtk_label_set_markup_with_mnemonic(), gtk_label_set_text_with_mnemonic(), gtk_label_new_with_mnemonic() or the “use_underline” property) the label can be associated with a widget that is the target of the mnemonic. When the label is inside a widget (like a GtkButton or a GtkNotebook tab) it is automatically associated with the correct widget, but sometimes (i.e. when the target is a GtkEntry next to the label) you need to set it explicitly using this function.
    /// The target widget will be accelerated by emitting the GtkWidget::mnemonic-activate signal on it. The default handler for this signal will activate the widget if there are no mnemonic collisions and toggle focus between the colliding widgets otherwise.
    /// </summary>
    public Widget MnemonicWidget { set => SetMnemonicWidget(this, value); }

    public Label SetUseUnderline() => this.SideEffect(l => SetUseUnderline(this, true));

    public Label SetMnemonicWidget(Widget widget) => this.SideEffect(l => SetMnemonicWidget(this, widget));

    public Label SetXAlign(float xalign) => this.SideEffect(l => SetXAlign(this, xalign));

    public Label SetEllipsize(EllipsizeMode mode) => this.SideEffect(l => SetEllipsize(this, mode));

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

    public Label(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_new", CallingConvention = CallingConvention.Cdecl)]
    extern static Label _New(string text);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_new", CallingConvention = CallingConvention.Cdecl)]
    extern static Label New(nint nil);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_get_label", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetLabel(Label label);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_label", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Set(Label label, string? text);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_selectable", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSelectable(Label label, bool selectable);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_get_selectable", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetSelectable(Label label);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_use_underline", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetUseUnderline(Label label, bool underline);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_get_use_underline", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetUseUnderline(Label label);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_mnemonic_widget", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetMnemonicWidget(Label label, Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_xalign", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetXAlign(Label label, float xalign);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_get_xalign", CallingConvention = CallingConvention.Cdecl)]
    extern static float GetXAlign(Label label);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_yalign", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetYAlign(Label label, float yalign);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_get_yalign", CallingConvention = CallingConvention.Cdecl)]
    extern static float GetYAlign(Label label);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_ellipsize", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetEllipsize(Label label, EllipsizeMode mode);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_get_ellipsize", CallingConvention = CallingConvention.Cdecl)]
    extern static EllipsizeMode GetEllipsize(Label label);
}

public static class LabelExtensions
{
    public static THandle Selectable<THandle>(this THandle widget, bool value = true)
         where THandle : Label
         => widget.SideEffect(w => widget.Selectable = value);
}
