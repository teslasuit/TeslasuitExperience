using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FollowCheckedTarget : MonoBehaviour
{
    [SerializeField] private CheckWorldCollisions target;
    public Vector3 offset = new Vector3(0f, 0f, 0f);

    [SerializeField] private CheckWorldCollisions ViveTarget;
    public CheckWorldCollisions Target
    {
        set { target = value; }
    }
    private void Start()
    {
        
        if (XRSettings.loadedDeviceName == "OpenVR")
            target = ViveTarget;
    }
    private void LateUpdate() {
        if (target)
            transform.position = target.LastFreePos + offset;
        else
        {
            Destroy(this);
        }
    }
}
