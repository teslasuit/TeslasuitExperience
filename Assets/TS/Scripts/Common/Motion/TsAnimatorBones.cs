using System.Collections.Generic;
using TsAPI.Types;
using TsSDK;
using UnityEngine;

public static class TsAnimatorBones
{
    public static IEnumerable<TsHumanBoneIndex> RequiredBones
    {
        get { return TsHumanBones.SuitBones; }
    }

    public static Vector3 GetSidePlanePoint(TsHumanBoneIndex index)
    {
        switch (index)
        {
            case TsHumanBoneIndex.RightUpperArm:
            case TsHumanBoneIndex.RightLowerArm:
            case TsHumanBoneIndex.RightHand:
            {
                return Vector3.back;
            }
            case TsHumanBoneIndex.LeftUpperArm:
            case TsHumanBoneIndex.LeftLowerArm:
            case TsHumanBoneIndex.LeftHand:
            {
                return Vector3.forward;
            }
            case TsHumanBoneIndex.Spine:
            case TsHumanBoneIndex.Chest:
            {
                return Vector3.left;
            }
            default:
            {
                return Vector3.right;
            }
        }
    }

    
}
