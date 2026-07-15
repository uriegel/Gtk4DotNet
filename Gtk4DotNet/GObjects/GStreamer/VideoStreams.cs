using System.Collections;
using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class VideoStreams : IDisposable, IEnumerable<VideoInfo>
{
    #region IEnumerator

    public IEnumerator<VideoInfo> GetEnumerator()
    {
        var current = list;
        while (current != 0)
        {
            var node = Marshal.PtrToStructure<GList>(current);
            var app = node.Data;
            yield return new VideoInfo(app);
            current = node.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    internal VideoStreams(nint list) => this.list = list;

    nint list;

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl)]
    static extern void gst_discoverer_stream_info_list_free(nint list);

    #region IDisposable

    public void Dispose()
    {
        // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // Verwalteten Zustand (verwaltete Objekte) bereinigen
            }

            // Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
            // Große Felder auf NULL setzen
            if (list != 0)
                gst_discoverer_stream_info_list_free(list);
            list = 0;
            disposedValue = true;
        }
    }
 
    // Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
    ~VideoStreams()
    {
        // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
        Dispose(disposing: false);
    }

    bool disposedValue;

    #endregion
}