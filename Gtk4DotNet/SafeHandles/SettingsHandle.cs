namespace GtkDotNet.SafeHandles;

public class SettingsHandle : ObjectHandle
{
    public SettingsHandle() : base() { }
    public string SchemaId { get; internal set; } = "";
}
