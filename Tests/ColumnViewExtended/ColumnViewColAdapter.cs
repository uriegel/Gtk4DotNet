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
            Console.WriteLine($"Links: {widgetLeft.Width} - {Math.Abs(widgetLeft.Width - widgetRight.Width)}");
            if (panedPosLeft != -1 && panedPosLeft == widgetLeft.Width)
            {
                if (thresholdLeft == -1)
                {
                    thresholdLeft = panedPosLeft;
                    OnLeftSizePressure?.Invoke();
                    if (Math.Abs(widgetLeft.Width - widgetRight.Width) < 20)
                        OnRightSizePressure?.Invoke();
                }
            }
            if (panedPosRight != -1 && panedPosRight == widgetRight.Width)
            {
                if (thresholdRight == -1)
                {
                    thresholdRight = panedPosRight;
                    OnRightSizePressure?.Invoke();
                    if (Math.Abs(widgetLeft.Width - widgetRight.Width) < 20)
                        OnLeftSizePressure?.Invoke();
                }
            }
            if (panedPosLeft > thresholdLeft + 20 && thresholdLeft > 0)
            {
                thresholdLeft = -1;
                OnLeftSizePressureRelease?.Invoke();
                if (Math.Abs(widgetLeft.Width - widgetRight.Width) < 20)
                    OnRightSizePressureRelease?.Invoke();
            }
            if (panedPosRight > thresholdRight + 20 && thresholdRight > 0)
            {
                thresholdRight = -1;
                OnRightSizePressureRelease?.Invoke();
                if (Math.Abs(widgetLeft.Width - widgetRight.Width) < 20)
                    OnLeftSizePressureRelease?.Invoke();
            }
            if (panedPosLeft != widgetLeft.Width)
                panedPosLeft = widgetLeft.Width;
            if (panedPosRight != widgetRight.Width)
                panedPosRight = widgetRight.Width;
        };
    }

    int thresholdLeft = -1;
    int thresholdRight = -1;
    int panedPosLeft = -1;
    int panedPosRight = -1;
}