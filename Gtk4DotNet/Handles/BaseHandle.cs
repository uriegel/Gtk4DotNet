using CsTools.Extensions;
using Microsoft.Win32.SafeHandles;

namespace Gtk4DotNet;

// Release ready

public abstract class BaseHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    public BaseHandle()
        : base(ownsHandle: true) { }

    internal nint GetInternalHandle() => handle;

    internal void SetInternalHandle(nint handle) => this.handle = handle;

    /// <summary>
    /// Always override this method!
    /// </summary>
    /// <returns></returns>
    protected override bool ReleaseHandle()
    {
        Console.Error.WriteLine("You have to override 'ReleaseHandle' in a from BaseHandle inherited object");
        return false;
    }

    // - There is no need to implement a finalizer, MySafeHandle already has one
    // - You do not need to protect against multiple disposing, MySafeHandle already does
}

public static class BaseHandleExtensions
{
    public static THandle With<THandle>(this THandle handle, Action<THandle> action)
        where THandle : BaseHandle
        => handle.SideEffect(action);

    public static THandle If<THandle>(this THandle handle, Predicate<THandle> predicate, Action<THandle> action)
        where THandle : BaseHandle
        => handle.SideEffectIf(predicate(handle), action);

    public static THandle If<THandle>(this THandle handle, bool predicate, Action<THandle> action)
        where THandle : BaseHandle
        => handle.SideEffectIf(predicate, action);

    public static THandle Choose<THandle>(this THandle handle, Predicate<THandle> predicate, Action<THandle> trueAction, Action<THandle> falseAction)
        where THandle : BaseHandle
        => handle.SideEffectChoose(predicate(handle), trueAction, falseAction);

    public static THandle Choose<THandle>(this THandle handle, bool predicate, Action<THandle> trueAction, Action<THandle> falseAction)
        where THandle : BaseHandle
        => handle.SideEffectChoose(predicate, trueAction, falseAction);
}

// When working with unmanaged resources, you should consider:

// Using an existing SafeHandle if possible
// If not possible, subclass SafeHandle to create one that meets your needs. This class should not do anything more than managing unmanaged resources. It should be sealed.
// If that's not possible, create your class which implements IDisposable and a finalizer
// The class should be sealed
// If sealing the class is not possible, add a method protected void Dispose(bool disposing), so subclasses can implements the dispose pattern correctly.
