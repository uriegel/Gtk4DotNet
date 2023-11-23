namespace GtkDotNet;

[Flags]
public enum BindFlags
{
    Default = 0,
    Get = 1,
    Set = 2, 
    NoSensitivity = 4,
    GetNoChanges = 8,
    InvertBoolean = 16,
}