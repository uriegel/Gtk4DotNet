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
                                ToggleButton.New()
                                .Ref(progressStarter)
                                .IconName("open-menu-symbolic")
                            )
                            .PackEnd(
                                Revealer.New()
                                .SideEffect(r => progressStarter.Ref.BindProperty("active", r, "reveal-child", BindingFlags.Default))
                                .OnNotify("reveal-child", MakeProgress)                                                                
                                .TransitionType(RevealerTransition.SlideLeft)
                                .Child(
                                    MenuButton.New()
                                    .Child(
                                        DrawingArea.New()
                                        .Ref(drawingArea)
                                        .SetDrawFunction((area, cairo, w, h) => cairo
                                            .AntiAlias(CairoAntialias.Best)
                                            .LineJoin(LineJoin.Miter)
                                            .LineCap(LineCap.Round)
                                            .Translate(w / 2.0, h / 2.0)
                                            .StrokePreserve()
                                            .ArcNegative(0, 0, (w < h ? w : h) / 2.0, -Math.PI / 2.0, -Math.PI / 2.0 + progressRadius * Math.PI * 2)
                                            .LineTo(0, 0)
                                            .SourceRgb(0.7, 0.7, 0.7)
                                            .Fill()
                                            .MoveTo(0, 0)
                                            .Arc(0, 0, (w < h ? w : h) / 2.0, -Math.PI / 2.0, -Math.PI / 2.0 + progressRadius * Math.PI * 2)
                                            .SourceRgb(0.3, 0.3, 0.3)
                                            .Fill()
                                        )
                                    )
                                )
                            )
                        )
                        .Title("Hello Gtk👍")
                        .DefaultSize(800, 600)
                        .Show())
            .Run(0, IntPtr.Zero);

    static async void MakeProgress(RevealerHandle revealer)
    {
        if (!revealer.IsChildRevealed())
            for (int i = 0; i < 1000; i++)
            {
                progressRadius = i / 1000f;
                await Task.Delay(10);
                drawingArea.Ref.QueueDraw();
            }
    }

    static float progressRadius = 0.0f;

    static readonly ObjectRef<ToggleButtonHandle> progressStarter = new();
    static readonly ObjectRef<DrawingAreaHandle> drawingArea = new();
}

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

// gtk_widget_queue_draw