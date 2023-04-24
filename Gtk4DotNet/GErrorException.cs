using System;

namespace GtkDotNet;

public class GErrorException : Exception 
{
    public uint Domain { get; }
    public int Code { get; }

    internal static Exception New(GError error, string source, string destination)
        => error.Domain switch
        {
            232 => FileException.Create(error, source, destination),
            _ => new GErrorException(error)
        };

    internal GErrorException(GError error) : base(error.Message)
    {
        Domain = error.Domain;
        Code = error.Code;
    }
}
