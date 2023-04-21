using System;

namespace GtkDotNet;

[Flags]
public enum FileCopyFlags
{
    None = 0,
    Overwrite = 1,
    Backup = 2,
    NoFollowSymlinks = 4,
    /// <summary>
    /// Copy all file metadata instead of just default set used for copy
    /// </summary>
    AllMetaData = 8,
    /// <summary>
    /// Don't use copy and delete fallback if native move not supported
    /// </summary>
    NoFallbackForMove = 16,
    /// <summary>
    /// Leaves target file with default perms, instead of setting the source file perms
    /// </summary>
    TargetDefaultPerm = 32
}
