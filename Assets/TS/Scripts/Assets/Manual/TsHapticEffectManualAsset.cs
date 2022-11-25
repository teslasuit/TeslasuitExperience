using System;
using TsAPI.Types;
using TsSDK;
using UnityEngine;

[CreateAssetMenu(menuName = "Teslasuit/Haptic/Manual Haptic Effect")]
public class TsHapticEffectManualAsset : TsHapticEffectAsset
{
    [Serializable]
    public struct ParamCurve
    {
        //public TsHapticParamType paramType;
        public AnimationCurve curve;
    }

    [SerializeField] 
    public ParamCurve[] curves;

    private void OnValidate()
    {
        foreach (var paramCurve in curves)
        {
            ValidateCurve(paramCurve.curve);
        }
    }

    private void ValidateCurve(AnimationCurve curve)
    {
        var keys = curve.keys;
        if (keys.Length < 2)
        {
            Array.Resize(ref keys, 2);
        }

        for (int i = 0; i < keys.Length; ++i)
        {
            keys[i].value = Mathf.Clamp01(keys[i].value);
            keys[i].time = Mathf.Clamp01(keys[i].time);

            if (i == keys.Length - 1)
            {
                keys[i].time = 1.0f;
            }

            if (i == 0)
            {
                keys[i].time = 0.0f;
            }
        }

        curve.keys = keys;
    }

    //private TsBezier2dParamEffect[] CreateFromCurves(ParamCurve[] paramCurves)
    //{
    //    TsBezier2dParamEffect[] effects = new TsBezier2dParamEffect[paramCurves.Length];
    //    for (int i = 0; i < effects.Length; ++i)
    //    {
    //        effects[i] = CreateFromCurve(paramCurves[i]);
    //    }

    //    return effects;
    //}

    //private TsBezier2dParamEffect CreateFromCurve(ParamCurve paramCurve)
    //{
    //    var curve = paramCurve.curve;
    //    var effect = new TsBezier2dParamEffect();
    //    effect.curves = new TsBezier2dCurve[curve.length - 1];

    //    for (int i = 0; i < curve.keys.Length - 1; ++i)
    //    {
    //        var currentKey = curve.keys[i];
    //        var nextKey = curve.keys[i + 1];

    //        GetPointFromKey(currentKey, out var p0, out var p1, true);
    //        GetPointFromKey(nextKey, out var p3, out var p2, false);

    //        effect.curves[i].p0 = new TsVec2f {x = p0.x, y = p0.y};
    //        effect.curves[i].p1 = new TsVec2f {x = p1.x, y = p1.y};
    //        effect.curves[i].p2 = new TsVec2f {x = p2.x, y = p2.y};
    //        effect.curves[i].p3 = new TsVec2f {x = p3.x, y = p3.y};
    //    }

    //    return effect;
    //}

    private static void GetPointFromKey(Keyframe key, out Vector2 pControl, out Vector2 pTangent, bool outMode)
    {
        pControl.x = key.time;
        pControl.y = key.value;
        var weight = outMode ? key.outWeight : key.inWeight;
        var tangent = outMode ? key.outTangent : key.inTangent;
        
        var isWeightedMode = (key.weightedMode == WeightedMode.In && !outMode) ||
                             (key.weightedMode == WeightedMode.Out && outMode) ||
                             key.weightedMode == WeightedMode.Both;

        if (!isWeightedMode)
        {
            weight = 1.0f / 3;
        }

        int sign = outMode ? 1 : -1;
        pTangent.x = pControl.x + sign * weight;
        pTangent.y = pControl.y + sign * weight * tangent;
    }

    //protected override IHapticAsset Load()
    //{
    //    var effects = CreateFromCurves(curves);
    //    return TsManager.Root.HapticAssetManager.CreateCubicBezierEffect(effects);
    //}
}
