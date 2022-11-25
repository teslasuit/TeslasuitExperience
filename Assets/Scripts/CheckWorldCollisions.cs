using UnityEngine;
using VRTK;

public class CheckWorldCollisions : MonoBehaviour {
    [SerializeField] private float radius=0.05f;
    //[SerializeField] private LayerMask mask;
    public Vector3 LastFreePos => lastPos;
    [SerializeField] private Transform rootTransform;
    private Vector3 lastPos;
    private int hapticLayer;
    private void Start() {
        lastPos = transform.position;
        hapticLayer=LayerMask.NameToLayer("HapticRaycast");
    }

    void Update()
    {
        
        foreach (Collider collider in Physics.OverlapSphere(transform.position, radius)) {
            if(collider.isTrigger)
                continue;
            if (collider.gameObject.layer == hapticLayer)
                continue;
            if(collider.transform.IsChildOf(rootTransform))
                continue;
            if(collider.GetComponent<VRTK_InteractableObject>())
                continue;
            return;
            
        }

        lastPos = transform.position;
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
