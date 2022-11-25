using UnityEngine;

public class UiPositionSetter : MonoBehaviour {
    
	void Start () {
	    if (UiRoot.Instance) {
            transform.SetParent(UiRoot.Instance.transform);
            transform.localPosition=Vector3.zero;
            transform.localRotation=Quaternion.identity;
	    }
	}
	
	
}
