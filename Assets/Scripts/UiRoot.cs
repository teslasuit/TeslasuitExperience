using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiRoot : MonoBehaviour {
    public static UiRoot Instance=null;
	void Awake () {
	    if (Instance == null) {
	        Instance = this;
	    }

	}
	void OnDestroy () {
	    Instance = null;
	}

    public Transform posForDefaultTeleport;
}
