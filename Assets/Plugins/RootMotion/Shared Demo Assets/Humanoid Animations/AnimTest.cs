using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AnimTest : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(skinnedMeshRenderer == null)
        {
            return;
        }

        for(int i=0; i<skinnedMeshRenderer.bones.Length; ++i)
        {
            var transform = skinnedMeshRenderer.bones[i];
            var pose = skinnedMeshRenderer.sharedMesh.bindposes[i];
            transform.localRotation = pose.rotation;
        }
    }
}
