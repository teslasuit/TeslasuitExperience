using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEmission : MonoBehaviour {
    [SerializeField] private EmisInfo[] toTurn;
    public void TurnEmissionOn() {
        foreach (EmisInfo info in toTurn) {
            info.rend.materials[info.numOfMaterial].EnableKeyword("_EMISSION");
        }
    }
}

[Serializable]
public class EmisInfo {
    public Renderer rend;
    public int numOfMaterial;
}