using System;

namespace GtkDotNet;

[Flags]
public enum FileQueryInfoFlags
{
    None = 0,
    NoFollowSymlinks = 1,
}
