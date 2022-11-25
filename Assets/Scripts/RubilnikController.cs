using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubilnikController : MonoBehaviour {
    [SerializeField] private Transform HandleVisual;
    [SerializeField] private Transform maxPos;
    [SerializeField] private Transform minPos;
    [SerializeField] private Transform handleInteractive;
    [Range(0,1)]
    public float power;

    private static int emisId = Shader.PropertyToID("_EmissionColor");
    [SerializeField] private Renderer rend;
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    [SerializeField] private Color greenColor;
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    [SerializeField] private Color redColor;
    [SerializeField] private FanController fan;
    [SerializeField] private GramophoneController gramm;
    private Material mat;
    
    private void Start() {
        mat = rend.material;
    }
    private void Update() {
        var pos= HandleVisual.position;
        pos.y = Mathf.Clamp(handleInteractive.position.y, minPos.position.y, maxPos.position.y);
        HandleVisual.position=pos;
        power = Mathf.Clamp01((HandleVisual.position.y - minPos.position.y) /(maxPos.position.y-minPos.position.y));
        if(fan)
            fan.FunPower = power;
        if (gramm)
            gramm.GrammaphonePower = power;
        if (mat) {
            mat.mainTextureOffset=new Vector2(0f,Mathf.Lerp(2.48f,1.98f,power));
            mat.SetColor(emisId,Color.Lerp(greenColor, redColor, power));
        }

    }

}
