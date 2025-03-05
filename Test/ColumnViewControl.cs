using GtkDotNet;
using GtkDotNet.Controls;
using GtkDotNet.SafeHandles;

static class ColumnViewControlApp
{
    public static int Run()
        => Application
            .NewAdwaita("de.uriegel.first")
                .OnActivate(app =>
                    app
                        .NewWindow()
                            .Title("Hello ColumnView Control👍")
                            .Titlebar(HeaderBar
                                .New()
                                .PackEnd(ToggleButton.New()
                                    .Label("Alternative Model")
                                    .OnToggled(ModelToggled)))
                            .DefaultSize(600, 800)
                            .Child(columnView.CreateView(cv =>
                                cv.SetColumns(GetColumns1(), GetModel1())))
                            .Show())
                .Run(0, IntPtr.Zero);

    static ColumnViewControlColumn<Type1>[] GetColumns1()
        => [ new()
                {
                    Title = "Name", Expanded = true, OnLabelBind = i => i.Name
                },
            new()
                {
                    Title = "Number", OnLabelBind = i => i.Number.ToString()
                },
            ];

    static ColumnViewControlColumn<Type2>[] GetColumns2()
        => [ new()
                {
                    Title = "E Mail", Expanded = true, OnLabelBind = i => i.EMail
                },
            new()
                {
                    Title = "ID", OnLabelBind = i => i.Id
                },
            new()
                {
                    Title = "Active", OnLabelBind = i => i.Active ? "Yes" : "No"
                },
            ];

    static ObservableModel<Type1> GetModel1()
        => new([new Type1("Uwe Riegel", 1965), new Type1("Jim Doe", 222), new Type1("Jane Doe", 9999)]);

    static ObservableModel<Type2> GetModel2()
        => new([.. Enumerable.Range(1, 30).Select(n => new Type2($"item{n}@dom.de", $"ID-{n}", n % 3 == 0))]);

    static void ModelToggled(ToggleButtonHandle toggleButton)
    {
        if (toggleButton.Active())
            columnView.SetColumns(GetColumns2(), GetModel2());
        else
            columnView.SetColumns(GetColumns1(), GetModel1());
    }

    static readonly ColumnViewControl columnView = new();
}

record Type1(string Name, int Number);
record Type2(string EMail, string Id, bool Active);