using System;
using System.Collections.Generic;
using EasyButtons;
using TeslasuitAPI;
using UnityEngine;

public class GridCreator : MonoBehaviour {
    [Header("Vertical lines Settings")]
    [SerializeField] private float xStart = 1f;
    [SerializeField] private float xFinish = 17f;
    [SerializeField] private GameObject verticalPrefab;
    [SerializeField] private List<GameObject> verticals;
    public int k = 5;


    [Header("Horizontal lines Settings")]
    [SerializeField] private float yStart = 0f;
    [SerializeField] private float yFinish = 7f;
    [SerializeField] private GameObject horizontalPrefab;
    [SerializeField] private List<GameObject> horizontals;
    public int j = 5;
    [SerializeField] private float maxValue=6f;
    [Header("Chart Settings")]
    [SerializeField] private LineRenderer line;

    [Button]
    private void MakeXGrid() {
        createXGrid(new []{"-5", "-4", "-3","-2" ,"-1","0"});
        line.transform.SetAsLastSibling();
    }
    [Button]
    private void SetZeroValues()
    {
        var poss = new Vector3[k_elements];
        for (int i = 0; i < k_elements; i++)
        {
            poss[i] = new Vector3(Mathf.LerpUnclamped(xStart, xFinish, (i) / (k_elements-1.0f)),
                Mathf.Lerp(yStart, yFinish, (0f) * yFinish / (maxValue - 0.0f)), 0f);


        }
        line.SetPositions(poss);
        line.transform.SetAsLastSibling();
    }


    private void createXGrid(string[] pointsName) {
        for (int i = 0; i < verticals.Count; i++) {
            DestroyImmediate(verticals[i]);
        }
        verticals.Clear();
        k = pointsName.Length;
        for (int i = 0; i < k; i++) {
            var go=Instantiate(verticalPrefab, verticalPrefab.transform.parent);
            go.transform.localPosition=new Vector3(Mathf.Lerp(xStart,xFinish,(i+0.0f)/(k-1.0f)),0f,0f);
            go.SetActive(true);
            go.GetComponent<Line>().text.text = pointsName[i];
            verticals.Add(go);
        }
    }
    [Button]
    public void CreateYGrid(){
        for (int i = 0; i < horizontals.Count; i++) {
            DestroyImmediate(horizontals[i]);
        }
        horizontals.Clear();


        for (int i = 0; i < j; i++) {
            var go = Instantiate(horizontalPrefab, horizontalPrefab.transform.parent);
            go.transform.localPosition = new Vector3(0f, Mathf.Lerp(yStart, yFinish, (i + 1.0f) / (j - 0.0f)), 0f);
            go.SetActive(true);

            //TODO coefficient *10f
            go.GetComponent<Line>().text.text = (Mathf.Lerp(0f, maxValue, (i + 1.0f) / (j - 0.0f))).ToString("F2");
            horizontals.Add(go);
        }
        line.transform.SetAsLastSibling();
    }

    [SerializeField] private int k_elements = 1250;
    public void DrawChart(List<int> ECGBuffer) {
        var poss = new Vector3[ECGBuffer.Count];
        for (int i = 0; i < ECGBuffer.Count; i++) {
            poss[i] = new Vector3(Mathf.LerpUnclamped(xStart,xFinish, (i) / (k_elements-1.0f)),
                   Mathf.Lerp(yStart, yFinish, (ECGBuffer[i]+0f)* yFinish / (maxValue - 0.0f)), 0f);
            
            
        }
        line.SetPositions(poss);
    }

}
