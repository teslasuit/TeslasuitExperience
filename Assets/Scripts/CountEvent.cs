using UnityEngine;
using UnityEngine.Events;

public class CountEvent : MonoBehaviour {
    [SerializeField] private int CountForEvent = 2;
    [SerializeField] private UnityEvent onCountReachEvent;
    private int k;
    private bool reached;
    public void AddEvent() {
        Debug.Log(name+"add event");
        k++;
        if (!reached && k >= CountForEvent) {

            onCountReachEvent.Invoke();
            reached = true;
        }
    }
}
