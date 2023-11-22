using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class Stack
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_stack_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static StackHandle New();

    public static StackHandle AddChild(this StackHandle stack, WidgetHandle widget)
        => stack.SideEffect(s => s._AddChild(widget));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_stack_add_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AddChild(this StackHandle grid, WidgetHandle widget);

    
}

