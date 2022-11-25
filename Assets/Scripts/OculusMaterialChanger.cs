using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class OculusMaterialChanger : MonoBehaviour {

    [SerializeField] private Material oculusMat;
    [SerializeField] private Material oculusTouchMat;
	void Awake () {
	    if(UnityEngine.XR.XRSettings.loadedDeviceName== "Oculus")
            GetComponent<MeshRenderer>().material =  oculusTouchMat;
	}
	
	
}
