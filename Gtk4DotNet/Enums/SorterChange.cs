namespace GtkDotNet;

public enum SorterChange
{
    /// <summary>
    /// The sorter change cannot be described by any of the other enumeration values.
    /// </summary>
    Different,
    /// <summary>
    /// The sort order was inverted. Comparisons that returned GTK_ORDERING_SMALLER now return GTK_ORDERING_LARGER and vice versa. Other comparisons return the same values as before.
    /// </summary>
    Inverted,
    /// <summary>
    /// The sorter is less strict: Comparisons may now return GTK_ORDERING_EQUAL that did not do so before.
    /// </summary>
    LessStrict,
    /// <summary>
    /// The sorter is more strict: Comparisons that did return GTK_ORDERING_EQUAL may not do so anymore.
    /// </summary>
    MoreStrict,
}