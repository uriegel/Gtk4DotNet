using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class GObjectRef : IDisposable
{
    public IntPtr Value { get => obj; }
    
    public GObjectRef(IntPtr obj) => this.obj = obj;

    IntPtr obj;

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                GObject.Unref(obj);
                disposedValue = true;
            }
        }

        // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~GObjectRef() 
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            => Dispose(disposing: false);

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        bool disposedValue;

        #endregion
}


