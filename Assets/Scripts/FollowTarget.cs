using System;
using UnityEngine;
using UnityEngine.XR;


namespace UnityStandardAssets.Utility
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);

        [SerializeField] private Transform ViveTarget;
        public Transform Target {
            set { target = value; }
        }
        private void Start() {
            if (XRSettings.loadedDeviceName == "OpenVR")
                target = ViveTarget;
        }
        private void LateUpdate()
        {
            if(target)
                transform.position = target.position + offset;
            else {
                Destroy(this);
            }
        }
    }
}
