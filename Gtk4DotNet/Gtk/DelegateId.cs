public struct DelegateId
{
    internal DelegateId(long key, string name, long signalId = 0)
    {
        Key = key;
        Name = name;
        SignalId = signalId;
    }
    
    internal long Key { get; }
    internal string Name { get; }
    internal long SignalId { get; set; }
}
