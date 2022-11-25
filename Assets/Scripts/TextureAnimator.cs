using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAnimator : MonoBehaviour {

    [SerializeField] private string propertyToAnimate;
    [SerializeField] private MeshRenderer[] renderers;
    [SerializeField] private Texture[] textures;
    [SerializeField] private float timeOfLoop = 2f;
    [SerializeField] private bool SortOnAwake;
    [SerializeField] private bool loop = false;
    [SerializeField] private int numMaterial = 0;
    private int propertyToAnimateId;
    private void Awake(){
        if (SortOnAwake) {
            var list = new List<Texture>(textures);
            list.Sort(
                delegate (Texture p1, Texture p2) {
                    return p1.name.CompareTo(p2.name);
                }
            );
            textures = list.ToArray();
        }
    }

    private void OnEnable(){

        propertyToAnimateId = Shader.PropertyToID(propertyToAnimate);

        foreach (var meshRenderer in renderers){
            StartCoroutine(Animate(meshRenderer, textures));
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private IEnumerator Animate(MeshRenderer rend,Texture[] textures) {
        var texturesCount = textures.Length;
        var textureIndex = 0;
        var targetMaterial = rend.materials[numMaterial];
        var delayTime = timeOfLoop/texturesCount;
        var prevind = 0;
        var startTime = Time.time;
        var k = 1;
        while (true){
            targetMaterial.SetTexture(propertyToAnimateId, textures[textureIndex]);
            prevind = textureIndex;
            textureIndex = (textureIndex + 1) % texturesCount;
            if (!loop && textureIndex < prevind)
                break;
            while (Time.time - startTime < delayTime * k){
                yield return null;
            }
            k++;
        }
    }
}
