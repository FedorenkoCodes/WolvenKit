using WolvenKit.RED4.Archive.Buffer;
using WolvenKit.RED4.IO;
using WolvenKit.RED4.Types;

namespace WolvenKit.RED4.Archive.IO;

public class worldNodeDataReader : Red4Reader, IBufferReader
{
    public worldNodeDataReader(MemoryStream ms) : base(ms)
    {

    }

    public EFileReadErrorCodes ReadBuffer(RedBuffer buffer)
    {
        var data = new worldNodeDataBuffer();

        while (_reader.BaseStream.Position < _reader.BaseStream.Length)
        {
            // some of this stuff could be worldNodeEditorData
            var t = new worldNodeData();

            t.Position = new Vector4
            {
                X = _reader.ReadSingle(),
                Y = _reader.ReadSingle(),
                Z = _reader.ReadSingle(),
                W = _reader.ReadSingle(),
            };

            t.Orientation = new Quaternion
            {
                I = _reader.ReadSingle(),
                J = _reader.ReadSingle(),
                K = _reader.ReadSingle(),
                R = _reader.ReadSingle(),
            };

            t.Scale = new Vector3
            {
                X = _reader.ReadSingle(),
                Y = _reader.ReadSingle(),
                Z = _reader.ReadSingle()
            };

            t.Pivot = new Vector3
            {
                X = _reader.ReadSingle(),
                Y = _reader.ReadSingle(),
                Z = _reader.ReadSingle()
            };

            t.Bounds = new Box
            {
                Min = new Vector4
                {
                    X = _reader.ReadSingle(),
                    Y = _reader.ReadSingle(),
                    Z = _reader.ReadSingle()
                },
                Max = new Vector4
                {
                    X = _reader.ReadSingle(),
                    Y = _reader.ReadSingle(),
                    Z = _reader.ReadSingle()
                }
            };

            t.Id = _reader.ReadUInt64();

            t.QuestPrefabRefHash = _reader.ReadUInt64();
            t.UkHash1 = _reader.ReadUInt64();
            t.CookedPrefabData = new CResourceReference<worldCookedPrefabData>(_reader.ReadUInt64());

            t.MaxStreamingDistance = _reader.ReadSingle();
            t.UkFloat1 = _reader.ReadSingle();

            t.NodeIndex = _reader.ReadUInt16();

            t.Uk10 = _reader.ReadUInt16();
            t.Uk11 = _reader.ReadUInt16();
            t.Uk12 = _reader.ReadUInt16();

            // TODO: [Path-2.0] Check if its right
            t.Uk13 = _reader.ReadUInt64();
            t.Uk14 = _reader.ReadUInt64();

            if (!data.Lookup.ContainsKey(t.NodeIndex))
            {
                data.Lookup[t.NodeIndex] = new();
            }
            data.Lookup[t.NodeIndex].Add(t);
            data.Entries.Add(t);
        }

        if (buffer.Parent is worldStreamingSector wss)
        {
            wss.VariantNodes = new CArray<CArray<RedBaseClass>>();
            for (var i = 0; i < wss.VariantIndices.Count; i++)
            {
                var ra = new CArray<RedBaseClass>();
                for (int j = wss.VariantIndices[i]; j < data.Entries.Count && (((i + 1) < wss.VariantIndices.Count && j < wss.VariantIndices[i + 1]) || ((i + 1) >= wss.VariantIndices.Count && j < data.Entries.Count)); j++)
                {
                    ra.Add(data.Entries[j]);
                }
                if (i == wss.PersisentNodeIndex)
                {
                    wss.PersistentNodes = ra;
                }
                else
                {
                    wss.VariantNodes.Add(ra);
                }
            }
        }

        buffer.Data = data;

        return EFileReadErrorCodes.NoError;
    }
}