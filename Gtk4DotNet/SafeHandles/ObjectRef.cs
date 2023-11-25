namespace GtkDotNet.SafeHandles;

public class ObjectRef<THandle>
    where THandle: ObjectHandle, new()  
{
    public THandle Ref { get => Handle ?? new(); } 

    public void SetHandle<T>(Action<THandle> setHandle)
        where T: ObjectHandle, new()  
    {
        if (Handle != null)
            setHandle(Handle);

        // TODO if StackSwitcher is disposed, remove this eventhandler
        Changed += () =>
        {
            if (Handle != null)
                setHandle(Handle);
        };
    }

    internal THandle? Handle 
    { 
        get => _Handle;
        set
        {
            _Handle = value;
            Changed?.Invoke();
        }  
    }
    event Action? Changed;

    THandle? _Handle;
}
