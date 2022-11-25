using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class UiButton : VRTK_InteractableObject {

    public UnityEvent onClick;
    public override void StartUsing(VRTK_InteractUse usingObject) {
        base.StartUsing(usingObject);
        onClick.Invoke();
        Debug.Log(name+" OnClick");
    }

    public override void StopUsing(VRTK_InteractUse usingObject) {
        base.StopUsing(usingObject);
        onClick.Invoke();
        Debug.Log(name + " OnClick");
    }

}
