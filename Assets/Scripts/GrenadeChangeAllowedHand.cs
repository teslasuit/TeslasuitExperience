using System.Collections;
using UnityEngine;
using VRTK;

public class GrenadeChangeAllowedHand : MonoBehaviour {
    [SerializeField] private VRTK_InteractableObject innerVrtkObject;
    private VRTK_InteractableObject vrtkObject;
    void Awake()
    {
        vrtkObject = GetComponent<VRTK_InteractableObject>();

    }
    void OnEnable()
    {
        vrtkObject.InteractableObjectGrabbed += HandleGrab;
    }
    
    void OnDisable()
    {
        vrtkObject.InteractableObjectGrabbed -= HandleGrab;
    }
    void HandleGrab(object sender, InteractableObjectEventArgs e) {
        StartCoroutine(changeHand());
    }

    private IEnumerator changeHand() {
        yield return null;
        if (innerVrtkObject == null)
            innerVrtkObject = GetComponentInChildren<VRTK_InteractableObject>();
        Debug.Log(innerVrtkObject.gameObject.name+"   ");
        var dev = GetComponentInParent<Hand>();
        if (dev) {
            Debug.Log(dev.gameObject.name+"   "+ dev.hand);
            if (dev.hand == HumanHand.Left){
                innerVrtkObject.allowedGrabControllers = VRTK_InteractableObject.AllowedController.RightOnly;
                innerVrtkObject.allowedTouchControllers = VRTK_InteractableObject.AllowedController.RightOnly;
            }else{
                innerVrtkObject.allowedGrabControllers = VRTK_InteractableObject.AllowedController.LeftOnly;
                innerVrtkObject.allowedTouchControllers = VRTK_InteractableObject.AllowedController.LeftOnly;
            }

        }
    }
}
