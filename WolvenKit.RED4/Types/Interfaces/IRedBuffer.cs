namespace WolvenKit.RED4.Types;

public interface IRedBufferWrapper : IRedType
{
    public RedBuffer Buffer { get; set; }
    public IParseableBuffer? Data { get; set; }
}