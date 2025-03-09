namespace GtkDotNet.SafeHandles;

public class CairoWeakHandle : CairoHandle
{
    public static CairoWeakHandle Create(IntPtr raw)
    {
        var res = new CairoWeakHandle();
        res.handle = raw;
        return res;
    }
        
    public CairoWeakHandle() : base() {}

    protected override bool ReleaseHandle()  => true;
}
