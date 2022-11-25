using System;
using UnityEngine;

public class TutorialMotionControllerHelper : MonoBehaviour {
    private PingPongEmission emission;
    private ChangeEmissionMap map;

    private void Awake() {
        emission = GetComponent<PingPongEmission>();
        map = GetComponent<ChangeEmissionMap>();
    }

    public enum EmissionState {
        Default,
        BlinkStick,
        Stick,
        BlinkGrab,
        Grab,
        BlinkMenu,
        XyButtons,
        Trigger
    }

    public void SetDefaultState() {
        SetState(EmissionState.Default);
    }
    public void SetBlinkStickState()
    {
        SetState(EmissionState.BlinkStick);
    }
    public void SetStickState()
    {
        SetState(EmissionState.Stick);
    }
    public void SetBlinkGrabState()
    {
        SetState(EmissionState.BlinkGrab);
    }
    public void SetGrabState()
    {
        SetState(EmissionState.Grab);
    }
    public void SetBlinkMenuState()
    {
        SetState(EmissionState.BlinkMenu);
    }public void SetXyButtonsState()
    {
        SetState(EmissionState.XyButtons);
    }public void SetTriggerState()
    {
        SetState(EmissionState.Trigger);
    }




    public virtual void SetState(EmissionState state) {
        if(map==null||emission==null)
            Awake();
        switch (state) {
            case EmissionState.Default:
                map.ChangeMap(0);
                emission.enabled = false;
                emission.SetEmission(false);
                break;
            case EmissionState.BlinkStick:
                map.ChangeMap(2);
                emission.enabled = true;
                break;
            case EmissionState.Stick:
                map.ChangeMap(2);
                emission.enabled = false;
                emission.SetEmission(true);
                break;
            case EmissionState.BlinkGrab:
                map.ChangeMap(4);
                emission.enabled = true;
                break;
            case EmissionState.Grab:
                map.ChangeMap(4);
                emission.enabled = false;
                emission.SetEmission(true);
                break;
            case EmissionState.BlinkMenu:
                map.ChangeMap(0);
                emission.enabled = true;
                break;
            case EmissionState.XyButtons:
                map.ChangeMap(1);
                emission.enabled = true;
                break;
            case EmissionState.Trigger:
                map.ChangeMap(3);
                emission.enabled = true;
                break;
        }
    }
}
