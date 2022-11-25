using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieAnimator : MonoBehaviour {
    [SerializeField] private Light _light;
    [SerializeField] private Texture2D[] textures;
    [SerializeField] private float time=0.15f;
    private int k = 0;
    private float prevTime;
	void OnEnable () {
	    prevTime = Time.time;

	}
	
	void Update () {
	    if (Time.time - prevTime > time) {
	        k++;
	        if (k >= textures.Length) {
	            k = 0;
	        }

	        _light.cookie = textures[k];
	        prevTime = Time.time;

        }
		
	}
}
