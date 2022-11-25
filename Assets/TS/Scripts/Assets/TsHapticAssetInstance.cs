using System;
using TsSDK;
using UnityEngine;

public class TsHapticAssetInstance : ScriptableObject
{
    public IHapticAsset Asset
    {
        get { return m_asset; }
    }
    private IHapticAsset m_asset = null;

    public static TsHapticAssetInstance Create(Func<IHapticAsset> assetCreator)
    {
        var instance = CreateInstance<TsHapticAssetInstance>();
        instance.m_asset = assetCreator();
        return instance;
    }

    private void OnDestroy()
    {
        if (m_asset != null && TsManager.Root != null)
        {
            //TsManager.Root.AssetManager.Unload(m_asset);
        }
    }
}
