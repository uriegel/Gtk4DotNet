using GtkDotNet;

static class ImageView
{
    public static int Run()
        => Application
            .New("de.uriegel.first")
                .OnActivate(app =>
                    app
                        .NewWindow()
                            .Title("Hello Image👍")
                            .DefaultSize(600, 500)
                            .Child(Grid.New()
                                .Attach(
                                    Image.NewFromFile("../image.jpg"), 0, 0, 1, 1))
                            .Show())
                .Run(0, IntPtr.Zero);
}
