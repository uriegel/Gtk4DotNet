namespace GtkDotNet.SafeHandles;

public class FileInfoHandle : ObjectHandle
{
    public FileInfoHandle() : base() { }
    public FileInfoHandle(nint obj) : base() => SetInternalHandle(obj);
}