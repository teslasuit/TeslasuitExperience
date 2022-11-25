using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ChangeRoot : MonoBehaviour {
    [SerializeField] private float waitTime = 0.5f;
    [SerializeField] private Transform newRoot;
    [SerializeField] private bool onlyOculus = false;

    [Header("Oculus settings")]
    [SerializeField] private HumanHand hand;
    [SerializeField] private Transform anotherHand;
    void Start() {
        StartCoroutine(change());
        
    }

    private IEnumerator change() {
        yield return new WaitForSeconds(waitTime);
        //if (onlyOculus && XRSettings.loadedDeviceName == "OpenVR")
        //    yield break;

            transform.SetParent(newRoot);
        /*if (onlyOculus && XRSettings.loadedDeviceName == "Oculus") {
            var Hand=gameObject.AddComponent<Hand>();
            Hand.hand = hand;
            Hand.AnotherHandTransform = anotherHand;
        }*/
    }
}
