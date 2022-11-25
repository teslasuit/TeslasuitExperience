using UnityEngine;
using UnityEngine.XR;

public class OneHandHolder : MonoBehaviour {

    [SerializeField] Transform _holderTarget;
    [SerializeField] private Transform ViveTarget;
    [SerializeField] private Vector3 RotOffset;
    protected void Awake()
    {
        var photonView = GetComponent<PhotonView>();
        if (photonView&&!photonView.isMine){
            Destroy(this);
        }
        if (XRSettings.loadedDeviceName == "OpenVR")
            _holderTarget = ViveTarget;
    }
    void Update () {
       if(_holderTarget) transform.rotation = _holderTarget.rotation*Quaternion.Euler(RotOffset);
       else Debug.LogWarning("name:"+gameObject.name+" target is null"); 
    }
}
