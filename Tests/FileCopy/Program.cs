using CsTools.Extensions;
using Gtk4DotNet;

var copyDir = $"{CsTools.Directory.GetHomeDir()}/Copy";

Application
    .New("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .OnActivate(app => app
        .NewWindow()
        .Title("Copy File👍")
        .DefaultSize(600, 200)
        .Pipe(w => w.Child(
            Button
                .NewWithLabel($@"
To test copying a file, take a large file
and put it in the sub directory '{copyDir}'.
A copy will be created when pressing this button.")
                .MarginStart(20)
                .MarginEnd(20)
                .MarginTop(20)
                .MarginBottom(20)
                .SideEffect(b => b.OnClicked += TestCopy)))
        .Show()
    ).Run();




async void TestCopy()
{
    var filename = "/home/uwe/Videos/Spreewaldkrimi - Tödllliche Heimkehr.mp4";
    using var feile = GFile.New(filename);
    try
    {
        await feile.CopyAsync("/home/uwe/Test3/Videos/Spreewaldkrimi - Tödllliche Heimkehr.mp4", FileCopyFlags.Overwrite, true,
                (c, t) => Console.WriteLine($"Copy progress: {c}/{t}"));
        Console.WriteLine($"File copied to {filename}.copy");
    }
    catch (OperationCanceledException)
    {
        Console.Error.WriteLine($"Copying file canceled");
    }
    catch (Exception e)
    {
        Console.Error.WriteLine($"Error while copying: {e.Message}");
    }
}

// async void TestCopy()
// {
//     var filename = Directory.EnumerateFiles(copyDir).FirstOrDefault();
//     if (filename != null)
//     {
//         var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(30));
//         using var feile = GFile.New(filename);
//         try
//         {
//             await feile.CopyAsync($"{filename}.copy", FileCopyFlags.Overwrite, true,
//                 (c, t) => Console.WriteLine($"Copy progress: {c}/{t}"), cancellationToken.Token);
//             Console.WriteLine($"File copied to {filename}.copy");
//         }
//         catch (OperationCanceledException)
//         {
//             Console.Error.WriteLine($"Copying file canceled");
//         }
//         catch (Exception e)
//         {
//             Console.Error.WriteLine($"Error while copying: {e.Message}");
//         }
//     }
//     else
//         Console.Error.WriteLine($"You have to put a file in {copyDir}");
// }