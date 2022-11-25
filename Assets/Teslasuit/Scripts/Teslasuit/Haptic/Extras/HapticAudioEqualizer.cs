using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TeslasuitAPI;
using TsSDK;
using UnityEngine;

public class HapticAudioEqualizer : MonoBehaviour {
    public AudioSource source;
    private float[] samples = new float[FrequencyFilter.SamplesCount];
    float[] fw = new float[FrequencyFilter.SamplesCount];
    public HapticFilterData filtersData;

    [Range(0,1)]
    public float Volume = 0.5f;

    public IHapticPlayer hapticPlayer;

    [Range(10, 100)]
    public float deltaTime = 30f; 
	// Use this for initialization
	void Start () {

        StartCoroutine(AudioRoutine());   
    }

    private bool playing = false;
    private void OnDestroy()
    {
        playing = false;
    }

    private IEnumerator AudioRoutine()
    {
        playing = true;
        while (playing)
        {
            if(source.isPlaying)
            PlayCurrentFrame();
            yield return new WaitForSeconds(deltaTime/1000f);
        }
    }

    void PlayCurrentFrame () {

        if (hapticPlayer == null)
            hapticPlayer = GameManager.Instance.GetCurrentHapticPlayer();
        if(hapticPlayer==null)
            return;
        if (source.isPlaying)
        {
            source.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        }
        else
            return;

        for(int i=0; i< fw.Length; i++)
        {
            fw[i] = samples[i] * (1 + i * i);
        }

        List<FrequencyFilter.FilterResult> polygons = new List<FrequencyFilter.FilterResult>();

        foreach (FrequencyFilter filter in filtersData.data)
        {
            FrequencyFilter.FilterResult filterResult = null;

            if(filter.Process(fw, out filterResult))
            {
                polygons.Add(filterResult);
            }
        }

        EvaluateFrame(polygons);
    }

    private IMapping2dElectricChannel[] GetChannels(IHapticPlayer player, TsHapticSimplifiedChannel simplifiedChannel)
    {
        if (player == null)
        {
            return null;
        }
        var device = player.Device;
        if (device == null)
        {
            return null;
        }
        return device.Mapping2d.ElectricChannels.Where((item) =>
            item.BoneIndex == simplifiedChannel.BoneIndex && item.BoneSide == simplifiedChannel.BoneSide).ToArray();
    }

    private void EvaluateFrame(IEnumerable<FrequencyFilter.FilterResult> polygons)
    {
        foreach(var polyHit in polygons)
        {
            if (polyHit.materialNew)
            {
                if (hapticPlayer.GetPlayable(polyHit.materialNew.Instance) is
                    IHapticMaterialPlayable materialPlayable)
                {
                    materialPlayable.Play();
                    foreach (var simplifiedChannel in polyHit.channels)
                    {
                        var channels = GetChannels(hapticPlayer, simplifiedChannel);
                        foreach (var channel in channels)
                        {
                            hapticPlayer.AddImpact(materialPlayable, channel, polyHit.impact*Volume, 50);
                        }
                    }
                    
                }
            }
        }
    }

}




