using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace GtkDotNet;

public class Cancellable : IDisposable
{
    public Cancellable() => handle = New();

    public Cancellable(CancellationToken cancellationToken) : this()
        => cancellationToken.Register(Cancel);
    
    public void Cancel() => Cancel(handle);

    [DllImport(Globals.LibGio, EntryPoint = "g_cancellable_new", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr New();

    [DllImport(Globals.LibGio, EntryPoint = "g_cancellable_cancel", CallingConvention = CallingConvention.Cdecl)]
    extern static void Cancel(IntPtr handle);

    internal IntPtr handle;

    #region IDisposable

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            //if (disposing)
                // Verwalteten Zustand (verwaltete Objekte) bereinigen
           

            // Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
            // Große Felder auf NULL setzen
            
            GObject.Unref(handle);
            disposedValue = true;
        }
    }

    //  Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
    ~Cancellable()
        // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
        => Dispose(disposing: false);

    public void Dispose()
    {
        // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    bool disposedValue;
    
    #endregion
}