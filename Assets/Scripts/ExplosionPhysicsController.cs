using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPhysicsController : MonoBehaviour {

    [SerializeField] private float ImpulseOnEpicenter = 4000f;
    [SerializeField] private float ImpulseOnBorder = 100f;
    private float radius;

    void OnEnable() {
        radius = GetComponent<SphereCollider>().radius;
    }

    void OnTriggerEnter(Collider other) {
        //var destr = other.GetComponent<IDestroyable>();
        //destr?.TakeDamage(1,other.ClosestPoint(transform.position));

        if (other.attachedRigidbody&&!other.attachedRigidbody.isKinematic){
            if(other.GetComponentInParent<GrenadeController>())
                return;
            if (other.GetComponentInParent<OverheatableShootOnClick>())
                return;
            var dist = Vector3.Distance(other.transform.position, transform.position);
            //Debug.Log(dist);
            other.attachedRigidbody.AddExplosionForce(ImpulseOnBorder + (1 - dist / radius) * (ImpulseOnEpicenter - ImpulseOnBorder), transform.position, radius);
        }
    }
}
