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
    //     => NativeMethods.CloseHandle(handle);

    // - There is no need to implement a finalizer, MySafeHandle already has one
    // - You do not need to protect against multiple disposing, MySafeHandle already does
}


// When working with unmanaged resources, you should consider:

// Using an existing SafeHandle if possible
// If not possible, subclass SafeHandle to create one that meets your needs. This class should not do anything more than managing unmanaged resources. It should be sealed.
// If that's not possible, create your class which implements IDisposable and a finalizer
// The class should be sealed
// If sealing the class is not possible, add a method protected void Dispose(bool disposing), so subclasses can implements the dispose pattern correctly.
