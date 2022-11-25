using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class SetCurrentCamera : MonoBehaviour {
    private LookAtTarget look;
	void Start () {
	    look = GetComponent<LookAtTarget>();
	    StartCoroutine(waitForCamera());
	}

    private IEnumerator waitForCamera() {
        while (GameManager.Instance==null||GameManager.Instance.LocalCameraTransform==null) {
            yield return null;
        }

        look.target = GameManager.Instance.LocalCameraTransform;
    }
}
