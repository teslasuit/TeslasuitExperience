using UnityEngine;
using System.Collections;

public class SmoothFollowVR : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool LerpRandomValue;
    public Transform Target { set { target = value; } }
    [SerializeField] private float lerpspeed;
    private float randomValue = 1;
    // [SerializeField] private Transform adTarget;

    private void Start()
    {
        if (LerpRandomValue)
        {
            randomValue = Random.RandomRange(0.1f, 1);
            lerpspeed *= randomValue;
        }
    }
    void Update()
    {
        // if (adTarget == null) 
        transform.position = Vector3.Lerp(transform.position    , target.position, Time.deltaTime * lerpspeed);
        // else {
        //     transform.position = Vector3.Lerp(transform.position, (target.position+adTarget.position)/2.0f, Time.deltaTime * lerpspeed);
        // }
        // TestRotation();
    }
}
