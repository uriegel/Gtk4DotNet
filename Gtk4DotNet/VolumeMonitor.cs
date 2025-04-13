using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class VolumeMonitor
{
    [DllImport(Libs.LibGtk, EntryPoint = "g_volume_monitor_get", CallingConvention = CallingConvention.Cdecl)]
    public extern static VolumeMonitorHandle Get();

    public static VolumeHandle[] GetVolumes(this VolumeMonitorHandle volumeMonitor)
    {
        var volumes = _GetVolumes(volumeMonitor);
        nint current = volumes;
        var list = new List<VolumeHandle>();
        while (current != 0)
        {
            var glist = Marshal.PtrToStructure<GList>(current);
            var volume = new VolumeHandle();
            list.Add(volume);
            volume.SetInternalHandle(glist.Data);
            current = glist.Next;
        }
        GList.Free(volumes);
        return list.ToArray();
    }

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_monitor_get_volumes", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetVolumes(this VolumeMonitorHandle volumeMonitor);
}
