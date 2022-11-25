using UnityEngine;
using System.Collections;

public class TurnOffIn : MonoBehaviour {

    [SerializeField] private float lifetime = 5f;
    [SerializeField] private bool destroy;
    private float borntime;

    private void OnEnable(){
        borntime = Time.time;
    }

    void Update(){
        if (borntime + lifetime < Time.time)
            if (destroy) {
                DestroyObject(gameObject);
                                    
           }
            else
                gameObject.SetActive(false);
    }
}
