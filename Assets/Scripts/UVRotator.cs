using UnityEngine;
using System.Collections;

public class UVRotator : MonoBehaviour {
    public float speedRotationX = 1f;
    public float speedRotationY = 1f;
    public Material material;
    private float x;
    private float y;
    private void OnEnable() {
        var startOffset=material.GetTextureOffset("_MainTex");
        x = startOffset.x;
        y = startOffset.y;
    }

    private void Update() {
        x += Time.deltaTime * speedRotationX;
        y += Time.deltaTime * speedRotationY;
        material.SetTextureOffset("_MainTex", new  Vector2(x ,y));
    }


    public void TurnEmission(bool state) {
        if(state)
            material.EnableKeyword("_EMISSION");
        else 
            material.DisableKeyword("_EMISSION");
        
    }

    public void ResetOffset() {
        material.SetTextureOffset("_MainTex", new Vector2(0, 0));
    }
}
