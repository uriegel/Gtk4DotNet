using System.Diagnostics;
using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;
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
    public void OnShowProcesses(Action<string?, string[], Process[]> onChanged)
        => SignalConnect<FivePointerDelegate>("show-processes", (_, msg, pids, cptr, _) =>
        {
            var text = msg.PtrToString(false);
            var choices = ReadNullTerminatedStringArray(cptr);
            var ints = ReadInts(pids);
            var processes = ints.Select(n => Process.GetProcessById(n)).ToArray();
            onChanged(text, choices, processes);
        });
    public void ShowUnmountProgress(Action<string?, string?, ulong, ulong> onProgress)
        => SignalConnect<ShowUnmountProgressDelgate>("show-unmount-progress", (_, msg, tleft, bleft, _) =>
        {
            var msgs = msg.PtrToString(false)?.Split('\n') ?? [];
            onProgress(msgs.FirstOrDefault(), msgs.Skip(1).FirstOrDefault(), tleft, bleft);
        });

    static string[] ReadNullTerminatedStringArray(nint ptr)
    {
        var result = new List<string>();

        int offset = 0;
        while (true)
        {
            var strPtr = Marshal.ReadIntPtr(ptr, offset);

            if (strPtr == IntPtr.Zero)
                break;

            result.Add(Marshal.PtrToStringUTF8(strPtr)!);
            offset += IntPtr.Size;
        }

        return [.. result];
    }

    static int[] ReadInts(nint intPtr)
    {
        var array = Marshal.PtrToStructure<GArray>(intPtr);
        var pids = new int[array.Len];
        for (int i = 0; i < array.Len; i++)
            pids[i] = Marshal.ReadInt32(array.Data, i * sizeof(int));
        return pids;
    }

    [DllImport(Libs.LibGio, EntryPoint = "g_mount_operation_new", CallingConvention = CallingConvention.Cdecl)]
    extern static MountOperation _New();
}

delegate void ShowUnmountProgressDelgate(nint _, nint msg, ulong timeLeft, ulong bytesLeft, nint __);