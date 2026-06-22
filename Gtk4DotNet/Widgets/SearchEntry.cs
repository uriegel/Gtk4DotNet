using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

/// <summary>
/// A single-line text entry widget for use as a search entry.
/// The main API for interacting with a SearchEntry as entry is the Editable interface.
/// </summary>
public class SearchEntry : Widget
{
    /// <summary>
    /// Emitted with a delay. The length of the delay can be changed with the GtkSearchEntry:search-delay property.
    /// </summary>
    /// <param name="changed"></param>
    public void OnSearchChanged(Action changed) => SignalConnect<TwoPointerDelegate>("search-changed", (_, __) => changed());

    public Editable AsEditable() => new Editable(GetInternalHandle());

    public SearchEntry() : base() { }

    public SearchEntry(Builder builder, string? name = null) : base(builder, name) { }
}
