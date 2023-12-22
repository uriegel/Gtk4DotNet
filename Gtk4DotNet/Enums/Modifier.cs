namespace GtkDotNet;

[Flags]
public enum KeyModifiers
{
    No,
    Shift,
    Lock,
    Control = 4,
    Alt = 8,
    Button1 = 256,
    Button2 = 512,
    Button3 = 1024,
    Super = 67108864,
    Hyper = 134217728,
    Meta = 268435456
}