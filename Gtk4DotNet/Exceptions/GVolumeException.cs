namespace GtkDotNet.Exceptions;

public class VolumeException : GtkException
{
    public VolumeError ErrorType { get; }

    public string? Name { get; }
    public string? UnixDevice { get; }

    internal VolumeException(string message, string? name, string? unixDevice, GErrorStruct error)
        : base(message)
    {
        Name = name;
        UnixDevice = unixDevice;
        ErrorType = error.Domain switch
        {
            236 or 232 or 205 or 202 => error.Code switch
            {
                _ => VolumeError.General,
            },
            _ => VolumeError.General,
        };
    }
}