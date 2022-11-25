using UnityEngine;
using UnityEngine.XR;

public class MotionControllersManager : MonoBehaviour {
    [SerializeField] private TutorialMotionControllerHelper[] helpers;
    [SerializeField] private TweenRotation[] tweens;
    [SerializeField] private CustomLookAt[] lookAts;
    [SerializeField] private GameObject[] lasers;
    [SerializeField] private GameObject[] oculusObjects;
    [SerializeField] private GameObject[] viveObjects;
    private void Awake() {
        if (XRSettings.loadedDeviceName == "Oculus") {
            foreach (var oculusObject in oculusObjects) {
                oculusObject.SetActive(true);
            }
            foreach (var viveObj in viveObjects) {
                viveObj.SetActive(false);
            }
        } else {
            foreach (var oculusObject in oculusObjects)
            {
                oculusObject.SetActive(false);
            }
            foreach (var viveObj in viveObjects)
            {
                viveObj.SetActive(true);
            }
        }
    }


    public void TurnLookRotationAndlasers(bool state) {
        foreach (TweenRotation tweenRotation in tweens) {
            tweenRotation.enabled = !state;
        }

        foreach (CustomLookAt customLookAt in lookAts) {
            if(state)
                customLookAt.enabled = state;
            else {
                customLookAt.ReturnToIdentityRotation();
            }
        }

        foreach (GameObject laser in lasers) {
            laser.SetActive(state);
        }
    }

    public void SetDefaultState()
    {
        foreach (TutorialMotionControllerHelper helper in helpers)
            helper.SetState(TutorialMotionControllerHelper.EmissionState.Default);
    }
    public void SetBlinkStickState()
    {
        foreach (TutorialMotionControllerHelper helper in helpers)
            helper.SetState(TutorialMotionControllerHelper.EmissionState.BlinkStick);
    }
    public void SetStickState()
    {
        foreach (TutorialMotionControllerHelper helper in helpers)
            helper.SetState(TutorialMotionControllerHelper.EmissionState.Stick);
    }
    public void SetBlinkGrabState()
    {
        foreach (TutorialMotionControllerHelper helper in helpers)
            helper.SetState(TutorialMotionControllerHelper.EmissionState.BlinkGrab);
    }
    public void SetGrabState()
    {
        foreach (TutorialMotionControllerHelper helper in helpers)
            helper.SetState(TutorialMotionControllerHelper.EmissionState.Grab);
    }
    public void SetBlinkMenuState()
    {
        foreach (TutorialMotionControllerHelper helper in helpers)
            helper.SetState(TutorialMotionControllerHelper.EmissionState.BlinkMenu);
    }
    public void SetXyButtonsState()
    {
        foreach (TutorialMotionControllerHelper helper in helpers)
            helper.SetState(TutorialMotionControllerHelper.EmissionState.XyButtons);
    }
    public void SetTriggerState()
    {
        foreach (TutorialMotionControllerHelper helper in helpers)
            helper.SetState(TutorialMotionControllerHelper.EmissionState.Trigger);
    }
}
