using EasyButtons;
using UnityEngine;

public class TurnOffVisualisers : MonoBehaviour {

    [Button]
    private void TurnOff() {
        foreach (var visualise in GetComponentsInChildren<VisualiseMusic>()) {
            //Debug.Log(visualise.gameObject.name);
            visualise.gameObject.SetActive(false);
        }
    }
    [Button]
    private void TurnOn()
    {
        foreach (var visualise in GetComponentsInChildren<VisualiseMusic>(true))
        {
            visualise.gameObject.SetActive(true);
        }
    }
}
