using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HapticFilterData : MonoBehaviour {

    public FrequencyFilter[] data;

   
}


[Serializable]
public class FrequencyFilter
{

    public class FilterResult {
        //public HapticPolygonAsset[] polygons;
        public TsHapticSimplifiedChannel[] channels;
        public float impact;
        public TsHapticMaterialAsset materialNew;

    }
    public bool activeFilter=true;
    public VisualiseMusic[] rend;
    [MinMaxSlider(0f, 1f)]
    public Vector2 bandWidth = new Vector2(0.4f, 1f);

    public TsHapticMaterialAsset materialNew;

    public const int SamplesCount = 64;
    [Range(0, SamplesCount - 1)]
    public int frequencyIndex = 40;

    [Range(0, SamplesCount - 1)]
    public int freqWidth = 5;

    public float multiplayer = 1f;
    //public HapticPolygonAsset[] polygons;
    public TsHapticSimplifiedChannel[] channels;

    public bool Process(float[] samples, out FilterResult result) {
        if (!this.activeFilter) {
            result = null;
            return false;
        }

        float avgAmplitude = getAvg(samples, frequencyIndex, freqWidth);

        if (avgAmplitude >= bandWidth.x)
        {
            result = new FilterResult();
            result.impact = Mathf.Clamp01((avgAmplitude - bandWidth.x) / (bandWidth.y - bandWidth.x));
            result.impact = Mathf.Clamp01(result.impact*multiplayer);
            result.channels = channels;
            result.materialNew = materialNew;
#if UNITY_EDITOR
            Visualise(result.impact);
#endif
            return true;
        }
        else
        {
            result = null;
            return false;
        }

    }

    private void Visualise(float impact) {
        foreach (var VARIABLE in rend) {
            if(VARIABLE)VARIABLE.VisualiseTouch(impact);
        }
    }

    private float getAvg(float[] src, int index, int count)
    {
        float[] range = getRange(src, index, count);

        float sum = 0f;

        for (int i = 0; i < range.Length; i++)
            sum += range[i];
        return sum / range.Length;
    }

    private float[] getRange(float[] src, int index, int count)
    {
        float[] result = new float[0];

        int rightWidth = src.Length - 1 - index;
        int leftWidth = index;

        if (count <= rightWidth)
            rightWidth = count;
        if (count <= leftWidth)
            leftWidth = count;



        Array.Resize(ref result, rightWidth + leftWidth + 1);

        Array.Copy(src, index - leftWidth, result, 0, leftWidth);
        Array.Copy(src, index, result, 0 + leftWidth, 1);
        Array.Copy(src, index + 1, result, 0 + leftWidth + 1, rightWidth);

        return result;
    }
}