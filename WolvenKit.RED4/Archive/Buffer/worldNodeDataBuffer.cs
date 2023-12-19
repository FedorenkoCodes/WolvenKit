using WolvenKit.RED4.Types;

namespace WolvenKit.RED4.Archive.Buffer;

public class worldNodeDataBuffer : IParseableBuffer, IRedType
{
    public IRedType? Data => null;

    public CArray<worldNodeData> Entries { get; } = new();

    public Dictionary<int, List<worldNodeData>> Lookup = new();

    public worldNodeDataBuffer()
    {

    }
}

public class RedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IRedType where TKey : notnull
{

}