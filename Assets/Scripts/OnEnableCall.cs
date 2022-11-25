using UnityEngine;
using UnityEngine.Events;
public class OnEnableCall : MonoBehaviour {
	[SerializeField] private UnityEvent onEnable;
	[SerializeField] private UnityEvent onDisable;
    [SerializeField] private GameObject[] OnEnableTurnOff;
	void OnEnable () {
		onEnable.Invoke();
	    foreach (GameObject o in OnEnableTurnOff) {
	        o.SetActive(false);
	    }
	}

    void OnDisable() {
        onDisable.Invoke();
    }

}
