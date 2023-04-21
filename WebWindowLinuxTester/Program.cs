using LinqTools;
using AspNetExtensions;

var sseEventSource = WebView.CreateEventSource<Event>();
StartEvents(sseEventSource.Send);

WebView
    .Create()
    .InitialBounds(600, 800)
    .ResourceIcon("icon")
    .Title("Commander")
    .QueryString("?theme=windows")
    .SaveBounds()
    .DebugUrl("http://localhost:3000")
    //.Url($"file://{Directory.GetCurrentDirectory()}/webroot/index.html")
    .ConfigureHttp(http => http
        .ResourceWebroot("webroot", "/web")
        .UseSse("sse/test", sseEventSource)
        .MapGet("video", context => 
            context
                .SideEffect(c => Console.WriteLine("Range request"))
            .StreamRangeFile("/home/uwe/Videos/Buster Keaton - Sherlock Jr..mp4"))        
        .Build())
#if DEBUG            
    .DebuggingEnabled()
#endif            
    .Build()
    .Run("de.uriegel.Commander");    

void StartEvents(Action<Event> onChanged)   
{
    var counter = 0;
    new Thread(_ =>
        {
            while (true)
            {
                Thread.Sleep(5000);
                onChanged(new($"Ein Event {counter++}"));
           }
        })
        {
            IsBackground = true
        }.Start();   
}

record Event(string Content);
