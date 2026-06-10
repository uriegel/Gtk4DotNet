using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class SoupMessageHeaders : BaseHandle
{
    public SoupMessageHeaders() : base() { }
    
    [DllImport(Libs.LibWebKit, EntryPoint = "soup_message_headers_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static SoupMessageHeadersNewHandle New(SoupMessageHeaderType type);

    public IEnumerable<MessageHeader> Get()
    {
        List<MessageHeader> headerList = [];
        Foreach(this, (h, v) => headerList.Add(new(h, v)));
        return headerList;
    }

    public void Set(IEnumerable<MessageHeader> headerList)
        => headerList.ForEach(hv => Append(this, hv.Key, hv.Value));

    [DllImport(Libs.LibWebKit, EntryPoint = "soup_message_headers_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void Append(SoupMessageHeaders headers, string key, string value);

    [DllImport(Libs.LibWebKit, EntryPoint = "soup_message_headers_foreach", CallingConvention = CallingConvention.Cdecl)]
    extern static void Foreach(SoupMessageHeaders headers, SoupMessageHeadersDelegate foreachHeader);
}

public class SoupMessageHeadersNewHandle : SoupMessageHeaders
{
    public SoupMessageHeadersNewHandle() : base() {}

    protected override bool ReleaseHandle()
        => true; // .SideEffect(_ => Unref(handle)); TODO Ref Unref. Use Ref when adding header

    [DllImport(Libs.LibWebKit, EntryPoint = "soup_message_headers_unref", CallingConvention = CallingConvention.Cdecl)]
    extern static void Unref(nint headers);
}

public record MessageHeader(string Key, string Value);