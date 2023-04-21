using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

[StructLayout(LayoutKind.Sequential)]
struct GTypeInfo {
    // interface types, classed types, instantiated types 
    public ushort class_size;
    public IntPtr base_init;
    public IntPtr base_finalize;
    // interface types, classed types, instantiated types 
    public IntPtr class_init;
    public IntPtr class_finalize;
    public IntPtr class_data;
    // instantiated types 
    public ushort instance_size;
    public IntPtr instance_init;
    public ushort n_preallocs;
    // value handling 
    public IntPtr value_table;
};

