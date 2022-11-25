using UnityEngine;

public class SetLocalCamera : MonoBehaviour {
    void Awake() {
        GameManager.Instance.LocalCameraTransform = transform;
    }

    
}
