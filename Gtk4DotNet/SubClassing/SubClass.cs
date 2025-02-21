using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class SubClass<THandle>
    where THandle : ObjectHandle
{
    public GTypeHandle Type { get; }

    public SubClass(Parent parent, string name, Func<nint, SubClassInst<THandle>> constructor)
    {
        this.constructor = constructor;
        GTypeHandle parentType = InitializeParentType();

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

        GTypeHandle InitializeParentType()
            => parent switch
            {
                Parent.Box => Box.Type(),
                Parent.Button => Button.Type(),
                Parent.ComboBoxText => ComboBoxText.Type(),
                Parent.Dialog => Dialog.Type(),
                Parent.DrawingArea => DrawingArea.Type(),
                Parent.GObject => GObject.Type(),
                Parent.Label => Label.Type(),
                Parent.ListBox => ListBox.Type(),
                Parent.MenuButton => MenuButton.Type(),
                Parent.Popover => Popover.Type(),
                Parent.ProgressBar => ProgressBar.Type(),
                Parent.Revealer => Revealer.Type(),
                Parent.TextView => TextView.Type(),
                Parent.ToggleButton => ToggleButton.Type(),
                Parent.Widget => Widget.Type(),
                Parent.Window => Window.Type(),
                _ => GObject.Type(),
            };

        ushort GetParentInstanceSize()
            => parent switch
            {
                Parent.GObject => (ushort)Marshal.SizeOf<GObjectType>(),
                _ => RetrieveParentInstanceSize()
            };

        ushort GetParentClassSize()
            => parent switch
            {
                Parent.GObject => (ushort)Marshal.SizeOf<GObjectClass>(),
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

    protected virtual void ClassInit(IntPtr gClass, IntPtr classData)
    {
        IntPtr finalizePtr = Marshal.GetFunctionPointerForDelegate(SubClassInst<THandle>.finalizeDelegate);
        Marshal.WriteIntPtr(gClass, 6 * IntPtr.Size, finalizePtr);
    }

    protected virtual void InstanceInit(IntPtr gClass, IntPtr classData)
    {
    }

    Func<nint, SubClassInst<THandle>> constructor;
}


[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void DisposeCallback(IntPtr obj);
delegate void SubClassInitDelegate(IntPtr gClass, IntPtr classData);
delegate void SubClassInstanceInitDelegate(IntPtr gClass, IntPtr classData);

