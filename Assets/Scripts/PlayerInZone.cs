using UnityEngine;
using UnityEngine.Events;

public class PlayerInZone : MonoBehaviour {
    [SerializeField] private UnityEvent onEnterEvent;
    [SerializeField] private UnityEvent onExitEvent;

    
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            onEnterEvent.Invoke();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            onExitEvent.Invoke();
        }
    }
}
