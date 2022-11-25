using UnityEngine;
using System.Collections;

public class AltAndroid : MonoBehaviour {
    private AndroidJavaObject environment;

    void Awake() {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            using (var activity = player.GetStatic<AndroidJavaObject>("currentActivity")){
                Antilatency.AltTracking.AltApi_init(activity.GetRawObject());
            }
        }
#endif
    }
}
