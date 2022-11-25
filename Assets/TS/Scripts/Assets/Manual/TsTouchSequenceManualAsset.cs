using System;
using TsAPI.Types;
using TsSDK;
using UnityEngine;

[CreateAssetMenu(menuName = "Teslasuit/Haptic/Manual Touch Sequence")]
public class TsTouchSequenceManualAsset : TsTouchSequenceAsset
{
    //[SerializeField]
    //public TsTouchPlacement[] touchPlacements;

    //private void OnValidate()
    //{
    //    for (var i = 0; i < touchPlacements.Length; ++i)
    //    {
    //        touchPlacements[i].paramsSize = (uint)touchPlacements[i].parameters.Length;
    //    }
    //}

    //protected override IHapticAsset Load()
    //{
    //    return TsManager.Root.HapticAssetManager.CreateTouchSequence(touchPlacements);
    //}
}
