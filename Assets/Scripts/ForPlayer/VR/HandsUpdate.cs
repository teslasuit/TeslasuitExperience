using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

public class HandsUpdate : MonoBehaviour
{
    [SerializeField] private VRTK.VRTK_InteractGrab grabObj;
    [SerializeField] private Animator _animator;
    [SerializeField] private bool left;   
    [SerializeField] private float time = 0.35f;
    [SerializeField] private bool useGrad = true;
    private Coroutine cor;
    private float smoothTime;
    private float elapsedTime;
    private float end;



    private void Awake() {
        var photonView = GetComponent<PhotonView>();
        if(photonView)
            if (!photonView.isMine){
                Destroy(this);
            }
    }
    IEnumerator Smoothy(float start, float time)
    {
        elapsedTime = 0;
        smoothTime = 0;
        while (elapsedTime < time)
        {
            smoothTime = Mathf.Lerp(start, end, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            _animator.SetFloat("Grab", smoothTime);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
        cor = null;
    }
    IEnumerator corr()
    {
        yield return null; 
        //Debug.Log(grabObj.GetGrabbedObject() + " " + left);
        if (grabObj.GetGrabbedObject() == null && !left)
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
    }

    private bool steamVR;
    void OnEnable()
    {if(useGrad)
        if (!left)
        {
            //VRInputManager.Instance.GripRightDown += Grab;
            //VRInputManager.Instance.GripRightUp += NonGrab;
        }
        else
        {
            //VRInputManager.Instance.GripLeftDown += Grab;
            //VRInputManager.Instance.GripLeftUp += NonGrab;
        }

        steamVR = XRSettings.loadedDeviceName == "OpenVR";
    }
    void OnDisable()
    {
        if (useGrad)
            if (!left)
        {
            //VRInputManager.Instance.GripRightDown -= Grab;
            //VRInputManager.Instance.GripRightUp -= NonGrab;
        }
        else
        {
            //VRInputManager.Instance.GripLeftDown -= Grab;
            //VRInputManager.Instance.GripLeftUp -= NonGrab;
        }
    }

    [SerializeField] private SteamVR_TrackedObject steamObject;
    [SerializeField] private GameObject haptoicCollisionObject;
    void Update() {
        var inputGrab = 0f;
        var inputTrigger = 0f;
        if (steamVR) {
            inputTrigger = SteamVR_Controller.Input((int)steamObject.index).GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger).x;
            inputGrab = SteamVR_Controller.Input((int)steamObject.index).GetPress(EVRButtonId.k_EButton_Grip)?1f:0f;
        } else {
            inputGrab = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, left?OVRInput.Controller.LTouch:OVRInput.Controller.RTouch);
            inputTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, left?OVRInput.Controller.LTouch:OVRInput.Controller.RTouch);
        }
        _animator.SetBool("Interact", inputTrigger > 0.15f || inputGrab > 0.15f);
        _animator.SetFloat("Grab", inputGrab);
        _animator.SetFloat("Trigger", inputTrigger);
        //if(grabObj&&inputGrab > 0.75f && inputTrigger > 0.75f) Debug.Log(grabObj.GetGrabbedObject()==null);

        if (haptoicCollisionObject) {
            if (grabObj) {
                if (inputGrab > 0.75f && inputTrigger > 0.75f)
                    haptoicCollisionObject.SetActive(grabObj.GetGrabbedObject() == null);
                else haptoicCollisionObject.SetActive(false);
            } else {
                haptoicCollisionObject.SetActive(inputGrab>0.75f&& inputTrigger > 0.75f);
            }

            
        }
        

    }

    void Grab()
    {
        end = 1;
        if (cor == null)
            cor = StartCoroutine(Smoothy(0, time));
    }
    void NonGrab()
    {
        end = 0;
        if (cor == null)
            cor = StartCoroutine(Smoothy(1, time));
    }


}