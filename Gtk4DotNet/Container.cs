using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Container
{
    /// <summary>
    /// The caller of the method takes ownership of the data container, but not the data inside it.
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_container_get_children ", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr GetChildren(this ContainerHandle container);
}

// GtkBin: get_child
// Gtk_Button -> GtkBin -> GtkContainer
// Window -> GtkBin -> GtkContainer

// struct GList {
//   gpointer data;
//   GList* next;
//   GList* prev;
// }

// void
// g_list_free (
//   GList* list
// )

// GtkWidget *_gtk_menu_get_item(GtkMenu *m, int pos){
//   int i=0;
//   GList *l=gtk_container_get_children (GTK_CONTAINER(m));    
  
//   while(l){
//     if(i == pos){
//       GtkWidget *ret = (GtkWidget *)l->data;
//       g_list_free (l);
//       return ret;
//     }

//     i++;
//     l=l->next;
//   }

//   return NULL;
// }
