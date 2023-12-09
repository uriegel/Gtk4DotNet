using GtkDotNet;
using GtkDotNet.SafeHandles;
using LinqTools;

static class Progress
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => 
                app
                    .NewWindow()
                        .Titlebar(
                            HeaderBar.New()
                            .PackEnd(
                                MenuButton.New()
                                .IconName("open-menu-symbolic")
                                .Child(
                                    DrawingArea.New()
                                    .Ref(drawingArea)
                                    .SetDrawFunction((area, cairo, w, h) =>
                                    {
                                        //cairo.                                           
                                    })
                                )
                            )
                        )
                        .Title("Hello Gtk👍")
                        .DefaultSize(800, 600)
                        .Show())
            .Run(0, IntPtr.Zero);

    static readonly ObjectRef<DrawingAreaHandle> drawingArea = new();
}

// Gtk.SignalConnect<DrawFunc>(kairo, "draw", (a, context, data) =>
//         {
//             var w = Widget.GetAllocatedWidth(a);
//             var h = Widget.GetAllocatedHeight(a);
//             CairoContext.SetAntiAlias(context, GtkDotNet.CairoAntialias.Best);
//             CairoContext.SetLineJoin(context, GtkDotNet.LineJoin.Miter);
//             CairoContext.SetLineCap(context, GtkDotNet.LineCap.Round);
//             CairoContext.Translate(context, w / 2.0, h / 2.0);
//             CairoContext.StrokePreserve(context);
//             CairoContext.ArcNegative(context, 0, 0, (w < h ? w : h) / 2.0, -Math.PI / 2.0, -Math.PI / 2.0 + 0.1 * Math.PI);
//             CairoContext.LineTo(context, 0, 0);
//             CairoContext.SetSourceRgb(context, 0.7, 0.7, 0.7);
//             CairoContext.Fill(context);
            
//             CairoContext.MoveTo(context, 0, 0);
//             CairoContext.Arc(context, 0, 0, (w < h ? w : h) / 2.0, -Math.PI / 2.0, -Math.PI / 2.0 + 0.1 * Math.PI);
//             CairoContext.SetSourceRgb(context, 0.3, 0.3, 0.3);
//             CairoContext.Fill(context);
//         });

// <child>
//               <object class="GtkRevealer" id="ProgressRevealer">
//                 <property name="visible">True</property>
//                 <property name="can-focus">False</property>
//                 <property name="icon-name">open-menu-symbolic</property>
//                 <property name="transition-type">slide-left</property>
//                 <child>
//                   <object class="GtkMenuButton">
//                     <property name="visible">True</property>
//                     <property name="can-focus">True</property>
//                     <property name="focus-on-click">False</property>
//                     <property name="receives-default">True</property>
//                     <property name="margin-end">5</property>
//                     <property name="popover">ProgressDisplay</property>
//                     <child>
//                       <object class="GtkDrawingArea" id="ProgressArea">
//                         <property name="visible">True</property>
//                         <property name="can-focus">False</property>
//                       </object>
//                     </child>
//                   </object>
//                 </child>

// <object class="GtkPopoverMenu" id="ProgressDisplay">
//     <property name="can-focus">False</property>
//     <property name="position">bottom</property>
//     <child>
//       <object class="GtkBox">
//         <property name="visible">True</property>
//         <property name="can-focus">False</property>
//         <property name="orientation">vertical</property>
//         <child>
//           <object class="GtkModelButton">
//             <property name="visible">True</property>
//             <property name="can-focus">True</property>
//             <property name="receives-default">True</property>
//             <property name="text" translatable="yes">Test</property>
//           </object>
//           <packing>
//             <property name="expand">False</property>
//             <property name="fill">True</property>
//             <property name="position">0</property>
//           </packing>
//         </child>
//       </object>
//       <packing>
//         <property name="submenu">main</property>
//         <property name="position">1</property>
//       </packing>
//     </child>
//   </object>