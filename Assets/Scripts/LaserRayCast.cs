using UnityEngine;

public class LaserRayCast : MonoBehaviour {
    [SerializeField] private LayerMask mask=0;
    [SerializeField] private float maxLen =15f;
    private RaycastHit hit;
    [SerializeField] private LineRenderer _renderer;
    [SerializeField] private Transform spheregameObject=null;

    private void Start() {
        if (_renderer == null) _renderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxLen, mask)) {
            _renderer.SetPosition(1, new Vector3(0f, 0f, hit.distance));
            if(spheregameObject){
                spheregameObject.position = hit.point;
                spheregameObject.gameObject.SetActive(true);
            }
        } else {
            _renderer.SetPosition(1,new Vector3(0f,0f,maxLen));
            if (spheregameObject) spheregameObject.gameObject.SetActive(false);
        }
    }
}
