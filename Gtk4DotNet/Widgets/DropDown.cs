using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class DropDown : Widget
{
    /// <summary>
    /// The index of the selected DropDown.
    /// </summary>
    public int SelectedPos
    {
        get => GetSelected(this);
        set => SetSelected(this, value);
    }
    public DropDown() : base() { }

    public DropDown(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_drop_down_get_selected", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetSelected(DropDown dropDown);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_drop_down_set_selected", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSelected(DropDown dropDown, int pos);
}