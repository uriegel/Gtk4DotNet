using Gtk4DotNet;

// TODO Two problems: One: Initial window size too small
// TODO Two problems: Two: Window size Ok, then reducing, only one side is adapted
class PanedSizeAdapter
{
    public event Action? OnLeftSizePressure;
    public event Action? OnRightSizePressure;
    public event Action? OnLeftSizePressureRelease;
    public event Action? OnRightSizePressureRelease;
    public PanedSizeAdapter(Paned paned, Widget widgetLeft, Widget widgetRight, bool symmetric = false)
    {
        paned["position"].OnNotify += () =>
        {
            if (panedPosLeft != -1 && panedPosLeft == widgetLeft.Width)
            {
                if (thresholdLeft == 0)
                {
                    thresholdLeft = panedPosLeft;
                    OnLeftSizePressure?.Invoke();
                }
            }
            if (panedPosRight != -1 && panedPosRight == widgetRight.Width)
            {
                if (thresholdRight == 0)
                {
                    thresholdRight = panedPosRight;
                    OnRightSizePressure?.Invoke();
                }
            }
            if (panedPosLeft > thresholdLeft + 20 && thresholdLeft > 0)
            {
                thresholdLeft = 0;
                OnLeftSizePressureRelease?.Invoke();
            }
            if (panedPosRight > thresholdRight + 20 && thresholdRight > 0)
            {
                thresholdRight = 0;
                OnRightSizePressureRelease?.Invoke();
            }
            if (panedPosLeft != widgetLeft.Width)
                panedPosLeft = widgetLeft.Width;
            if (panedPosRight != widgetRight.Width)
                panedPosRight = widgetRight.Width;
        };
    }

    int thresholdLeft;
    int thresholdRight;
    int panedPosLeft = -1;
    int panedPosRight = -1;
}