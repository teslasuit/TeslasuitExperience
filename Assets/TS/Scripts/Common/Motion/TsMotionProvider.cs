using TsSDK;
using UnityEngine;

public abstract class TsMotionProvider : MonoBehaviour
{
    public abstract bool Running { get; }
    public abstract ISkeleton GetSkeleton(float time = 0.0f);
    public abstract void Calibrate();

}
