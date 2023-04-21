using System;

namespace GtkDotNet;

[Flags]
public enum GdkModifierType
{
    Shift = 1,
    Lock = 2,
    Ctrl = 4,
    Alt = 8,
    MouseButton1 = 0x100,
    MouseButton2 = 0x200,
    MouseButton3 = 0x400,
    MouseButton4 = 0x800,
    MouseButton5 = 0x1000,
    Super = 0x4000000,
    Hyper = 0x8000000,
    Meta = 0x10000000
}
