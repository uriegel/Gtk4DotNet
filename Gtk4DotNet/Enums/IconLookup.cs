using System;

namespace GtkDotNet;

[Flags]
public enum IconLookup
{
    None = 0,
    ForceRegular = 1,
    ForceSymbolic = 2,
    Preload = 4,
}
