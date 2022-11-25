using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OnJointBreakCall : MonoBehaviour {

    [SerializeField] private UnityEvent OnBreak;
    [SerializeField] private UnityEvent OnScriptBreak;

    void OnJointBreak(float breakForce)
    {
        //transform.parent = null;
        Debug.Log("A joint has just been broken!, force: " + breakForce);
        OnBreak.Invoke();
    }

    public void BreakJoint()
    {
        Debug.Log("A joint has just been broken!, in script! ");
        OnScriptBreak.Invoke();
        OnBreak.Invoke();
    }
    [Header("Skoba force options")]
    [SerializeField] private float torque = 800f;
    [SerializeField] private Transform directionTransform;
    private Rigidbody rb;
    public void AddForce()
    {
        Destroy(GetComponent<Joint>());
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {

            rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 0.1f;
            rb.drag = 1f;
        }
        transform.parent = null;
        StartCoroutine(addForce());
    }

    IEnumerator addForce()
    {
        yield return null;
        rb.WakeUp();
        rb.AddForce(directionTransform.up * torque, ForceMode.Impulse);
        yield return null;
        rb.AddTorque(directionTransform.right * 100, ForceMode.Impulse);
        gameObject.AddComponent<MeshCollider>().convex = true;
    }
}
