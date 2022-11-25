using System;
using System.Collections;
using TeslasuitAPI;
using UnityEngine;

public class VibrationHandler : MonoBehaviour
{
    public enum vibroType
    {
        simple = 0,
        pulse = 1
    }

    public Hand Device;
    [SerializeField] private bool both;
    [SerializeField] private vibroType type = vibroType.simple;
    [Header("SimpleVibro")]
    [SerializeField] private float lenth;
    [Range(0.0f, 1.0f)] [SerializeField] private float strenght;
    [Header("PulseVibro")] [SerializeField] private int vibrationCount;
    [SerializeField] private float vibrationLength;
    [SerializeField] private float gapLength;
    [Range(0.0f, 1.0f)] [SerializeField] private float pulsestrenght;
    [Header("Tesla Haptic")]
    //[SerializeField] public HapticAnimationAsset animationLeftAsset;
    //[SerializeField] public HapticAnimationAsset animationRightAsset;

    [SerializeField] public TsHapticAnimationAsset animationLeftAssetNew;
    [SerializeField] public TsHapticAnimationAsset animationRightAssetNew;
    private void Awake()
    {
        //if (animationLeftAsset == null) Debug.LogWarning("[VibrationHandler] animationLeftAsset on " + gameObject.name + " is empty");
        //if (animationRightAsset == null) Debug.LogWarning("[VibrationHandler] animationRightAsset on " + gameObject.name + " is empty");

        if (animationLeftAssetNew == null) Debug.LogWarning("[VibrationHandler] animationLeftAsset on " + gameObject.name + " is empty");
        if (animationRightAssetNew == null) Debug.LogWarning("[VibrationHandler] animationRightAsset on " + gameObject.name + " is empty");
    }

    private void OnEnable()
    {
        var shoot = GetComponent<ShootOnClick>();
        
        if(shoot)shoot.Shoot += HandleVibro;
        if(shoot)shoot.Shoot += haptic;
        var overh = GetComponent<OverheatableShootOnClick>();
        if(overh)overh.Shoot += HandleVibro;
        if(overh)overh.Shoot += haptic;
    }
    private void OnDisable(){
        var shoot = GetComponent<ShootOnClick>();
        if (shoot) shoot.Shoot -= HandleVibro;
        if (shoot) shoot.Shoot -= haptic;
        var overh = GetComponent<OverheatableShootOnClick>();
        if (overh) overh.Shoot -= HandleVibro;
        if (overh) overh.Shoot -= haptic;
    }

    private void HandleVibro()
    {
        if(Device==null)
            return;
        if (type == vibroType.simple){
            if (!both) Device.Vibro(Device.hand==HumanHand.Left?0:1, lenth, strenght);
            else Device.Vibro(-1, lenth, strenght);
            

        } else {
            if (!both) Device.PulseVibro(Device.hand == HumanHand.Left ? 0 : 1, vibrationCount, vibrationLength, gapLength, pulsestrenght);
            else Device.PulseVibro(-1, vibrationCount, vibrationLength, gapLength, pulsestrenght);
        }

    }

    private void haptic() {
        if(Device==null)
            return;

        var player = GameManager.Instance.GetCurrentHapticPlayer();

        if (player != null)
        {
            switch (Device.hand) {
                case HumanHand.Left:
                    if (animationLeftAssetNew)
                    {
                        var playable = GameManager.Instance.GetPlayable(animationLeftAssetNew.Instance);
                        
                        player.Play(playable);
                    }
                    else{
                        Debug.LogWarning("[VibrationHandler] animationLeft donot initialized");
                    }
                    break;
                case HumanHand.Right:
                    if (animationRightAssetNew)
                    {
                        var playable = GameManager.Instance.GetPlayable(animationRightAssetNew.Instance);
                        player.Play(playable);
                    }
                    else {
                        Debug.LogWarning("[VibrationHandler] animationRight donot initialized");
                    }
                    break;
            }
        }
        else {
            Debug.LogWarning("[VibrationHandler] GameManager or GameManager.Instance.CurrentHandle donot initialized");
        }

    }
}
