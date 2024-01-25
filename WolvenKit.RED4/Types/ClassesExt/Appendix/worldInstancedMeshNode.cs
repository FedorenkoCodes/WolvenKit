using WolvenKit.Core.Extensions;
using WolvenKit.RED4.IO;

namespace WolvenKit.RED4.Types;

public partial class worldInstancedMeshNode : IRedAppendix
{
    [RED("unknown1")]
    [REDProperty(IsIgnored = true)]
    public CArray<Vector4> Unknown1
    {
        get => GetPropertyValue<CArray<Vector4>>();
        set => SetPropertyValue<CArray<Vector4>>(value);
    }

    [RED("unknown2")]
    [REDProperty(IsIgnored = true)]
    public CArray<CFloat> Unknown2
    {
        get => GetPropertyValue<CArray<CFloat>>();
        set => SetPropertyValue<CArray<CFloat>>(value);
    }

    [RED("unknown3")]
    [REDProperty(IsIgnored = true)]
    public CArray<CUInt32> Unknown3
    {
        get => GetPropertyValue<CArray<CUInt32>>();
        set => SetPropertyValue<CArray<CUInt32>>(value);
    }

    partial void PostConstruct()
    {
        Unknown1 = new CArray<Vector4>();
        Unknown2 = new CArray<CFloat>();
        Unknown3 = new CArray<CUInt32>();
    }

    public void Read(Red4Reader reader, uint size)
    {
        var cnt = reader.BaseReader.ReadVLQInt32() * 2; // probably Box cnt
        for (var i = 0; i < cnt; i++)
        {
            Unknown1.Add(new Vector4
            {
                X = reader.ReadCFloat(), 
                Y = reader.ReadCFloat(), 
                Z = reader.ReadCFloat(), 
                W = reader.ReadCFloat()
            });
        }

        cnt = reader.BaseReader.ReadByte(); // could be also bool, vlq or something...
        for (var i = 0; i < cnt; i++)
        {
            Unknown2.Add(reader.ReadCFloat());
        }

        cnt = reader.BaseReader.ReadByte(); // could be also bool, vlq or something...
        for (var i = 0; i < cnt; i++)
        {
            Unknown3.Add(reader.ReadCUInt32());
        }
    }

    public void Write(Red4Writer writer)
    {
        writer.BaseWriter.WriteVLQInt32(Unknown1.Count / 2);
        foreach (var vector4 in Unknown1)
        {
            writer.Write(vector4.X);
            writer.Write(vector4.Y);
            writer.Write(vector4.Z);
            writer.Write(vector4.W);
        }

        writer.BaseWriter.Write((byte)Unknown2.Count);
        foreach (var val in Unknown2)
        {
            writer.Write(val);
        }

        writer.BaseWriter.Write((byte)Unknown3.Count);
        foreach (var val in Unknown3)
        {
            writer.Write(val);
        }
    }
}