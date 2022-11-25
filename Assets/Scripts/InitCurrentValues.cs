using TeslasuitAPI;
using UnityEngine;

public class InitCurrentValues : MonoBehaviour
{
    
    void Start() {
        var suit = GetComponent<TsSuitBehaviour>();
        //GameManager.Instance.CurrentSuitApiObject = suit;
        GameManager.Instance.CurrentSuitBehaviour = suit;
    }

    
}
