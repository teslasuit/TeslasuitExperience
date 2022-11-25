using UnityEngine;
using System.Collections;
using UnityEngine.XR;

public class RotateInOpposite : MonoBehaviour {

    [SerializeField]private Transform target;
    public Transform Target { set { target = value; } }
    [SerializeField] private bool x=true;
    [SerializeField] private bool y = true;
    [SerializeField] private bool z = true;
    [SerializeField] private bool negative = false;
    [SerializeField] private Transform ViveTarget;

    private void Awake()
    {
        if (XRSettings.loadedDeviceName == "OpenVR")
            target = ViveTarget;
    }
    void Update()
    {
        if (target != null)
        {
            var v = target.rotation;
            if(!x)v.x = 0;
            if(!y)v.y = 0;else if (negative) v.y *= -1;
            if (!z) v.z = 0;
            transform.rotation=v;


            
        }
    }
}
