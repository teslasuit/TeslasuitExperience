using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class AnimateScale : MonoBehaviour {
    [SerializeField] private Vector3 target=Vector3.zero;
    [SerializeField] private Vector3 start;
    [SerializeField] private float duration=1f;
    [SerializeField] private Renderer rend;
    [SerializeField] private Material matOpen;
    [SerializeField] private Material matHide;
    private bool forward;
    [SerializeField] private VRTK_Pointer pointer;
    public bool IsOpen {
        get { return forward; }
    }
    public void Animate() {
        if (pointer) pointer.enabled = forward;
        forward = !forward;
        StopAllCoroutines();
        StartCoroutine(animate(forward));
    }



    private IEnumerator animate(bool fwd) {
        float startTime = Time.time;
        if (fwd) {
            if (rend) rend.material = matHide;
            while (Time.time - startTime < duration) {
                yield return null;
                transform.localScale = Vector3.Lerp(transform.localScale, start,(Time.time - startTime) / duration);
            }

            
        } else {
            if(rend)rend.material = matOpen;
            while (Time.time - startTime < duration) {
                yield return null;
                transform.localScale = Vector3.Lerp(transform.localScale, target, (Time.time - startTime) / duration);
            }
            
        }
    }
}
