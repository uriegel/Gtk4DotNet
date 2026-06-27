namespace Gtk4DotNet;

public enum PropagationPhase
{
    None,

    /// <summary>
    /// Events are delivered in the capture phase. The capture phase happens before the bubble phase, runs from the toplevel down to the event widget. 
    /// This option should only be used on containers that might possibly handle events before their children do.
    /// </summary>
    Capture,

    /// <summary>
    /// Events are delivered in the bubble phase. The bubble phase happens after the capture phase, 
    /// and before the default handlers are run. This phase runs from the event widget, up to the toplevel.
    /// </summary>
    Bubble,

    /// <summary>
    /// Events are delivered in the default widget event handlers, note that widget implementations must 
    /// chain up on button, motion, touch and grab broken handlers for controllers in this phase to be run.
    /// </summary>
    Target
}
