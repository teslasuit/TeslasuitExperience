using UnityEngine;
using System.Collections;

public class ViveTrackingDisabler : MonoBehaviour {

    [SerializeField] private Transform CameraTransform;
    [SerializeField] private Vector3 vdelta;

    public Transform CamTransform {
        set { CameraTransform = value; }
    }
    
	// Use this for initialization
	void Start () {
	    transform.localPosition = -(CameraTransform.localPosition);

        
    }

	// Update is called once per frame
	void Update () {
        transform.localPosition = -(CameraTransform.localPosition)+ vdelta;
	    
	}
}
