using UnityEngine;

public class CallToActionController : MonoBehaviour {
    [SerializeField] private GameObject label;
    private bool inZone;

    private void Awake() {
        label.SetActive(false);
    }


    public void OnEnterZone() {
        inZone = true;
        label.SetActive(true);
    }

    public void OnExitZone() {
        inZone = false;
        label.SetActive(false);
    }

    public void OnInteract() {

        label.SetActive(false);
    }
}
