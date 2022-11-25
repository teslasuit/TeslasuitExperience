using TsSDK;
using UnityEngine;

[CreateAssetMenu(menuName = "Teslasuit/Haptic/Haptic Material")]
public class TsHapticMaterialAsset : TsHapticAssetBase
{
    [SerializeField]
    private TsHapticEffectAsset m_hapticEffect;

    [SerializeField]
    private TsTouchSequenceAsset m_touchSequence;


    protected override IHapticAsset Load()
    {
        return TsManager.Root.AssetManager.CreateMaterialAsset(m_touchSequence.Instance, m_hapticEffect.Instance);
    }

}
