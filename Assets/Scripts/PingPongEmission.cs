using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Highlighters;

public class PingPongEmission : MonoBehaviour {

	[SerializeField] private MeshRenderer renderer;
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    [SerializeField] private Color color;
    [SerializeField] private float timeLoop;
    private Color startColor, currentColor;

    private int emis = Shader.PropertyToID("_EmissionColor");
    void Awake() {
        if (renderer) { 
            renderer.material.EnableKeyword("_EMISSION");
            startColor = renderer.material.GetColor(emis);
        }

        if (colorSwap) {
            colorSwap.Initialise(color);
        }
    }

    [SerializeField] private VRTK_MaterialColorSwapHighlighter colorSwap;
	void Update () {
	    var k = Time.time-((int)(Time.time/timeLoop))* timeLoop;
	    currentColor = k < timeLoop/2 ? Color.Lerp(startColor,color,  (k*2/timeLoop)) : Color.Lerp(color,startColor,  ((k- timeLoop/2) * 2 / timeLoop));
        if (renderer) { 
	    
	       
            
            renderer.material.SetColor(emis, currentColor);
        }

	    if (colorSwap) {
	        colorSwap.Highlight(currentColor);
            /*
            if(!grow&&k< timeLoop / 2) { 
                colorSwap.Highlight(color,timeLoop/2);
                grow = true;
            }

	        if (grow && k > timeLoop / 2) {
	            colorSwap.Unhighlight(Color.clear, timeLoop / 2);//Highlight(startColor, timeLoop / 2);
	            grow = false;
            }*/
        }
    }

    private bool grow;
    public void SetEmission(bool state) {
        renderer.material.SetColor(emis, state ? color : startColor);
    }
}
