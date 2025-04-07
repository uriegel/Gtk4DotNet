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

    [DllImport(Libs.LibGtk, EntryPoint = "g_type_ensure")]
    public static extern void Ensure(GTypeHandle gtype);

    [DllImport(Libs.LibGtk, EntryPoint = "g_type_query")]
    public static extern void Query(this GTypeHandle gtype, ref GTypeQuery query);


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
        GTypeEnum.ApplicationWindow => ApplicationWindow.Type(),
        GTypeEnum.WebKitWebView => WebKit.Type(),
        GTypeEnum.ColumnView => ColumnView.Type(),
        GTypeEnum.ScrolledWindow => ScrolledWindow.Type(),
        GTypeEnum.Paned => Paned.Type(),
        GTypeEnum.AdwApplicationWindow => AdwApplicationWindow.Type(),
        GTypeEnum.AdwDialog => AdwDialog.Type(),
        GTypeEnum.AdwAlertDialog => AdwAlertDialog.Type(),
        _ => GObject.Type(),
    };

    public static int SignalNew(GTypeHandle gtype, string name, SignalFlags signalFlags, GTypes returnType, GTypes[] types)
        => types.Length switch
        {
            0 => SignalNew0(name, gtype, signalFlags, 0, 0, 0, 0, returnType, 0),
            1 => SignalNew1(name, gtype, signalFlags, 0, 0, 0, 0, returnType, 1, types[0]),
            2 => SignalNew2(name, gtype, signalFlags, 0, 0, 0, 0, returnType, 2, types[0], types[1]),
            3 => SignalNew3(name, gtype, signalFlags, 0, 0, 0, 0, returnType, 2, types[0], types[1], types[2]),
            _ => throw new Exception("Too many GType arguments")
        };

    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_new", CallingConvention = CallingConvention.Cdecl)]
    extern static int SignalNew0(string name, GTypeHandle gtype, SignalFlags signalFlags, nint classClosure, nint accumulator, nint accuData, nint cMarshaller,
        GTypes returnType, int nParams);

    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_new", CallingConvention = CallingConvention.Cdecl)]
    extern static int SignalNew1(string name, GTypeHandle gtype, SignalFlags signalFlags, nint classClosure, nint accumulator, nint accuData, nint cMarshaller,
        GTypes returnType, int nParams, GTypes paramType1);

    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_new", CallingConvention = CallingConvention.Cdecl)]
    extern static int SignalNew2(string name, GTypeHandle gtype, SignalFlags signalFlags, nint classClosure, nint accumulator, nint accuData, nint cMarshaller,
        GTypes returnType, int nParams, GTypes paramType1, GTypes paramType2);

    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_new", CallingConvention = CallingConvention.Cdecl)]
    extern static int SignalNew3(string name, GTypeHandle gtype, SignalFlags signalFlags, nint classClosure, nint accumulator, nint accuData, nint cMarshaller,
        GTypes returnType, int nParams, GTypes paramType1, GTypes paramType2, GTypes paramType3);
}