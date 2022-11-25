using UnityEngine;
using UnityEngine.XR;

public class TwoHand : MonoBehaviour {
    public GameObject holdControl;
    public GameObject rHand;
    private Vector3 upDir;
    private void Start() {
        if (XRSettings.loadedDeviceName == "OpenVR") {
            upDir=Vector3.forward;
        }else if(XRSettings.loadedDeviceName == "Oculus")
        {
            upDir = Vector3.up;
        }
    }

    private Vector3 leftHandLerpd, rightHandLerpd;
    private Vector3 startLocalPos;
    private void OnEnable() {
        leftHandLerpd = holdControl.transform.position;
        startLocalPos = transform.localPosition;
        rightHandLerpd = transform.position;
    }

    [SerializeField] private float lerpSpeedForLeftHand = 5f;
    [SerializeField] private float lerpSpeedForRightHand = 5f;
    [SerializeField] private bool useRightHandLerp;
    void Update () {
        leftHandLerpd = Vector3.Lerp(leftHandLerpd, holdControl.transform.position, Time.deltaTime* lerpSpeedForLeftHand);

        rightHandLerpd = Vector3.Lerp(rightHandLerpd, rHand.transform.position+ startLocalPos, Time.deltaTime * lerpSpeedForRightHand);

        if (useRightHandLerp) {
            transform.position = rightHandLerpd;
            transform.rotation = Quaternion.LookRotation(leftHandLerpd- rightHandLerpd, rHand.transform.TransformDirection(upDir));
        } else {
            transform.rotation = Quaternion.LookRotation(leftHandLerpd - rHand.transform.position, rHand.transform.TransformDirection(upDir));
        }
        
    }
}
