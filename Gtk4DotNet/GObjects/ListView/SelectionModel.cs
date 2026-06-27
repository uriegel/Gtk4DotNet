using System.Runtime.InteropServices;
using System.Xml;

namespace Gtk4DotNet;

public abstract class SelectionModel : ListModel
{
    public BitSet GetSelection()
    {
        var bitset = GetSelection(this);
        return bitset;
    }

    public bool SetSelection(BitSet selection)
        =>  SetSelection(this, ref selection, ref selection);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_get_selection", CallingConvention = CallingConvention.Cdecl)]
    extern static BitSet GetSelection(SelectionModel model);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_set_selection", CallingConvention = CallingConvention.Cdecl)]
    extern static bool SetSelection(SelectionModel model, ref BitSet selection, ref BitSet mask);
}