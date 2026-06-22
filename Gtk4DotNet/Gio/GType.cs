// namespace Gtk4DotNet;

// /// <summary>
// /// The type of an object
// /// </summary>
// public class GType : GObject
// {
//     /// <summary>
//     /// Gets a specified GType 
//     /// </summary>
//     /// <param name="type">The wanted GType</param>
//     /// <returns>The GType of the object</returns>
//     public static nint Get(GTypeEnum type)
//     {
//         var res = type switch
//         {
//             GTypeEnum.GObject => Type(),
//             GTypeEnum.WebKitWebView => WebView.Type(),
//             _ => Type(),
//         };
//         return res;
//     }
// }
