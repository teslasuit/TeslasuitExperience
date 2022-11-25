using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualiseMusic : MonoBehaviour {

    private Renderer renderer;
    private Color targetColor = Color.red;
    private float timeOfTouch = 0.05f;
    private float startTouchTime;
    private int emis = Shader.PropertyToID("_EmissionColor");
    private void Awake() {
        renderer = GetComponent<Renderer>();
        renderer.material.EnableKeyword("_EMISSION");
        renderer.material.SetColor(emis,Color.black);
    }

    public void VisualiseTouch(float impact) {
        if (isActiveAndEnabled) {
            startTouchTime = Time.time;
            targetColor = Color.Lerp(Color.green, Color.red, impact);
            renderer.material.SetColor(emis, targetColor);
            changingColor = true;
        }
        
    }

    private bool changingColor;
	void Update () {
	    if (changingColor) {
	        var t = (Time.time - startTouchTime)/timeOfTouch;
	        renderer.material.SetColor(emis, Color.Lerp(targetColor,Color.black,t ));


            if (Time.time - startTouchTime > timeOfTouch) {
                renderer.material.SetColor(emis, Color.black);
                changingColor = false;
	        }
	    }
	}
}
