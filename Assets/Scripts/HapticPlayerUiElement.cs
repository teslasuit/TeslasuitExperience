using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HapticPlayerUiElement : MonoBehaviour {

    [SerializeField] private Image backImage;
    [SerializeField] private Text name;
    [SerializeField] private Text[] typeTexts;
    [SerializeField] private Text durations;
    [SerializeField] private Image PausePlayImage;
    [SerializeField] private Slider slider;
    [SerializeField] private Sprite PlaySprite;
    [SerializeField] private Sprite PauseSprite;
    private HapticClipInfo hapticInfo;
    public HapticClipInfo HapticInfo {
        get { return hapticInfo; }
    }
    public void Initialize(HapticClipInfo info) {
        hapticInfo = info;
        name.text = hapticInfo.Name;

        //TimeSpan timeSpan = TimeSpan.FromSeconds(hapticInfo.Duration);
        durations.text = hapticInfo.Duration.ToString("F2");
        slider.value = 0;
        foreach (Text typeText in typeTexts) {
            typeText.text = hapticInfo.type;
        }
    }
    public void Play() {
        PausePlayImage.sprite = PauseSprite;
        StopAllCoroutines();
        StartCoroutine(sliderEnumerator());
    }
    public void Stop() {
        PausePlayImage.sprite = PlaySprite;
        StopAllCoroutines();
    }

    public void SetSlider(HapticClipInfo info,float sliderValue) {
        if (info.animationAssetNew == hapticInfo.animationAssetNew) {
            slider.value = sliderValue;
        }
    }


    public void ResetUI() {
        
        StopAllCoroutines();
        PausePlayImage.sprite = PlaySprite;
        slider.value = 0;
    }
    private IEnumerator sliderEnumerator() {

        float startTime = Time.time;
        while (Time.time-startTime<hapticInfo.Duration) {
            yield return null;
            slider.value = ((Time.time - startTime) / hapticInfo.Duration);
        }
        PausePlayImage.sprite = PlaySprite;
        HapticUiController.Instance.NullAnimId();
        HapticUiController.Instance.CurrentClipInfo.Stop();
        HapticUiController.Instance.CurrentClipInfo = null;
    }
}
