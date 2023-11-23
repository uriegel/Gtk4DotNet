using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class StackSwitcher
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_stack_switcher_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static StackSwitcherHandle New();

    public static StackSwitcherHandle Stack(this StackSwitcherHandle stackSwitcher, StackHandle stack)
        => stackSwitcher.SideEffect(s => s.SetStack(stack));

    public static StackSwitcherHandle StackRef(this StackSwitcherHandle stackSwitcher, WidgetRef<StackHandle> stack)
    {
        if (stack.Handle != null)
            stackSwitcher.Stack(stack.Handle);

        // TODO if StackSwitcher is disposed, remove this eventhandler
        stack.Changed += () =>
        {
            if (stack.Handle != null)
                stackSwitcher.Stack(stack.Handle);
        };
        return stackSwitcher;
    }

    [DllImport(Libs.LibGtk, EntryPoint="gtk_stack_switcher_set_stack", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetStack(this StackSwitcherHandle stackSwitcher, StackHandle stack);
}

