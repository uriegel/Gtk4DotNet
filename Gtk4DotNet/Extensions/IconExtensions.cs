namespace Gtk4DotNet.Extensions;

public static class IconExtensions
{
    public static bool IsSvg(this byte[] payload)
        => payload.Length > 4
            && (payload[0] == 60 && payload[1] == 115 && payload[2] == 118 && payload[3] == 103
            || payload[0] == 60 && payload[1] == 63 && payload[2] == 120 && payload[3] == 109);    
}