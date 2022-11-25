using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.XR;

public class ChangeArm : MonoBehaviour {

    private VRIK vrik;
    [SerializeField] private Transform ViveHead;
    [SerializeField] private Transform ViveRightArm;
    [SerializeField] private Transform ViveLeftArm;
    private void Start() {
        vrik = GetComponent<VRIK>();
        if (XRSettings.loadedDeviceName == "OpenVR") {
            vrik.solver.spine.headTarget = ViveHead;
            if(ViveRightArm) vrik.solver.rightArm.target = ViveRightArm;
            if(ViveLeftArm) vrik.solver.leftArm.target = ViveLeftArm;
        }
        
    }
}
