using System;
using System.Collections;

using TsSDK;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class HapticUiController : MonoBehaviour
{
    public static HapticUiController Instance;
    [SerializeField] public HapticClipInfo[] clips;
    [SerializeField] private HapticPlayerUiElement[] uiElements;

    private int currentClipId=0;


    public HapticClipInfo CurrentClipInfo=null;
     private void Start() {
         if(!Instance)
            Instance = this;
         else {
             Destroy(this);
             return;
         }
         CurrentClipInfo = null;
         ReInitUi();

     }

    private void ReInitUi() {
        var middle = (uiElements.Length - 1) / 2;
        for (int i = 0; i < uiElements.Length; i++) {
            var initId=currentClipId - middle + i;
            if (initId < 0) {
                initId = clips.Length - 1;
            } else if(initId>= clips.Length) {
                initId = 0;
            }
            uiElements[i].Initialize(clips[initId]);
        }
    }

    public void ArrowClick(bool up) {
        if (up)
            currentClipId++;
        else
            currentClipId--;

        if (currentClipId < 0){
            currentClipId = clips.Length - 1;
        } else if (currentClipId >= clips.Length) {
            currentClipId = 0;
        }
        ReInitUi();
    }


    public void StopAll() {
        if (CurrentClipInfo != null){
            CurrentClipInfo.element.ResetUI();
            CurrentClipInfo.Stop();
            StopAnimation();
        }
    }
    

   
    public void GetOutOfZone() {
        Debug.Log("[HapticUiController]: get out from hapticPlayer zone");
    }
    public void GetInZone() {
        Debug.Log("[HapticUiController]: get in hapticPlayer zone");
    }

   
    public void NullAnimId() {
        currentAnimation = null;
    }

    private IHapticPlayable currentAnimation = null;

    public void PlayCurrent() {
        if(currentAnimation!=null)
            StopAnimation();
        else {
            stop = false;
            StopAllCoroutines();
            StartCoroutine(sliderEnumerator(clips[currentClipId]));
            PlayAsset(clips[currentClipId].animationAssetNew);
        }
        
    }


    //public void PlayAsset(IHapticAnimation animation) {
    //    anim_id = animation;
    //    GameManager.Instance.CurrentSuitApiObject.Haptic.SourceMapping.Play(animation);
    //    Debug.Log("Play animation"+animation.ToString());
    //    //anim_id = _handle.Suit.Haptic.PlayById(animation.AssetId);
    //
    //
    //}

    public void PlayAsset(TsHapticAnimationAsset asset)
    {
        var player = GameManager.Instance.GetCurrentHapticPlayer();
        if (player != null)
        {
            currentAnimation = GameManager.Instance.GetPlayable(asset.Instance);
            currentAnimation.Play();
        }

        Debug.Log("Play animation"+asset.name);
    }

    private bool stop = false;
    public void StopAnimation() {
        //if (anim_id != null) {
        //    SetPausePlaySprite(false);
        //    GameManager.Instance.CurrentSuitApiObject.Suit.Haptic.SourceMapping.Stop(anim_id);//.StopPlayable();
        //    //StopAllCoroutines();
        //    stop = true;
        //    anim_id = null;
        //}

        if (currentAnimation != null)
        {
            SetPausePlaySprite(false);
            var player = GameManager.Instance.GetCurrentHapticPlayer();
            if (player != null)
            {
                player.Stop(currentAnimation);
                player.Remove(currentAnimation);
            }
            //StopAllCoroutines();
            stop = true;
            currentAnimation = null;
        }
    }


    private IEnumerator sliderEnumerator(HapticClipInfo hapticInfo) {
        SetPausePlaySprite(true);
        float time = 0f;
        while (time < hapticInfo.Duration){
            yield return null;
            if (!stop)
                time += Time.deltaTime;
            for (int i = 0; i < uiElements.Length; i++) {
                uiElements[i].SetSlider(hapticInfo,time / hapticInfo.Duration);
            }
        }
        SetPausePlaySprite(false);
        NullAnimId();
    }

    [SerializeField] private GameObject playObject;
    [SerializeField] private GameObject pauseObject;
    private void SetPausePlaySprite(bool pause) {
        playObject.SetActive(!pause);
        pauseObject.SetActive(pause);
    }
}

[Serializable]
public class HapticClipInfo {
    public string Name;
    public float Duration;
    public bool isPlaying {
        get { return playing; }
    }

    public string type = "Normal";
    private bool playing;
    public HapticPlayerUiElement element;
    
    public TsHapticAnimationAsset animationAssetNew;
    public void Play() {
        if (playing) {
            Debug.Log("[HapticUiController]" + Name + " stoppedPlaying by button");
            playing = false;
            element.Stop();
            HapticUiController.Instance.StopAnimation();
        } else {
            if (HapticUiController.Instance.CurrentClipInfo != null&& HapticUiController.Instance.CurrentClipInfo.element !=null&& HapticUiController.Instance.CurrentClipInfo != this) {
                HapticUiController.Instance.CurrentClipInfo.element.ResetUI();
                HapticUiController.Instance.CurrentClipInfo.Stop();
                HapticUiController.Instance.StopAnimation(); 
                
            }
            Debug.Log("[HapticUiController]" + Name +" startedPlaying");
            playing = true;
            element.Play();
            
            HapticUiController.Instance.CurrentClipInfo = this;
            if (animationAssetNew == null) Debug.LogWarning("[HapticClipInfo] on " + " animationAsset is emty");
            //HapticUiController.Instance.PlayAsset(animationAsset);
            HapticUiController.Instance.PlayAsset(animationAssetNew);
        }
    }

    public void Stop() {
        
        if (playing) {
            playing = false;
            Debug.Log("[HapticUiController]" + Name + " stopedPlaying");
            HapticUiController.Instance.CurrentClipInfo = null;
            HapticUiController.Instance.StopAnimation();
        }
        
    }
}