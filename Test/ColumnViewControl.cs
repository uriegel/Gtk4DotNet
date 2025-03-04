using GtkDotNet;
using GtkDotNet.Controls;

static class ColumnViewControlApp
{
    public static int Run()
        => Application
            .NewAdwaita("de.uriegel.first")
                .OnActivate(app =>
                    app
                        .NewWindow()
                            .Title("Hello ColumnView Control👍")
                            .DefaultSize(600, 800)
                            .Child(columnView.CreateView(cv =>
                                cv.SetColumns([
                                    new ColumnViewControlColumn<Type1>()
                                    {
                                        Title = "Name", Expanded = true, OnLabelBind = i => i.Name
                                    }, 
                                    new ColumnViewControlColumn<Type1>()
                                    {
                                        Title = "Number", OnLabelBind = i => i.Number.ToString()
                                    },
                                ], new ([new Type1("Uwe Riegel", 1965), new Type1("Jim Doe", 222), new Type1("Jane Doe", 9999)]))))
                            .Show())
                .Run(0, IntPtr.Zero);

    static readonly ColumnViewControl columnView = new();
}

record Type1(string Name, int Number);