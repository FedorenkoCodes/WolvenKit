using WolvenKit.Core.Extensions;
using WolvenKit.RED4.IO;

namespace WolvenKit.RED4.Types;

public partial class worldInstancedOccluderNode : IRedAppendix
{
    [RED("unknown1")]
    [REDProperty(IsIgnored = true)]
    public CArray<Vector4> Unknown1
    {
        get => GetPropertyValue<CArray<Vector4>>();
        set => SetPropertyValue<CArray<Vector4>>(value);
    }

    partial void PostConstruct()
    {
        Unknown1 = new CArray<Vector4>();
    }

    public void Read(Red4Reader reader, uint size)
    {
        var cnt = reader.BaseReader.ReadVLQInt32() * 4; // probably 2 Boxes cnt
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
    }

    public void Write(Red4Writer writer)
    {
        writer.BaseWriter.WriteVLQInt32(Unknown1.Count / 4);
        foreach (var vector4 in Unknown1)
        {
            writer.Write(vector4.X);
            writer.Write(vector4.Y);
            writer.Write(vector4.Z);
            writer.Write(vector4.W);
        }
    }
}