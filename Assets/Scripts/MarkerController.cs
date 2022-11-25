using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour {
    public bool InHand {
        set { inHand = value; }
    }

    public LayerMask mask;
    public bool inHand = false;
    [SerializeField] private Color markerColor;
	void OnTriggerStay (Collider other) {
		if(!inHand)
		    return;
	    var board = other.GetComponent<WhiteBoardController>();
	    if (board) {
           
	        RaycastHit hit;
	        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.15f,mask)) {
	            var brd = hit.transform.GetComponent<WhiteBoardController>();
	            if (brd) {
                    brd.Paint(hit.textureCoord,markerColor);
	            }
	        }
	    }
	}
}
