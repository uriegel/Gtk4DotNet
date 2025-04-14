namespace GtkDotNet.Exceptions;

public class GFileException : GtkException
{
    public GFileError ErrorType { get; }

    internal GFileException(string? file, GErrorStruct error)
        : base(error.Message)
    {
        ErrorType = error.Domain switch
        {
            236 or 232 or 205 or 202 => error.Code switch
            {
                1 when File.Exists(file) => GFileError.TargetNotFound,
                1 => GFileError.SourceNotFound,
                2 => GFileError.TargetExisting,
                12 => GFileError.NoDiskSpace,
                14 => GFileError.AccessDenied,
                19 => GFileError.Canceled,
                _ => GFileError.General,
            },
            _ => GFileError.General,
        };
    }
}