namespace Gtk4DotNet;

class ManagedGInputStream(InputStream stream) : Stream
{
    public override void Close() 
        => stream.Dispose();

    #region Stream

    public override bool CanRead => true;

    public override bool CanSeek => false;

    public override bool CanWrite => false;

    public override long Length => throw new NotImplementedException();

    public override long Position  { get; set; }

    public override void Flush() 
    {

    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        unsafe 
        {
            fixed (byte* ptr = buffer)
            {
                var read = stream.Read((nint)ptr, count);
                return read;
            }
        }
    }

    public override long Seek(long offset, SeekOrigin origin) 
    {
        return 0;
    }

    public override void SetLength(long value) => throw new NotImplementedException();

    public override void Write(byte[] buffer, int offset, int count)=> throw new NotImplementedException();
    
    #endregion
}

