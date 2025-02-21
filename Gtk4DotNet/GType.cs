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
}