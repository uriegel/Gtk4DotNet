using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class SubClass<THandle>
    where THandle : ObjectHandle, new()
{
    public GTypeHandle Type { get; }

    static internal ushort MemoryOffset { get; private set; }

    public SubClass(GTypeEnum parent, string typeName, Func<nint, SubClassInst<THandle>> constructor)
    {
        this.constructor = constructor;
        GTypeHandle parentType = GType.Get(parent);
        
        MemoryOffset = GetParentInstanceSize();
        GTypeQuery query = new();
        parentType.Query(ref query);
        Console.WriteLine($"{typeName}: {MemoryOffset} {typeof(THandle).FullName})]");
        initDelegate = ClassInit;
        instanceInitDelegate = InstanceInit;

        var typeInfo = new GTypeInfo()
        {
            classSize = query.classSize,
            instanceSize = (ushort)(MemoryOffset + nint.Size),
            classInit = Marshal.GetFunctionPointerForDelegate(initDelegate),
            instanceInit = Marshal.GetFunctionPointerForDelegate(instanceInitDelegate)
        };
        
        Type = GType.RegisterStatic(parentType, typeName, ref typeInfo);
        if (Type.IsInvalid)
            throw new Exception("Custom sub class could not be registered");

        ushort GetParentInstanceSize()
            => parent switch
            {
                GTypeEnum.GObject => (ushort)Marshal.SizeOf<GObjectType>(),
                _ => 500 // TODO smaller sizes
            };
    }

    protected virtual void ClassInit(nint cls, nint _)
    {
        Marshal.WriteIntPtr(cls, 3 * nint.Size, Marshal.GetFunctionPointerForDelegate(SubClassInst<THandle>.setPropertyDelegate));
        Marshal.WriteIntPtr(cls, 4 * nint.Size, Marshal.GetFunctionPointerForDelegate(SubClassInst<THandle>.getPropertyDelegate));
        Marshal.WriteIntPtr(cls, 6 * nint.Size, Marshal.GetFunctionPointerForDelegate(SubClassInst<THandle>.finalizeDelegate));
    }

    protected virtual void InstanceInit(nint obj, nint _)
    {
        var inst = constructor(obj);
        var gchandle = GCHandle.Alloc(inst, GCHandleType.Normal);
        Marshal.WriteIntPtr(obj, MemoryOffset, GCHandle.ToIntPtr(gchandle));
        inst.OnCreate();
    }

    protected void RegisterProperty(nint cls, int id, string name, string? defaultValue = null)
        => GObject.ClassInstallProperty(cls, id, GObject.ParamSpecString(
            name,
            null,
            null,
            defaultValue, ParamFlags.ReadWrite));

    protected int NewSignal(GTypeHandle type, string name, SignalFlags flags, GTypes returnType, GTypes[] param)
        => GType.SignalNew(type, name, flags, returnType, param);

    protected void InitTemplateFromResource(nint cls, string name)
        => cls.ClassSetTemplateFromDotNetResource(name);

    SubClassInitDelegate? initDelegate;
    SubClassInstanceInitDelegate? instanceInitDelegate;
    readonly Func<nint, SubClassInst<THandle>> constructor;
}


[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void DisposeDelegate(nint obj);
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void SubClassInitDelegate(nint gClass, nint classData);
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void SubClassInstanceInitDelegate(nint gClass, nint classData);
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void SetPropertyDelegate(nint obj, int propId, nint value, nint pspec);
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void GetPropertyDelegate(nint obj, int propId, nint value, nint pspec);

