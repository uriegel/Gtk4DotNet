namespace GtkDotNet;

[Flags]
public enum BindingFlags
{
    Default = 0,
    Bidirectional = 1,
    SyncCreate = 2, 
    InvertBoolean = 4,
}