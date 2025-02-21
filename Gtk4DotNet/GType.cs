using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

namespace GtkDotNet;

public static class GType
{
    [DllImport(Libs.LibGLib, EntryPoint = "g_type_class_peek", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle PeekClass(GTypeHandle gtype);

    [DllImport(Libs.LibGLib, EntryPoint = "g_type_register_static", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle RegisterStatic(GTypeHandle parentType, string typeName, ref GTypeInfo info, TypeFlags fags = TypeFlags.None);

    [DllImport(Libs.LibGLib, EntryPoint = "g_type_class_ref", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle RefClass(GTypeHandle gtype);

    public static GTypeHandle Get(GTypeEnum type)
    => type switch
    {
        GTypeEnum.Box => Box.Type(),
        GTypeEnum.Button => Button.Type(),
        GTypeEnum.ComboBoxText => ComboBoxText.Type(),
        GTypeEnum.Dialog => Dialog.Type(),
        GTypeEnum.DrawingArea => DrawingArea.Type(),
        GTypeEnum.GObject => GObject.Type(),
        GTypeEnum.Label => Label.Type(),
        GTypeEnum.ListBox => ListBox.Type(),
        GTypeEnum.MenuButton => MenuButton.Type(),
        GTypeEnum.Popover => Popover.Type(),
        GTypeEnum.ProgressBar => ProgressBar.Type(),
        GTypeEnum.Revealer => Revealer.Type(),
        GTypeEnum.TextView => TextView.Type(),
        GTypeEnum.ToggleButton => ToggleButton.Type(),
        GTypeEnum.Widget => Widget.Type(),
        GTypeEnum.Window => Window.Type(),
        _ => GObject.Type(),
    };
}