using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class ComboBoxText
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_combo_box_text_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ComboBoxTextHandle New();

    [DllImport(Libs.LibGtk, EntryPoint="gtk_combo_box_text_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();        

    public static ComboBoxTextHandle Append(this ComboBoxTextHandle combobox, string id, string text)
        => combobox.SideEffect(c => c._Append(id, text));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_combo_box_text_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Append(this ComboBoxTextHandle combobox, string id, string text);
}
