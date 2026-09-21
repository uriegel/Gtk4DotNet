using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class EditableLabel : Widget
{
    public bool IsEditing { get => GetEditing(this); }

    public static EditableLabel New(string text)
    {
        var res = _New(text);
        res.CheckDiagnostics();
        return res;
    }

    /// <summary>
    /// Is the label text selectable?
    /// </summary>
    public EllipsizeMode Ellipsize
    {
        get => FindLabel(this)?.Ellipsize ?? EllipsizeMode.None;
        set => FindLabel(this)?.Ellipsize = value;
    }

    public  void StartEditing() => StartEditing(this);

    public void StopEditing(bool commit) => StopEditing(this, commit);
    
    public Editable AsEditable() => new(this);  

    public EditableLabel() : base() { }

    public EditableLabel(Builder builder, string? name = null) : base(builder, name) { }

    public EditableLabel(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }


    static Label? FindLabel(EditableLabel el)
    {
        var stack = el.GetChildren().FirstOrDefault(n => n.WidgetName == "GtkStack");
        var labelPtr = stack?.GetChildren().FirstOrDefault(n => n.WidgetName == "GtkLabel");
        if (labelPtr == null)
            return null;
        var label = new Label();
        label.SetInternalHandle(labelPtr.GetInternalHandle());
        label.AutoDestroyed = true;
        label.CheckDiagnostics();
        return label;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_label_new", CallingConvention = CallingConvention.Cdecl)]
    extern static EditableLabel _New(string label);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_label_get_editing", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetEditing(EditableLabel editableLabel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_label_start_editing", CallingConvention = CallingConvention.Cdecl)]
    extern static void StartEditing(EditableLabel editableLabel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_label_stop_editing", CallingConvention = CallingConvention.Cdecl)]
    extern static void StopEditing(EditableLabel editableLabel, bool commit);
}