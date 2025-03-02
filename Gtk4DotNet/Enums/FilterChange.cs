namespace GtkDotNet;

public enum FilterChange
{
    /// <summary>
    /// The filter change cannot be described with any of the other enumeration values.
    /// </summary>
    Different,
    /// <summary>
    /// The filter is less strict than it was before: All items that it used to return true still return true, others now may, too
    /// </summary>
    LessStrict,
    /// <summary>
    /// The filter is more strict than it was before: All items that it used to return false still return false, others now may, too.
    /// </summary>
    MoreStrict
}
