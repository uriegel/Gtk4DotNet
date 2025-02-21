using Microsoft.Win32.SafeHandles;

namespace GtkDotNet.SafeHandles;

public abstract class BaseHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    public BaseHandle()
        : base(ownsHandle: true) { }

    public BaseHandle(nint handle)
        : base(ownsHandle: true) => this.handle = handle;

    public nint GetInternalHandle() => handle;

    public void SetInternalHandle(nint handle) => this.handle = handle;

    protected override bool ReleaseHandle() => true;
    //     => NativeMethods.CloseHandle(handle);

    internal IntPtr TakeHandle() 
    {
        var result = handle;
        handle = 0;
        return result;
    }

    // - There is no need to implement a finalizer, MySafeHandle already has one
    // - You do not need to protect against multiple disposing, MySafeHandle already does
}


// When working with unmanaged resources, you should consider:

// Using an existing SafeHandle if possible
// If not possible, subclass SafeHandle to create one that meets your needs. This class should not do anything more than managing unmanaged resources. It should be sealed.
// If that's not possible, create your class which implements IDisposable and a finalizer
// The class should be sealed
// If sealing the class is not possible, add a method protected void Dispose(bool disposing), so subclasses can implements the dispose pattern correctly.
