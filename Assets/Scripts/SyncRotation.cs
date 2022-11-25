using System.Collections;
using TeslasuitAPI;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

public class SyncRotation : MonoBehaviour {
    [SerializeField] private Transform sceletonHead;
    [SerializeField] private Transform _camera;

    [SerializeField]
    private bool useTPose = true;
    //[SerializeField] private SuitMocapSkeleton skeleton;

    private void OnEnable() {
        StartCoroutine(syncCoroutine());
    }

    private IEnumerator syncCoroutine() {
        yield return new WaitForSeconds(1f);
        sync();

    }

    void sync() {
        //var rotHead = sceletonHead.eulerAngles;
        //rotHead.z = 0;
        //rotHead.x = 0;
        //
        //    var rot=_camera.localEulerAngles;
        //rot.z = 0;
        //rot.x = 0;
        //transform.eulerAngles=(rot+rotHead)/2f ;
        var rotHead = sceletonHead.rotation;
        rotHead.z = 0;
        rotHead.x = 0;
        var rot = _camera.localRotation;
        rot.z = 0;
        rot.x = 0;
        transform.localRotation = rotHead * Quaternion.Inverse(rot);
    }
    [SerializeField] private SteamVR_TrackedObject steamLObject;
    [SerializeField] private SteamVR_TrackedObject steamRObject;

    private void Update() {
        var input = Input.GetKeyDown(KeyCode.Space);
        if (XRSettings.loadedDeviceName == "OpenVR") {
            input = input 
                    || SteamVR_Controller.Input((int) steamLObject.index).GetPressDown(EVRButtonId.k_EButton_Grip) 
                    || SteamVR_Controller.Input((int)steamRObject.index).GetPressDown(EVRButtonId.k_EButton_Grip) 
                    || SteamVR_Controller.Input((int)steamLObject.index).GetPressDown(EVRButtonId.k_EButton_ApplicationMenu) 
                    || SteamVR_Controller.Input((int) steamRObject.index).GetPressDown(EVRButtonId.k_EButton_ApplicationMenu);
        } else {
            input = input || OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch);
        }
        if (input ) {
            
            //if (skeleton != null)
            {
            //    if(useTPose)
            //        skeleton.PoseCapture();
                //skeleton.ForceUpdateMocap();
            }
                
            sync();

        }
    }
    /*
    [Button]
    private void showRots() {
        Debug.Log(_camera.localEulerAngles +"     "+sceletonHead.eulerAngles+"    "+transform.localEulerAngles +"     "+transform.eulerAngles);
    }*/
}
