using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class ProgressBar : Widget
{
    public static ProgressBar New()
    {
        var res = _New();
        res.CheckDiagnostics();
        return res;
    }

    public bool ShowText
    {
        set => SetShowText(this, value);
        get => GetShowText(this);
    }

    public double Fraction
    {
        set => SetFraction(this, value);
        get => GetFraction(this);
    }

    public double PulseStep
    {
        set => SetPulseStep(this, value);
        get => GetPulseStep(this);
    }

    public void Pulse() => Pulse(this);

    public ProgressBar() : base() { }

    public ProgressBar(Builder builder, string? name = null) : base(builder, name) { }

    public ProgressBar(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_progress_bar_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ProgressBar _New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_progress_bar_set_show_text", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetShowText(ProgressBar progressBar, bool show);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_progress_bar_get_show_text", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetShowText(ProgressBar progressBar);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_progress_bar_set_fraction", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetFraction(ProgressBar progressBar, double fraction);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_progress_bar_get_fraction", CallingConvention = CallingConvention.Cdecl)]
    extern static double GetFraction(ProgressBar progressBar);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_progress_bar_set_pulse_step", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetPulseStep(ProgressBar progressBar, double fraction);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_progress_bar_get_pulse_step", CallingConvention = CallingConvention.Cdecl)]
    extern static double GetPulseStep(ProgressBar progressBar);
    
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_progress_bar_pulse", CallingConvention = CallingConvention.Cdecl)]
    extern static void Pulse(ProgressBar progressBar);
}


