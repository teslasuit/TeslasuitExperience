using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

public class UiSyncPosition : MonoBehaviour {

    [SerializeField] private RectTransform Container;
    [SerializeField] private float multiplayer;
    private Vector3 startPosContainer;
    [SerializeField] private GameObject[] blocks;
    void Start () {
        startPosContainer = Container.anchoredPosition;
    }
	
	
	void Update () {
	    var y = multiplayer * (Container.anchoredPosition.y - startPosContainer.y);
        transform.localPosition = new Vector3(0f,y, 0f);
	    for (int i = 0; i < blocks.Length; i++) {
            //if(Mathf.Abs(y-i*0.825f)<1.5f*0.825f)
	        blocks[i].SetActive(Mathf.Abs((y+0.4125f) - i * 0.825f) < 1.65f * 0.825f);
            if(maxEntries!=-1) {
                if (i >= maxEntries) blocks[i].SetActive(false);
            }
	    }
	}
    [Button]
    private void calc() {
        multiplayer = transform.localPosition.y/(Container.anchoredPosition.y - startPosContainer.y);
        Debug.Log("Calculated multiplayer: "+multiplayer);
    }

    public int maxEntries = -1;
}
