using System;
using System.Collections;
using EasyButtons;
using RootMotion.FinalIK;
using TsSDK;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using IHapticAsset = TsSDK.IHapticAsset;

public class GameManager : MonoBehaviour {
    public static GameManager Instance = null;
	void Awake () {
	    if (Instance != null) {
            Destroy(this);
            return;
	    }
	    Instance = this;
        DontDestroyOnLoad(gameObject);

        TsManager.Root.SuitManager.OnSuitConnected += OnDeviceConnected;
        TsManager.Root.SuitManager.OnSuitDisconnected += OnDeviceDisconnected;

	    foreach (var suit in TsManager.Root.SuitManager.Suits) {
	        Debug.Log(suit.Ssid);
	    }

	    

	    
        
    }
    

    private void OnDeviceConnected(ISuit obj)
    {
        Debug.Log($"Connected: {obj.Ssid}");
        //SuitIndex = obj.SuitIndex;
    }

    private void OnDeviceDisconnected(ISuit obj)
    {
        Debug.Log($"Disconnected: {obj.Ssid}");
        //SuitIndex = obj.SuitIndex;
    }

    //public SuitAPIObject CurrentSuitApiObject;
    public TsSuitBehaviour CurrentSuitBehaviour;
    //public HapticMesh LocalHapticMesh;
    public Transform LocalCameraTransform;
    public VRIK LocalIk;
    public AnimateScale CalibrationAnimateScale;
    private bool Ik=true;
    public event Action<bool> OnMocapChange;
    public bool IKMocap {
        set { Ik = value;
            if (OnMocapChange!=null)
                OnMocapChange.Invoke(Ik); }
        get { return Ik; }
    }
    


    public IHapticPlayer GetCurrentHapticPlayer()
    {
        var device = GetCurrentDevice();
        if (device != null)
        {
            return device.HapticPlayer;
        }

        return null;
    }

    public IHapticPlayable GetPlayable(IHapticAsset asset)
    {
        var player = GetCurrentHapticPlayer();
        if (asset == null || player == null)
        {
            return null;
        }

        return player.GetPlayable(asset);
    }

    public IDevice GetCurrentDevice()
    {
        var currentSuitBehaviour = Instance.CurrentSuitBehaviour;
        if (currentSuitBehaviour != null)
        {
            return currentSuitBehaviour.Device;
        }

        return null;
    }


}
