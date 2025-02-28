using System.Reflection.Metadata.Ecma335;
using CsTools.Extensions;

namespace GtkDotNet.SafeHandles;

public abstract class ObjectHandle : BaseHandle
{
    public ObjectHandle() : base() { }

    public bool IsFloating { get; set; }

    protected override bool ReleaseHandle()
    // => IsFloating
    //     || true.SideEffectIf(!IsFloating, _ => GObject.Unref(handle));

    {
        if (!IsFloating)
        {
            Console.WriteLine($"Mache was kauptt!!! {this.GetType()}");
            GObject.Unref(handle);
        }

        return true;
            }

}