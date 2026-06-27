using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class EventController : GObject
{
    Machine enum
    public void SetPropagationPhase(int p) => SetPropagationPhase(this, p);
    
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_event_controller_set_propagation_phase", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetPropagationPhase(EventController controller, int p);
}
