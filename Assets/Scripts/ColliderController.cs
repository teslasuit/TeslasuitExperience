using TeslasuitAPI;
using UnityEngine;
using UnityEngine.XR;

public class ColliderController : MonoBehaviour {

    [SerializeField] private Transform cameraTransform;

    private CapsuleCollider collider;
    [SerializeField] private Transform cameraVive;

    private void Start() {
        if (XRSettings.loadedDeviceName == "OpenVR") {
            cameraTransform = cameraVive;
        }
        collider = GetComponent<CapsuleCollider>();
    }
    private void Update() {
        transform.position = cameraTransform.position;
        collider.height = cameraTransform.localPosition.y;
        collider.center=new Vector3(0f, -cameraTransform.localPosition.y/2, 0f);
    }

}
