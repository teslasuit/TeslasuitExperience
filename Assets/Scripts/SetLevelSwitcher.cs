using System.Collections;
using UnityEngine;
using VRTK;

public class SetLevelSwitcher : MonoBehaviour {

    private ChangeOwnership owner;

    private void Awake() {
        owner = GetComponent<ChangeOwnership>();
    }


    public void SetLevel(Transform that) {
        owner.OnGrab(null,new InteractableObjectEventArgs());
        transform.position=new Vector3(transform.position.x, that.position.y, transform.position.z);
        StopAllCoroutines();
        StartCoroutine(returnOwnerShip());
    }

    private IEnumerator returnOwnerShip() {
        yield return new WaitForSeconds(0.5f);
        owner.OnRelease(null, new InteractableObjectEventArgs());
    }
}
