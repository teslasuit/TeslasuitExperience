using TsAPI.Types;
using TsSDK;
using UnityEditor;
using UnityEngine;

public abstract class TsHapticAssetBase : ScriptableObject
{
    public byte[] Bytes { get => m_bytes; }
    
    [SerializeField]
    [HideInInspector]
    protected byte[] m_bytes = null;

    public TsAssetType AssetType
    {
        get { return m_assetType; }
    }

    [SerializeField]
    [HideInInspector]
    private TsAssetType m_assetType = TsAssetType.Undefined;

    public IHapticAsset Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = TsHapticAssetInstance.Create(Load);
            }
            return m_instance.Asset;
        }
    }

    private TsHapticAssetInstance m_instance = null;

    public static TsHapticAssetBase Create(byte[] bytes)
    {
        var type = GetAssetType(bytes);
        TsHapticAssetBase result = null;
        switch (type)
        {
            case TsAssetType.PresetAnimation:
                result = CreateInstance<TsHapticAnimationAsset>();
                break;
            case TsAssetType.TouchSequence:
                result = CreateInstance<TsTouchSequenceAsset>();
                break;
            case TsAssetType.HapticEffect:
                result = CreateInstance<TsHapticEffectAsset>();
                break;
        }

        if (result != null)
        {
            result.m_bytes = bytes;
            result.m_assetType = type;
        }

        return result;
    }

    protected virtual IHapticAsset Load()
    {
        var asset = TsManager.Root.AssetManager.Load(m_bytes);
        return asset as IHapticAsset;
    }

    protected static TsAssetType GetAssetType(byte[] bytes)
    {
        var root = new TsRoot();
        var assetRaw = root.AssetManager.Load(bytes);
        var type = assetRaw.AssetType;
        root.Dispose();
        root = null;
        return type;
    }
}
