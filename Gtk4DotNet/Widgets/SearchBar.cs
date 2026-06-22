using System.Runtime.InteropServices;

namespace Gtk4DotNet;

/// <summary>
/// GtkSearchBar is a container made to have a search entry (possibly with additional connex widgets, such as drop-down menus, or buttons) built-in. 
/// The search bar would appear when a search is started through typing on the keyboard, or the application’s search mode is toggled on.
/// For keyboard presses to start a search, events will need to be forwarded from the top-level window that contains the search bar. 
/// See gtk_search_bar_handle_event() for example code. Common shortcuts such as Ctrl+F should be handled as an application action, or through the menu items.
/// You will also need to tell the search bar about which entry you are using as your search entry using gtk_search_bar_connect_entry(). 
/// The following example shows you how to create a more complex search entry.
/// </summary>
public class SearchBar : Widget
{
    public bool SearchMode
    {
        get => GetSearchMode(this); 
        set => SetSearchMode(this, value); 
    }
    
    public SearchBar() : base() { }

    public SearchBar(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_search_bar_get_search_mode", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetSearchMode(SearchBar widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_search_bar_set_search_mode", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSearchMode(SearchBar widget, bool value);
}