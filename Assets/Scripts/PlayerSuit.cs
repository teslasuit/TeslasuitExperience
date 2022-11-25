using System.Collections;
using TeslasuitAPI;
using UnityEngine;

public class PlayerSuit : MonoBehaviour {

	void Start () {
	    if (GameManager.Instance) {
            if (GameManager.Instance.CurrentSuitBehaviour != null) {
                //foreach (HapticMesh haptic in haptics)
                {
                    //haptic.Init(GameManager.Instance.CurrentSuitBehaviour);

                }
                return;
            } 
	        
	    }

	    StartCoroutine(waitCor());
	}


    private IEnumerator waitCor() {
        while (GameManager.Instance==null|| GameManager.Instance.CurrentSuitBehaviour == null) {
            yield return null;
        }

        //foreach (HapticMesh haptic in haptics) {
        //    haptic.Init(GameManager.Instance.CurrentSuitBehaviour);
        //}
    }

    private void OnDisable() {
        
    }


    
}
