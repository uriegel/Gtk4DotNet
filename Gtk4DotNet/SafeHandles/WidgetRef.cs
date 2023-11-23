namespace GtkDotNet.SafeHandles;

public class WidgetRef<THandle>
    where THandle: WidgetHandle, new()  
{
    public event Action? Changed;
    public THandle Ref { get => Handle ?? new(); } 
    internal THandle? Handle 
    { 
        get => _Handle;
        set
        {
            _Handle = value;
            Changed?.Invoke();
        }  
    }
    THandle? _Handle;
}
