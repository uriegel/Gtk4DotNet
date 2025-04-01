using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class EditableLabel
{
    public static EditableLabelHandle New() => New(0);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_label_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static EditableLabelHandle New(string text);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_label_start_editing", CallingConvention = CallingConvention.Cdecl)]
    public extern static void StartEditing(this EditableLabelHandle editableLabel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_label_stop_editing", CallingConvention = CallingConvention.Cdecl)]
    public extern static void StopEditing(this EditableLabelHandle editableLabel, bool commit);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_label_get_editing", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool IsEditing(this EditableLabelHandle editableLabel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_set_text", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetText(this EditableLabelHandle label, string text);

    public static string? GetText(this EditableLabelHandle editableLabel)
        => editableLabel._GetText().PtrToString(false);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_get_text", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetText(this EditableLabelHandle editableLabel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_label_new", CallingConvention = CallingConvention.Cdecl)]
    extern static EditableLabelHandle New(nint _);
}