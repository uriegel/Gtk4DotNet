using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class MountOperation : GObject
{
    public static MountOperation New()
    {
        var op = _New();
        op.CheckDiagnostics();
        return op;
    }

    public void OnAskQuestion(Action onChanged)
        => SignalConnect<FourPointerDelegate>("ask-question", (_, _, _, _) => onChanged());

    [DllImport(Libs.LibGio, EntryPoint = "g_mount_operation_new", CallingConvention = CallingConvention.Cdecl)]
    extern static MountOperation _New();
   
}
