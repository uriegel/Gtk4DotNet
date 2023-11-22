using System.Runtime.CompilerServices;

namespace GtkDotNet.SafeHandles;

public class WidgetRef<THandle>
    where THandle: WidgetHandle, new()  
{
    public THandle Ref { get => Handle ?? new(); } 
    internal THandle? Handle { get; set; }
}
