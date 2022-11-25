using System.Collections;
using System.Collections.Generic;
using System.IO;
using TsAPI.Types;
using UnityEngine;

[CreateAssetMenu(menuName = "Teslasuit/Haptic/Simplified haptic channel")]
public class TsHapticSimplifiedChannel : ScriptableObject
{
    public TsHumanBoneIndex BoneIndex;
    public TsBone2dSide BoneSide;


    #if UNITY_EDITOR
    public static TsHapticSimplifiedChannel Create(TsHumanBoneIndex boneIndex, TsBone2dSide side, string path)
    {
        var name = $"{boneIndex}{side}";
        var instance = CreateInstance<TsHapticSimplifiedChannel>();
        instance.BoneIndex = boneIndex;
        instance.BoneSide = side;
        instance.name = name;
        UnityEditor.AssetDatabase.CreateAsset(instance, Path.Combine(path, $"{name}.asset"));
        return instance;
    }
    #endif
}
