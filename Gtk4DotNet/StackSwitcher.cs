using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class StackSwitcher
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_stack_switcher_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static StackSwitcherHandle New();

    public static StackSwitcherHandle Stack(this StackSwitcherHandle stackSwitcher, StackHandle stack)
        => stackSwitcher.SideEffect(s => s.SetStack(stack));

    public static StackSwitcherHandle StackRef(this StackSwitcherHandle stackSwitcher, ObjectRef<StackHandle> stack)
        => stackSwitcher.SideEffect(s => stack.SetHandle<StackHandle>(st => s.Stack(st)));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_stack_switcher_set_stack", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetStack(this StackSwitcherHandle stackSwitcher, StackHandle stack);
}

