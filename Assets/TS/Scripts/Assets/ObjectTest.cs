using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTest : MonoBehaviour
{
    [SerializeField]
    private TsHapticAnimationAsset m_animationAsset;

    // Start is called before the first frame update
    void Start()
    {
        var instance = m_animationAsset.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        //Destroy(obj);
        //obj = null;
    }
}
