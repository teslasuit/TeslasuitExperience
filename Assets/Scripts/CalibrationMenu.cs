using System.Collections.Generic;
using System.Linq;
using TsAPI.Types;
using TsSDK;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;

public class CalibrationMenu : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private float delta = 0.05f;
    [SerializeField] private AnimateScale animate;
    public static CalibrationMenu Instance;

    //[SerializeField] private HapticAnimationAsset animationAsset;
    [SerializeField] private TsHapticAnimationAsset animationAssetNew;
    [SerializeField] private Text mocapText;
    [SerializeField] private Text qualityText;
    [SerializeField] private Text nameText;
    private void Awake() {
        Instance = this;
        if(animationAssetNew==null)Debug.LogWarning("[CalibrationMenu] on " +gameObject.name+ " animationAsset is emty");
    }

    private void OnEnable() {
        if(GameManager.Instance) ChangeMocap(GameManager.Instance.IKMocap);
        GameManager.Instance.OnMocapChange += ChangeMocap;
    }
    private void OnDisable() {
        GameManager.Instance.OnMocapChange -= ChangeMocap;
        StopCalibrationAnimation();
    }
    
    private void ChangeMocap(bool ik) {
        mocapText.text = ik ? "Ik mocap" : "Tesla mocap";
    }


    public void ChangeValue(bool value)
    {
        if(!initialized)
        {
            pwMultiplier = GetMultiplierValue(TsHapticParamType.PulseWidth);

            //GameManager.Instance.CurrentHandle.Suit.Haptic.GetMasterMultiplier(ref multiplayer);
            
            if (slider)
                slider.value = pwMultiplier;
            initialized = true;
        }

        pwMultiplier = Mathf.Clamp01(pwMultiplier + (value ? delta : -delta));
        SetMultiplierValue(TsHapticParamType.PulseWidth, pwMultiplier);
        
        //GameManager.Instance.CurrentHandle.Suit.Haptic.UpdateMasterMultiplier(multiplayer.frequency,multiplayer.amplitude,Mathf.Clamp01(multiplayer.pulse_width +(value? delta : -delta)));
        //GameManager.Instance.CurrentHandle.Suit.Haptic.GetMasterMultiplier(ref multiplayer);
        if (slider)
            slider.value = pwMultiplier;
    }

    private void SetMultiplierValue(TsHapticParamType type, float value)
    {
        var player = GameManager.Instance.GetCurrentHapticPlayer();
        if (player == null)
        {
            return;
        }

        multipliers = player.MasterMultipliers.ToList();

        for (var i = 0; i < multipliers.Count; ++i)
        {
            if (multipliers[i].type == type)
            {
                multipliers[i] = new TsHapticParamMultiplier(type, value);
            }
        }

        player.MasterMultipliers = multipliers;
    }

    private float GetMultiplierValue(TsHapticParamType type)
    {
        var player = GameManager.Instance.GetCurrentHapticPlayer();
        if (player == null)
        {
            return 0.0f;
        }

        multipliers = player.MasterMultipliers.ToList();

        foreach (var multiplier in multipliers)
        {
            if (multiplier.type == type)
            {
                return multiplier.value;
            }
        }

        return 0.0f;
    }

    //private HapticMultiplier multiplayer=new HapticMultiplier();
    private float pwMultiplier;
    private List<TsHapticParamMultiplier> multipliers;
    private bool initialized = false,active;
    //private IHapticAnimation anim_id = null;

    [SerializeField] private UnityEvent OnOpenUi;
    [SerializeField] private UnityEvent OnCloseUi;
	void Update () {
	    if (InputMenuButton()) {
	        if (animate) { 
	            animate.Animate();
	            active = !active;
                if(active)
	                OnOpenUi.Invoke();
                else
                    OnCloseUi.Invoke();

            } else {
                animate = GameManager.Instance.CalibrationAnimateScale;
                animate.Animate();
	            active = !active;
	            
            }
	        if (active) {
                //startAnimation
	            PlayCalibrationAnimation();
            }
            else {
	            StopCalibrationAnimation();
	            //stopAnimation
	        }
	    }
        if(GameManager.Instance==null || GameManager.Instance.CurrentSuitBehaviour == null) {
            qualityText.text = "No connection";
            qualityText.color = Color.red;
            return;
        }
	    if (!initialized) {

	        pwMultiplier = GetMultiplierValue(TsHapticParamType.PulseWidth);
            //GameManager.Instance.CurrentHandle.Suit.Haptic.GetMasterMultiplier(ref multiplayer);
            if (slider)
	            slider.value = pwMultiplier;
            initialized = true;
	    }

	    if (animate && animate.IsOpen)
        {
            var device = GameManager.Instance.GetCurrentDevice();
            if (device != null && nameText)
            {
                nameText.text = device.Ssid;
                qualityText.text = "Connected";
                qualityText.color = Color.green;
            }

            if (InputDownButton())
	            ChangeValue(false);
	        if (InputUpButton())
	            ChangeValue(true);
            
        }

	}



    public void CloseUI() {
        animate.Animate();
        active = !active;
        OnCloseUi.Invoke();
    }

    public void StopCalibrationAnimation() {
        var currentPlayable = GetCurrentPlayable();
        if (currentPlayable != null && currentPlayable.IsPlaying)
        {
            currentPlayable.Stop();
        }
    }

    public void PlayCalibrationAnimation()
    {
        var currentPlayable = GetCurrentPlayable();
        if (currentPlayable != null) 
        {
            currentPlayable.IsLooped = true;
            currentPlayable.Play();
        } else {
            Debug.Log("Failed to play");
        }
    }

    private IHapticPlayable GetCurrentPlayable()
    {
        var hapticPlayer = GameManager.Instance.GetCurrentHapticPlayer();
        if (hapticPlayer != null)
        {
            var playable = GameManager.Instance.GetPlayable(animationAssetNew.Instance);
            return playable;
        }

        return null;
    }

    private bool InputMenuButton()
    {
        if (XRSettings.loadedDeviceName == "Oculus")
            return OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LTouch);

        if (XRSettings.loadedDeviceName == "OpenVR") {
            if(Hand.leftHand) return SteamVR_Controller.Input((int)Hand.leftHand.index).GetPressDown(EVRButtonId.k_EButton_ApplicationMenu);
        }
        return false;
    }

    private bool InputChangeMocapButton() {
        if (XRSettings.loadedDeviceName == "Oculus")
            return OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch);
        if (XRSettings.loadedDeviceName == "OpenVR") {
            if (Hand.rightHand) return SteamVR_Controller.Input((int)Hand.rightHand.index).GetPressDown(EVRButtonId.k_EButton_ApplicationMenu);
        }
        return false;
    }
    private bool InputUpButton(){

        if (XRSettings.loadedDeviceName == "Oculus")
            return OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch);
        if (XRSettings.loadedDeviceName == "OpenVR") {
            if (Hand.leftHand) if (SteamVR_Controller.Input((int)Hand.leftHand.index).GetPressDown(EVRButtonId.k_EButton_SteamVR_Touchpad))
                return SteamVR_Controller.Input((int)Hand.leftHand.index).GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad).y>0.25f;

        }
        return false;
    }
    private bool InputDownButton()
    {
        if (XRSettings.loadedDeviceName == "Oculus")
            return OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch);
        if (XRSettings.loadedDeviceName == "OpenVR")
        {
            if (Hand.leftHand)
                if (SteamVR_Controller.Input((int)Hand.leftHand.index).GetPressDown(EVRButtonId.k_EButton_SteamVR_Touchpad))
                    return SteamVR_Controller.Input((int)Hand.leftHand.index).GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad).y < - 0.25f;

        }
        return false;
    }
}
