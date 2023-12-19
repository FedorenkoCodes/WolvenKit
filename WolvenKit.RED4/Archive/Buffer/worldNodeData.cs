using WolvenKit.RED4.Types;

namespace WolvenKit.RED4.Archive.Buffer;

// this might just be worldNodeEditorData
public class worldNodeData : RedBaseClass
{
    [RED("id")]
    [REDProperty(IsIgnored = true)]
    public CUInt64 Id
    {
        get => GetPropertyValue<CUInt64>();
        set => SetPropertyValue<CUInt64>(value);
    }

    [RED("nodeIndex")]
    [REDProperty(IsIgnored = true)]
    public CUInt16 NodeIndex
    {
        get => GetPropertyValue<CUInt16>();
        set => SetPropertyValue<CUInt16>(value);
    }

    [RED("position")]
    [REDProperty(IsIgnored = true)]
    public Vector4 Position
    {
        get => GetPropertyValue<Vector4>();
        set => SetPropertyValue<Vector4>(value);
    }

    [RED("orientation")]
    [REDProperty(IsIgnored = true)]
    public Quaternion Orientation
    {
        get => GetPropertyValue<Quaternion>();
        set => SetPropertyValue<Quaternion>(value);
    }

    [RED("scale")]
    [REDProperty(IsIgnored = true)]
    public Vector3 Scale
    {
        get => GetPropertyValue<Vector3>();
        set => SetPropertyValue<Vector3>(value);
    }

    [RED("pivot")]
    [REDProperty(IsIgnored = true)]
    public Vector3 Pivot
    {
        get => GetPropertyValue<Vector3>();
        set => SetPropertyValue<Vector3>(value);
    }

    [RED("bounds")]
    [REDProperty(IsIgnored = true)]
    public Box Bounds
    {
        get => GetPropertyValue<Box>();
        set => SetPropertyValue<Box>(value);
    }

    [RED("questPrefabRefHash")]
    [REDProperty(IsIgnored = true)]
    public NodeRef QuestPrefabRefHash
    {
        get => GetPropertyValue<NodeRef>();
        set => SetPropertyValue<NodeRef>(value);
    }

    [RED("ukHash1")]
    [REDProperty(IsIgnored = true)]
    public NodeRef UkHash1
    {
        get => GetPropertyValue<NodeRef>();
        set => SetPropertyValue<NodeRef>(value);
    }

    [RED("cookedPrefabData")]
    [REDProperty(IsIgnored = true)]
    public CResourceReference<worldCookedPrefabData> CookedPrefabData
    {
        get => GetPropertyValue<CResourceReference<worldCookedPrefabData>>();
        set => SetPropertyValue<CResourceReference<worldCookedPrefabData>>(value);
    }

    [RED("maxStreamingDistance")]
    [REDProperty(IsIgnored = true)]
    public CFloat MaxStreamingDistance
    {
        get => GetPropertyValue<CFloat>();
        set => SetPropertyValue<CFloat>(value);
    }

    [RED("ukFloat1")]
    [REDProperty(IsIgnored = true)]
    public CFloat UkFloat1
    {
        get => GetPropertyValue<CFloat>();
        set => SetPropertyValue<CFloat>(value);
    }

    // likely a bitfield

    [RED("uk10")]
    [REDProperty(IsIgnored = true)]
    public CUInt16 Uk10
    {
        get => GetPropertyValue<CUInt16>();
        set => SetPropertyValue<CUInt16>(value);
    }

    [RED("uk11")]
    [REDProperty(IsIgnored = true)]
    public CUInt16 Uk11
    {
        get => GetPropertyValue<CUInt16>();
        set => SetPropertyValue<CUInt16>(value);
    }

    [RED("uk12")]
    [REDProperty(IsIgnored = true)]
    public CUInt16 Uk12
    {
        get => GetPropertyValue<CUInt16>();
        set => SetPropertyValue<CUInt16>(value);
    }

    [RED("uk13")]
    [REDProperty(IsIgnored = true)]
    public CUInt64 Uk13
    {
        get => GetPropertyValue<CUInt64>();
        set => SetPropertyValue<CUInt64>(value);
    }

    [RED("uk14")]
    [REDProperty(IsIgnored = true)]
    public CUInt64 Uk14
    {
        get => GetPropertyValue<CUInt64>();
        set => SetPropertyValue<CUInt64>(value);
    }
}