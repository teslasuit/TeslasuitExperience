using UnityEngine;

public class DestroyOnEnter : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        Debug.LogWarning("[DestroyOnEnter]:TODO destroy "+other.name);
        var pw = other.GetComponent<PhotonView>();
        if (pw == null) {
            pw = GetComponentInParent<PhotonView>();
        }
        if (pw) {
            if (pw.ownerId==0)
                PhotonNetwork.Destroy(pw);
        }
    }
}
