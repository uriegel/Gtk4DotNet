using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

// Release ready

public class SoupMessageHeaders : BaseHandle
{
    public static SoupMessageHeaders New(SoupMessageHeaderType type) => _New(type);
    public SoupMessageHeaders() : base() { }

    public IEnumerable<MessageHeader> Get()
    {
        List<MessageHeader> headerList = [];
        Foreach(this, (h, v) => headerList.Add(new(h, v)));
        return headerList;
    }

    public void Set(IEnumerable<MessageHeader> headerList)
        => headerList.ForEach(hv => Append(this, hv.Key, hv.Value));

    protected override bool ReleaseHandle()
    {
        Unref(handle);
        return true;
    }

    [DllImport(Libs.LibWebKit, EntryPoint = "soup_message_headers_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void Append(SoupMessageHeaders headers, string key, string value);

    [DllImport(Libs.LibWebKit, EntryPoint = "soup_message_headers_foreach", CallingConvention = CallingConvention.Cdecl)]
    extern static void Foreach(SoupMessageHeaders headers, SoupMessageHeadersDelegate foreachHeader);

    [DllImport(Libs.LibWebKit, EntryPoint = "soup_message_headers_new", CallingConvention = CallingConvention.Cdecl)]
    extern static SoupMessageHeaders _New(SoupMessageHeaderType type);
    [DllImport(Libs.LibWebKit, EntryPoint = "soup_message_headers_unref", CallingConvention = CallingConvention.Cdecl)]
    extern static void Unref(nint headers);
}

public record MessageHeader(string Key, string Value);