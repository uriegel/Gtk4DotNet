namespace GtkDotNet;

public enum Interpolation
{
    /// <summary>
    /// Nearest neighbor sampling; this is the fastest and lowest quality mode. Quality is normally unacceptable when scaling down, but may be OK when scaling up.
    /// </summary>
    Nearest,
    /// <summary>
    /// This is an accurate simulation of the PostScript image operator without any interpolation enabled. Each pixel is rendered as a tiny parallelogram of solid color, the edges of which are implemented with antialiasing. It resembles nearest neighbor for enlargement, and bilinear for reduction.
    /// </summary>
    Tiles,
    /// <summary>
    /// Best quality/speed balance; use this mode by default. Bilinear interpolation. For enlargement, it is equivalent to point-sampling the ideal bilinear-interpolated image. For reduction, it is equivalent to laying down small tiles and integrating over the coverage area.
    /// </summary>
    Bilinear
}