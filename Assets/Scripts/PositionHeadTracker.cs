using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PositionHeadTracker : MonoBehaviour {
    [SerializeField] private Transform CameraHead;
    [SerializeField] private Transform skeletonHead;
    [SerializeField] private Transform ViveHead;

    private void Start()
    {
        if (XRSettings.loadedDeviceName == "OpenVR")
            CameraHead = ViveHead;
    }
    void Update () {
        
	    transform.position =(CameraHead.position- (skeletonHead.position - transform.position)) ;
        //Debug.Log(CameraHead.position+"   "+ skeletonHead.position);

	}
}
