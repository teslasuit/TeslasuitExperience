using System.Collections; 
using UnityEngine;
using VRTK;

public class TeleportOnWall : MonoBehaviour {

    private VRTK_HeadsetCollision collision;
    private VRTK_BasicTeleport teleport;
    private void OnEnable() {
        collision = GetComponent<VRTK_HeadsetCollision>();
        teleport = GetComponent<VRTK_BasicTeleport>();
        collision.HeadsetCollisionDetect += OnCollision;
        collision.HeadsetCollisionEnded += OnEndCollision;
        teleport.Teleported += Teleport_Teleported;
    }

    private void Teleport_Teleported(object sender, DestinationMarkerEventArgs e)
    {
        foreach (var raycastHit in Physics.RaycastAll(e.destinationPosition + Vector3.up * 0.5f, Vector3.down, 2f)) {
            if (raycastHit.transform.GetComponent<Floor>()) {
                return;
            }
        }
        Debug.Log("Teleport without floor event");
        Teleport();
    }

    private void OnDisable() {
        collision.HeadsetCollisionDetect -= OnCollision;
        collision.HeadsetCollisionEnded -= OnEndCollision;
    }

    private IEnumerator waitCor() {
        yield return new WaitForSeconds(1f);
        Teleport();
    }

    private void OnEndCollision(object sender, HeadsetCollisionEventArgs e) {
        if(waitCoroutine!=null){
            Debug.Log("end Collision event");
            StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }
    }

    private void OnCollision(object sender, HeadsetCollisionEventArgs e) {
        if(waitCoroutine==null){
            Debug.Log("Collision event");
            waitCoroutine=StartCoroutine(waitCor());

        }
    }

    public void MakeTeleport() {
        Teleport();
    }
    private Coroutine waitCoroutine=null;
    private void Teleport() {
        
        if (UiRoot.Instance.posForDefaultTeleport==null)
            teleport.ForceTeleport(Vector3.zero);
        else {
            teleport.ForceTeleport(UiRoot.Instance.posForDefaultTeleport.position);
        }
        Debug.Log("Teleport action");
        waitCoroutine = null;
    }
    
}
