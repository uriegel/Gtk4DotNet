using Gtk4DotNet;

class ColumnViewColAdapter
{
    public ColumnViewColAdapter(Paned paned, ScrolledWindow scrolled, ScrolledWindow scrolledOther, Action onsizePressure, Action onsizePressureRelease)
    {
        paned["position"].OnNotify += () =>
        {
            if (panedPos == scrolled.Width && panedPosOther != scrolledOther.Width)
            {
                if (threshold == 0)
                {
                    threshold = panedPos;
                    onsizePressure();
                }
            }
            if (panedPos > threshold + 20 && threshold > 0)
            {
                threshold = 0;
                onsizePressureRelease();
            }
            if (panedPos != scrolled.Width)
                panedPos = scrolled.Width;
            if (panedPosOther != scrolledOther.Width)
                panedPosOther = scrolledOther.Width;
        };
    }

    int threshold;

    int panedPos;
    int panedPosOther;
}