using UnityEngine;

public class ChangeEmissionMap : MonoBehaviour {
    [SerializeField] private Texture2D[] textures;
    [SerializeField] private Renderer rend;

    public void ChangeMap(int k) {
        if(k<textures.Length&&k>=0&& textures[k])
            rend.material.SetTexture("_EmissionMap",textures[k]);
    }
}
