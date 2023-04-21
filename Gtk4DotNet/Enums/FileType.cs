namespace GtkDotNet;

public enum FileType
{
    Unknown = 0,
    Regular,
    Directory,
    SymbolicLink,
    /// <summary>
    /// socket, fifo, blockdev, chardev 
    /// </summary>
    Special, 
    Shortcut,
    Mountable
} 