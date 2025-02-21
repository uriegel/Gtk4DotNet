using System.Runtime.InteropServices;

namespace GtkDotNet.SubClassing;

[StructLayout(LayoutKind.Sequential)]
public struct GObjectClass
{
    public IntPtr g_type_class; // GTypeClass* (pointer to parent type class)

    IntPtr construct_properties;
    // Virtual function pointers
    public IntPtr constructor;
    public IntPtr set_property;
    public IntPtr get_property;
    public IntPtr dispose;
    public IntPtr finalize;

    public IntPtr dispatch_properties_changed;
    public IntPtr notify;
    IntPtr constructed;
    ulong flags;
    ulong n_construct_properties;
    IntPtr specs;
    ulong size;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public IntPtr[] padding; // Reserved for future expansion
   
}

[StructLayout(LayoutKind.Sequential)]
public struct GObjectType
{
    public IntPtr g_type_instance; // Pointer to GObjectClass
    public uint ref_count;         // Reference count
    public IntPtr qdata;
}