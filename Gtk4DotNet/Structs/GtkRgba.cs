using System.Drawing;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct GtkRgba
{
    public float Red;
    public float Green;
    public float Blue;
    public float Alpha;

    public GtkRgba ToSrgb()
    {
        return new GtkRgba
        {
            Red = ToSrgb(Red),
            Green = ToSrgb(Green),
            Blue = ToSrgb(Blue),
            Alpha = Alpha,
        };

        float ToSrgb(float c)
            => c <= 0.0031308f
                ? 12.92f * c
                : 1.055f * (float)Math.Pow(c, 1.0 / 2.4) - 0.055f;
    }

    public static Color ToColor(GtkRgba gtkRgba)
        => Color.FromArgb((int)(gtkRgba.Alpha * 255), (int)(gtkRgba.Red * 255), (int)(gtkRgba.Green * 255), (int)(gtkRgba.Blue * 255));
    public static GtkRgba FromColor(Color color)
        => new GtkRgba
        {
            Alpha = color.A / 255.0f,
            Red = color.R / 255.0f,
            Green = color.G / 255.0f,
            Blue = color.B / 255.0f,
        };
}
