using System.Runtime.InteropServices;

namespace Gtk4DotNet;

// TODO Release ready

public class DropDown : Widget
{
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