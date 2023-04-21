using System;
using System.Collections.Generic;

namespace GtkDotNet;

static class Delegates
{
    public static void Add(Delegate delegat) => delegates.Add((-1, delegat));
    public static void Add(long id, Delegate delegat) => delegates.Add((id, delegat));
    public static long Remove(Delegate delegat) 
    {
        (long id, Delegate d) = delegates.Find(n => n.Item2 == delegat);
        if (d != null)
            delegates.Remove((id, d));
        return id;
    } 
    static List<(long, Delegate)> delegates = new();
}