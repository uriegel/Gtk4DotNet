using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class Grid
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_grid_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New();

    [DllImport(Globals.LibGtk, EntryPoint="gtk_grid_attach", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Attach(this IntPtr grid, IntPtr widget, int column, int row, int columnSpan, int rowSpan);
}

