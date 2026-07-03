using Gtk4DotNet;

// TODO Two problems: One: Initial window size too small
// TODO Two problems: Two: Window size Ok, then reducing, only one side is adapted
class PanedSizeAdapter
{
    public event Action? OnSizePressure;
    public event Action? OnSizePressureRelease;
    public PanedSizeAdapter(Paned paned, Widget widget, Widget other)
    {
        paned["position"].OnNotify += () =>
        {
            if (panedPos == widget.Width && panedPosOther != other.Width)
            {
                if (threshold == 0)
                {
                    threshold = panedPos;
                    OnSizePressure?.Invoke();
                }
            }
            if (panedPos > threshold + 20 && threshold > 0)
            {
                threshold = 0;
                OnSizePressureRelease?.Invoke();
            }
            if (panedPos != widget.Width)
                panedPos = widget.Width;
            if (panedPosOther != other.Width)
                panedPosOther = other.Width;
        };
    }

    int threshold;
    int panedPos;
    int panedPosOther;
}