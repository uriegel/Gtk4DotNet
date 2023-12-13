namespace GtkDotNet;

public class FileError : GError 
{
    public enum ErrorType
    {
        General,
        AccessDenied,
        TargetExisting,
        SourceNotFound,
        TargetNotFound
    }

    public ErrorType Error { get; }

    public new int Code { get => base.Code; }

    internal static FileError Create(GErrorStruct error, string source) => new(error, source);

    FileError(GErrorStruct error, string source) : base(error) 
        => Error = 
            error.Code switch
            {
                1 when File.Exists(source) => ErrorType.TargetNotFound,
                1                          => ErrorType.SourceNotFound,
                2                          => ErrorType.TargetExisting,
                14                         => ErrorType.AccessDenied,
                _                          => ErrorType.General,
            };
}




