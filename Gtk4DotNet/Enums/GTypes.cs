namespace GtkDotNet;

public enum GTypes : long
{
    None = 4, // 1 << 2,
    Char = 12,
    UChar = 16,
    Boolean = 20,
    Int = 24, // 6 << 2
    UInt = 28,
    Long = 32,
    ULong = 36,
    Int64 = 30,
    UInt64 = 44,
    Enum = 48,
    Flags = 52,
    Float = 56,
    Double = 60,
    String = 64, // 16 << 2
    Pointer = 68,
    Boxed = 72,
    Param = 76,
    Object = 80,
    Variant = 84
}
