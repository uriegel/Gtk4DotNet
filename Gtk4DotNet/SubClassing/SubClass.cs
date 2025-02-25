using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class SubClass<THandle>
    where THandle : ObjectHandle
{
    public GTypeHandle Type { get; }

    public SubClass(GTypeEnum parent, string name, Func<nint, SubClassInst<THandle>> constructor)
    {
        this.constructor = constructor;
        GTypeHandle parentType = GType.Get(parent);

        var typeInfo = new GTypeInfo()
        {
            classSize = GetParentClassSize(),
            instanceSize = GetParentInstanceSize(),
            classInit = Marshal.GetFunctionPointerForDelegate<SubClassInitDelegate>(ClassInit),
            instanceInit = Marshal.GetFunctionPointerForDelegate<SubClassInstanceInitDelegate>(InstanceInit)
        };
        Type = GType.RegisterStatic(parentType, name, ref typeInfo);
        if (Type.IsInvalid)
            throw new Exception("Custom sub class could not be registered");

        ushort GetParentInstanceSize()
            => parent switch
            {
                GTypeEnum.GObject => (ushort)Marshal.SizeOf<GObjectType>(),
                _ => RetrieveParentInstanceSize()
            };

        ushort GetParentClassSize()
            => parent switch
            {
                GTypeEnum.GObject => (ushort)Marshal.SizeOf<GObjectClass>(),
                _ => RetrieveParentClassSize()
            };

        ushort RetrieveParentClassSize()
        {
            var classType = GType.PeekClass(parentType).Pipe(n => n.IsInvalid ? GType.RefClass(parentType) : n);
            if (classType.IsInvalid)
                throw new Exception("Failed to get parent class size");
            return (ushort)Marshal.ReadInt32(classType.GetInternalHandle()); // Read size from the first field of class struct
        }

        ushort RetrieveParentInstanceSize()
        {
            var classType = GType.PeekClass(parentType).Pipe(n => n.IsInvalid ? GType.RefClass(parentType) : n);
            if (classType.IsInvalid)
                throw new Exception("Failed to get parent instance size");
            return (ushort)Marshal.ReadInt32(classType.GetInternalHandle(), IntPtr.Size); // Read size from the second field of class struct
        }
    }

    public SubClassInst<THandle> New()
    {
        var ptr = GObject.New(Type, 0);
        return constructor(ptr);
    }

    protected virtual void ClassInit(nint cls, nint _)
    {
        Marshal.WriteIntPtr(cls, 3 * nint.Size, Marshal.GetFunctionPointerForDelegate(SubClassInst<THandle>.setPropertyDelegate));
        Marshal.WriteIntPtr(cls, 4 * nint.Size, Marshal.GetFunctionPointerForDelegate(SubClassInst<THandle>.getPropertyDelegate));
        Marshal.WriteIntPtr(cls, 6 * nint.Size, Marshal.GetFunctionPointerForDelegate(SubClassInst<THandle>.finalizeDelegate));
    }

    protected void RegisterProperty(nint cls, uint id, string name, string? defaultValue = null)
        => GObject.ClassInstallProperty(cls, id, GObject.ParamSpecString(
            name,
            null,
            null,
            defaultValue, ParamFlags.ReadWrite));

    protected virtual void InstanceInit(nint obj, nint _)
        => constructor(obj);

    protected void InitTemplateFromResource(nint cls, string name)
        => cls.ClassSetTemplateFromDotNetResource(name);

    readonly Func<nint, SubClassInst<THandle>> constructor;
}


[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void DisposeDelegate(nint obj);
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void SubClassInitDelegate(nint gClass, nint classData);
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void SubClassInstanceInitDelegate(nint gClass, nint classData);
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void SetPropertyDelegate(nint obj, uint propId, nint value, nint pspec);
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void GetPropertyDelegate(nint obj, uint propId, nint value, nint pspec);

