namespace GtkDotNet;

public class FileException : GException 
{
    public new int Code { get => base.Code; }

    public static GException Create(GError error, string source, string destination)
        => error.Code switch
        {
            1 => CreateNotFoundException(error, source, destination),
            2 => new TargetExistingException(error),
            14 => new AccessDeniedException(error),

            _ => new FileException(error)
        };

    static GException CreateNotFoundException(GError error, string source, string destination)
        => File.Exists(source)
        ? new TargetNotFoundException(error)
        : new SourceNotFoundException(error);

    protected FileException(GError error) : base(error) { }
}

public class AccessDeniedException : FileException
{
    internal AccessDeniedException(GError error) : base(error) {}
}        

public class TargetExistingException : FileException
{
    internal TargetExistingException(GError error) : base(error) {}
}        

public class SourceNotFoundException : FileException
{
    internal SourceNotFoundException(GError error) : base(error) {}
}        

public class TargetNotFoundException : FileException
{
    internal TargetNotFoundException(GError error) : base(error) {}
}        