using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

public class Hand : MonoBehaviour {

    public HumanHand hand;
    public SteamVR_TrackedObject trackedObjects;
    public static SteamVR_TrackedObject leftHand;
    public static SteamVR_TrackedObject rightHand;
    public Transform AnotherHandTransform;
    private bool oculusInput;
    private void Start() {
        if(trackedObjects==null) trackedObjects=GetComponent<SteamVR_TrackedObject>();

        oculusInput = XRSettings.loadedDeviceName == "Oculus";
        if (!oculusInput) {
            switch (hand) {
                case HumanHand.Left:
                    leftHand = trackedObjects;
                    break;
                case HumanHand.Right:
                    rightHand = trackedObjects;
                    break;
            }
        }
    }

    public bool GetDownTrigger() {
        if (oculusInput){
            return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, hand == HumanHand.Left ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
        } else {
            if (trackedObjects == null) return false;
            return SteamVR_Controller.Input((int) trackedObjects.index).GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger);
        }
        
    }

    public float GetTrigger() {
        if (oculusInput){
            return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, hand == HumanHand.Left ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
        } else {
            if (trackedObjects == null) return 0f;
            return SteamVR_Controller.Input((int)trackedObjects.index).GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger).x;
        }
        
    }
    private OVRHapticsClip hapticsProceduralClip = new OVRHapticsClip();
    public void HapticPulseOnIndex(int Numdev, float strength = 0.5f)
    {
        hapticsProceduralClip.WriteSample((byte)(strength * byte.MaxValue));
        if (Numdev == 0)
            OVRHaptics.LeftChannel.Preempt(hapticsProceduralClip);
        else if (Numdev == 1)
            OVRHaptics.RightChannel.Preempt(hapticsProceduralClip);
        else if (Numdev == -1)
        {
            OVRHaptics.RightChannel.Preempt(hapticsProceduralClip);
            OVRHaptics.LeftChannel.Preempt(hapticsProceduralClip);
        }

    }
    IEnumerator LongVibration(SteamVR_Controller.Device device, float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            if (device != null) device.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
            yield return null;
        }
    }
    IEnumerator LongVibration(int numdev, float length, float strength)
    {
        HapticPulseOnIndex(numdev, strength);

        yield return new WaitForSeconds(length);
        if (numdev == 0)
            OVRHaptics.LeftChannel.Clear();
        else if (numdev == 1)
            OVRHaptics.RightChannel.Clear();
        else if (numdev == -1)
        {
            OVRHaptics.RightChannel.Clear();
            OVRHaptics.LeftChannel.Clear();
        }
    }
    IEnumerator LongVibration(SteamVR_Controller.Device device, int vibrationCount, float vibrationLength, float gapLength, float strength)
    {
        strength = Mathf.Clamp01(strength);
        for (int i = 0; i < vibrationCount; i++)
        {
            if (i != 0) yield return new WaitForSeconds(gapLength);
            yield return StartCoroutine(LongVibration(device, vibrationLength, strength));
        }
    }
    IEnumerator LongVibration(int numdev, int vibrationCount, float vibrationLength, float gapLength, float strength)
    {
        strength = Mathf.Clamp01(strength);
        for (int i = 0; i < vibrationCount; i++)
        {
            if (i != 0) yield return new WaitForSeconds(gapLength);
            yield return StartCoroutine(LongVibration(numdev, vibrationLength, strength));
            yield return new WaitForSeconds(gapLength);
            if (numdev == 0)
                OVRHaptics.LeftChannel.Clear();
            else if (numdev == 1)
                OVRHaptics.RightChannel.Clear();
            else if (numdev == -1)
            {
                OVRHaptics.RightChannel.Clear();
                OVRHaptics.LeftChannel.Clear();
            }
        }
    }

    public void Vibro(int Numdev, float length, float strength)
    {
        if (UnityEngine.XR.XRSettings.loadedDeviceName != "OpenVR")
        {
            StartCoroutine(LongVibration(Numdev, length, strength));
            return;
        }
        if (Numdev == 0)
            StartCoroutine(LongVibration(SteamVR_Controller.Input((int)trackedObjects.index), length, strength));
        else if (Numdev == 1)
            StartCoroutine(LongVibration(SteamVR_Controller.Input((int)trackedObjects.index), length, strength));
        else if (Numdev == -1)
        {
            StartCoroutine(LongVibration(SteamVR_Controller.Input((int)trackedObjects.index), length, strength));
            StartCoroutine(LongVibration(SteamVR_Controller.Input((int)trackedObjects.index), length, strength));
        }
        else Debug.Log("Wrong Num dev");
    }
    public void PulseVibro(int Numdev, int vibrationCount, float vibrationLength, float gapLength, float strength)
    {
        if (UnityEngine.XR.XRSettings.loadedDeviceName != "OpenVR")
        {
            StartCoroutine(LongVibration(Numdev, vibrationCount, vibrationLength, gapLength, strength));
            return;
        }
        if (Numdev == 0)
            StartCoroutine(LongVibration(SteamVR_Controller.Input((int)trackedObjects.index), vibrationCount, vibrationLength, gapLength, strength));
        else if (Numdev == 1)
            StartCoroutine(LongVibration(SteamVR_Controller.Input((int)trackedObjects.index), vibrationCount, vibrationLength, gapLength, strength));
        else if (Numdev == -1)
        {
            StartCoroutine(LongVibration(SteamVR_Controller.Input((int)trackedObjects.index), vibrationCount, vibrationLength, gapLength, strength));
            StartCoroutine(LongVibration(SteamVR_Controller.Input((int)trackedObjects.index), vibrationCount, vibrationLength, gapLength, strength));
        }
        else Debug.Log("Wrong Num dev");
    }

}

public enum HumanHand {
    Left,
    Right
}