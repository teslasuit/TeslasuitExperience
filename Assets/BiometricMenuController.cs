using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Valve.VR;

public class BiometricMenuController : MonoBehaviour
{
    private bool  active;
    [SerializeField] private AnimateScale animate;
    [SerializeField] private UnityEvent OnOpenUi;
    [SerializeField] private UnityEvent OnCloseUi;
    void Update()
    {
        if (InputMenuButton()) {
            if (animate) {
                animate.Animate();
                active = !active;
                if (active)
                    OnOpenUi.Invoke();
                else
                    OnCloseUi.Invoke();

            }
        }
    }

    private bool InputMenuButton()
    {

        if (XRSettings.loadedDeviceName == "Oculus")
            return OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch);
        if (XRSettings.loadedDeviceName == "OpenVR")
        {
            if (Hand.rightHand) return SteamVR_Controller.Input((int)Hand.rightHand.index).GetPressDown(EVRButtonId.k_EButton_ApplicationMenu);

        }
        return false;
    }
}
