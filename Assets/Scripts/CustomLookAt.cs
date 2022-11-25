using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLookAt : MonoBehaviour
{
    public Transform target;
    public Transform objectToLook;
    public float speed = 2f;
    void Update()
    {
        if (returning) {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Mathf.Clamp01(speed * Time.deltaTime));
            if (Quaternion.Angle(transform.localRotation, Quaternion.identity) < 5f) {
                transform.localRotation = Quaternion.identity;
                returning = false;
                enabled = false;
            }
            return;
        }
        if (target != null)
        {
            Vector3 dir = target.position - objectToLook.position;
            float mag = dir.magnitude;

            if (mag > 0.001f)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                Quaternion rot = lookRot * Quaternion.Inverse(objectToLook.rotation);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot*transform.rotation, Mathf.Clamp01(speed * Time.deltaTime));
            }
        }

    }

    private bool returning;
    public void ReturnToIdentityRotation() {
        returning = true;
    }
}
