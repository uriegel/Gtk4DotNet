using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public class EditableLabel : Widget
{
    public string Text
    {
        get => GetText(this).PtrToString(false) ?? "";
        set => SetText(this, value);
    }
    
    public bool IsEditing { get => GetEditing(this); }

    public static EditableLabel New(string text)
    {
        var res = _New(text);
        res.CheckDiagnostics();
        return res;
    }
    
    public  void StartEditing() => StartEditing(this);

    public  void StopEditing(bool commit) => StopEditing(this, commit);

    public EditableLabel() : base() { }

    public EditableLabel(Builder builder, string? name = null) : base(builder, name) { }


    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_label_new", CallingConvention = CallingConvention.Cdecl)]
    extern static EditableLabel _New(string label);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_label_get_editing", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetEditing(EditableLabel editableLabel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_label_start_editing", CallingConvention = CallingConvention.Cdecl)]
    extern static void StartEditing(EditableLabel editableLabel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_label_stop_editing", CallingConvention = CallingConvention.Cdecl)]
    extern static void StopEditing(EditableLabel editableLabel, bool commit);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_set_text", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetText(EditableLabel label, string text);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_get_text", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetText(EditableLabel editableLabel);

}