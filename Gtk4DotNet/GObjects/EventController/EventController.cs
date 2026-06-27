using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class EventController : GObject
{
    public void SetPropagationPhase(PropagationPhase phase) => SetPropagationPhase(this, phase);
    
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_event_controller_set_propagation_phase", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetPropagationPhase(EventController controller, PropagationPhase phase);
}
