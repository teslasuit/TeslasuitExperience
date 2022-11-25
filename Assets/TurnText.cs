using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnText : MonoBehaviour
{
    
    public void Change()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
