using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class ListBox : Widget
{
    public SelectionMode SelectionMode
    {
        get => GetSelectionMode(this);
        set => SetSelectionMode(this, value);
    }
    public static ListBox New()
    {
        var listbox = _New();
        listbox.CheckDiagnostics();
        return listbox;
    }

    /// <summary>
    /// Inserts a listbox item from a .NET resource template.ui at the first position
    /// </summary>
    /// <param name="template">The resource name of the template.ui</param>
    /// <param name="getWidget">CTor to create a listbox item from the template builder. <see cref="ListBox"/> will automatically wrap its children in a <see cref="ListBoxRow"/> when necessary.</param>
    public void PrependFromTemplate(string template, Func<Builder, Widget> getWidget)
    {
        using var builder = Builder.FromDotNetResource(template);
        Prepend(this, getWidget(builder));
    }

    /// <summary>
    /// Appends a listbox item from a .NET resource template.ui
    /// </summary>
    /// <param name="template">The resource name of the template.ui</param>
    /// <param name="getWidget">CTor to create a listbox item from the template builder. <see cref="ListBox"/> will automatically wrap its children in a <see cref="ListBoxRow"/> when necessary.</param>
    public void AppendFromTemplate(string template, Func<Builder, Widget> getWidget)
    {
        using var builder = Builder.FromDotNetResource(template);
        Append(this, getWidget(builder));
    }

    /// <summary>
    /// Inserts a listbox item from a .NET resource template.ui
    /// </summary>
    /// <param name="template">The resource name of the template.ui</param>
    /// <param name="getWidget">CTor to create a listbox item from the template builder. <see cref="ListBox"/> will automatically wrap its children in a <see cref="ListBoxRow"/> when necessary.</param>
    /// <param name="position">Position at which the newly created listbox item is to be inserted.</param>
    public void InsertFromTemplate(string template, Func<Builder, Widget> getWidget, int position = -1)
    {
        using var builder = Builder.FromDotNetResource(template);
        Insert(this, getWidget(builder), position);
    }

    /// <summary>
    /// Returns the selected listbox item. The returned <see cref="ListBoxRow"/>
    /// </summary>
    /// <returns>A newly created <see cref="ListBoxRow"/> containig the listbox row handle and the attached managed data, if previously set</returns>
    public ListBoxRow GetSelectedRow() => GetSelectedRow(this);

    /// <summary>
    /// Returns the listbox item at the specified position. The returned <see cref="ListBoxRow"/>
    /// </summary>
    /// <param name="index"></param>
    /// <returns>A newly created <see cref="ListBoxRow"/> containing the listbox row handle and the attached managed data, if previously set</returns>
    public ListBoxRow GetRowAtIndex(int index) => GetRowAtIndex(this, index);

    /// <summary>
    /// Select the listbox row at the specified position.
    /// </summary>
    /// <param name="row"></param>
    public void SelectRow(ListBoxRow row) => SelectRow(this, row);

    /// <summary>
    /// Sets a header function.
    /// By setting a header function on the box one can dynamically add headers in front of rows, depending on the contents of the row and its position in the list.
    /// For instance, one could use it to add headers in front of the first item of a new kind, in a list sorted by the kind.
    /// </summary>
    /// <param name="onHeader">Callback that is called for each listbox row and its predecessor, if available. With the help of <see cref="ListBoxRow.SetHeader(Widget)"/> you can set a header if the sonditions are right. </param>
    public void SetHeaderFunc(Action<ListBoxRow?, ListBoxRow?> onHeader)
    {
        ThreePointerDelegate threePointerDelegate = (p1, p2, p3) =>
        {
            var currentRow = p1 != 0 ? new ListBoxRow(p1) : null;
            var previousRow = p2 != 0 ? new ListBoxRow(p2) : null;
            onHeader(currentRow, previousRow);
        };
        var key = GtkDelegates.Instance.GetKey("ListBoxHeaderFunc");
        GtkDelegates.Instance.Add(key, threePointerDelegate);
        AddWeakRef(() => GtkDelegates.Instance.Remove(key.Key));
        SetHeaderFunc(this, Marshal.GetFunctionPointerForDelegate((Delegate)threePointerDelegate), 0, 0);
    }

    /// <summary>
    /// Installs a callback that is being called on listbox row activation
    /// </summary>
    /// <param name="onActivated"></param>
    public DelegateId OnRowActivated(Action onActivated) => SignalConnect<ThreePointerDelegate>("row-activated", (_, nint, __) => onActivated());

    /// <summary>
    /// Removes all items of this ListBox
    /// </summary>
    public void RemoveAll() => RemoveAll(this);

    /// <summary>
    /// </summary>
    /// <param name="widget">The listbox item to insert. <see cref="ListBox"/> will automatically wrap its children in a <see cref="ListBoxRow"/> when necessary.</param>
    /// <param name="position">Position at which the listbox item is to be inserted.</param>
    public void Insert(Widget widget, int position = -1) => Insert(this, widget, position);

    /// <summary>
    /// Inserts a listbox item at the first position
    /// </summary>
    /// <param name="widget">The listbox item to insert. <see cref="ListBox"/> will automatically wrap its children in a <see cref="ListBoxRow"/> when necessary.</param>
    public void Prepend(Widget widget) => Prepend(this, widget);

    /// <summary>
    /// Appends listbox item
    /// </summary>
    /// <param name="widget">The listbox item to append. <see cref="ListBox"/> will automatically wrap its children in a <see cref="ListBoxRow"/> when necessary.</param>
    public void Append(Widget widget) => Append(this, widget);

    /// <summary>
    /// Removes a listbox item from the ListBox
    /// </summary>
    /// <param name="widget"></param>
    public void Remove(Widget widget) => Remove(this, widget);

    public void BindModel<T>(ListModel model, Func<T?, Widget> onCreate)
        where T : class
    {
        CreateItemDelegate callback = (item, _) =>
        {
            var t = GetManagedData<T>(item, ListStore.DATA);
            var widget = onCreate(t);
            return widget.GetInternalHandle();
        };
        BindModel(this, model, Marshal.GetFunctionPointerForDelegate((Delegate)callback), 0, 0);
    }

    public void BindModel<T>(ListModel model, string template, Func<Builder, T?, Widget> onCreate)
        where T : class
    {
        List<Builder> builders = [];
        CreateItemDelegate callback = (item, _) =>
        {
            using var builder = Builder.FromDotNetResource(template);
            var t = GetManagedData<T>(item, ListStore.DATA);
            var widget = onCreate(builder, t);
            widget.Ref();
            return widget.GetInternalHandle();
        };
        BindModel(this, model, Marshal.GetFunctionPointerForDelegate((Delegate)callback), 0, 0);
    }

    public ListBox() : base() { }

    public ListBox(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ListBox _New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_remove_all", CallingConvention = CallingConvention.Cdecl)]
    extern static void RemoveAll(ListBox listbox);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_insert", CallingConvention = CallingConvention.Cdecl)]
    extern static void Insert(ListBox listbox, Widget widget, int position);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_prepend", CallingConvention = CallingConvention.Cdecl)]
    extern static void Prepend(ListBox listbox, Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void Append(ListBox listbox, Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_remove", CallingConvention = CallingConvention.Cdecl)]
    extern static void Remove(ListBox listbox, Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_set_selection_mode", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSelectionMode(ListBox listbox, SelectionMode selectionMode);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_get_selection_mode", CallingConvention = CallingConvention.Cdecl)]
    extern static SelectionMode GetSelectionMode(ListBox listbox);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_set_header_func", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetHeaderFunc(ListBox listbox, nint callback, nint _, nint __);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_get_selected_row", CallingConvention = CallingConvention.Cdecl)]
    extern static ListBoxRow GetSelectedRow(ListBox listbox);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_get_row_at_index", CallingConvention = CallingConvention.Cdecl)]
    extern static ListBoxRow GetRowAtIndex(ListBox listbox, int index);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_select_row", CallingConvention = CallingConvention.Cdecl)]
    extern static void SelectRow(ListBox listbox, ListBoxRow row);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_bind_model", CallingConvention = CallingConvention.Cdecl)]
    extern static void BindModel(ListBox listbox, ListModel model, nint onCallback, nint _, nint onDestroy);
}

delegate nint CreateItemDelegate(nint item, nint _);

