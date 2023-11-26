using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class ListBox
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_new ", CallingConvention = CallingConvention.Cdecl)]
    public extern static ListBoxHandle New();

    // public static LabelHandle Set(this LabelHandle label, string text)
    //     => label.SideEffect(l => l._Set(text));

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

    // [DllImport(Libs.LibGtk, EntryPoint = "gtk_label_set_label", CallingConvention = CallingConvention.Cdecl)]
    // extern static void _Set(this LabelHandle label, string text);
}