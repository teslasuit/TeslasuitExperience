using System.Collections;
using System.Collections.Generic;
using TeslasuitAPI;
using UnityEngine;

public class ChartRenderer : MonoBehaviour {
    [Header( "Left hand")]
    [SerializeField] private GridCreator creator;
    [SerializeField] private GridCreator gsr0n7chCreator;
    [SerializeField] private GridCreator gsr0n11chCreator;
    [Header("RightHand")]
    [SerializeField] private GridCreator EcgRight;
    [SerializeField] private GridCreator gsr1n0chCreator;
    [SerializeField] private GridCreator gsr1n12chCreator;
    public static ChartRenderer Instance=null;
    //private void Awake() {

    //    if(Instance==null)
    //        Instance = this;
    //    else {
    //        Destroy(this);
    //        return;
    //    }

    //    StartCoroutine(waitSuit());

    //    for (int i = 0; i < 1250; i++) {
    //        datalistEcg.Add(0);
    //    }
    //    for (int i = 0; i < 200; i++) {
    //        gsr0n7ch.Add(0);
    //        gsr0n11ch.Add(0);
    //        gsr1n0ch.Add(0);
    //        gsr1n12ch.Add(0);
    //    }
    //}

    private bool startListening;
    //private IEnumerator waitSuit() {
    //    while (GameManager.Instance==null
    //           || GameManager.Instance.CurrentSuitApiObject==null
    //           || !GameManager.Instance.CurrentSuitApiObject.IsAvailable) {
    //        yield return null;
    //    }

    //    startListening = true;
    //    GameManager.Instance.CurrentSuitApiObject.Biometry.StartECG();
    //    GameManager.Instance.CurrentSuitApiObject.Biometry.StartGSR();
    //    GameManager.Instance.CurrentSuitApiObject.Biometry.UpdateECGFrequency(ECGFrequency.TS_ECG_FPS_20);
    //    GameManager.Instance.CurrentSuitApiObject.Biometry.UpdateGSRFrequency(GSRFrequency.TS_GSR_FPS_40);
    //    Debug.Log("ECG started");
    //    GameManager.Instance.CurrentSuitApiObject.Biometry.OnECGUpdated += Biometry_OnECGUpdated;
    //    GameManager.Instance.CurrentSuitApiObject.Biometry.OnGSRUpdated += Biometry_OnGSRUpdated;
    //}

    //private List<int> gsr0n7ch=new List<int>(), gsr0n11ch = new List<int>(), gsr1n0ch = new List<int>(), gsr1n12ch = new List<int>(); 

    //private void Biometry_OnGSRUpdated(ref GSRBuffer gSRBuffer, System.IntPtr opaque, ResultCode resultCode) {
    //    var str = "";
    //    if (resultCode == ResultCode.TS_SUCCESS) {
    //        gsr0n7ch.Add(gSRBuffer.data[0].data[7]);
    //        gsr0n11ch.Add(gSRBuffer.data[0].data[11]);
    //        gsr1n0ch.Add(gSRBuffer.data[1].data[0]);
    //        gsr1n12ch.Add(gSRBuffer.data[1].data[12]);
    //        str = gSRBuffer.data[0].data[7]+"  "+ gSRBuffer.data[0].data[11]+"  "+ gSRBuffer.data[1].data[0]+"  "+gSRBuffer.data[1].data[12];
    //    }
    //    gsr0n7ch.RemoveRange(0, gsr0n7ch.Count - 200);
    //    gsr0n11ch.RemoveRange(0, gsr0n11ch.Count - 200);
    //    gsr1n0ch.RemoveRange(0, gsr1n0ch.Count - 200);
    //    gsr1n12ch.RemoveRange(0, gsr1n12ch.Count - 200);
    //    /*
    //    for (int i = 0; i < gSRBuffer.data.Length; i++) {
    //        str += "gSRBuffer.data["+i+"].data.Length="+gSRBuffer.data[i].data.Length+ "\n";
    //        for (int j = 0; j < gSRBuffer.data[i].data.Length; j++) {
    //            str += gSRBuffer.data[i].data[j]+"  ";
    //        }

    //        str }*/
    //    //Debug.Log("gSRBuffer.data.Length=" + gSRBuffer.data.Length+" "+ resultCode+"\n" +str);
    //    changedgsrL = true;
    //    changedgsrR = true;
    //}

    //private List<int> datalistEcg=new List<int>(1250) ;
    public bool renderLeftHand,renderRightHand;
    //private void Biometry_OnECGUpdated(ref ECGBuffer ECGBuffer, System.IntPtr opaque, ResultCode result) {
    //    for (int i = 0; i < ECGBuffer.data.Length; i++) {
    //        datalistEcg.Add(ECGBuffer.data[i].amplitude);
    //    }

    //    if (datalistEcg.Count > 1250) {
    //        datalistEcg.RemoveRange(0, datalistEcg.Count-1250);
    //    }

    //    changedEcgL = true;
    //    changedEcgR = true;
    //}

    //private bool changedEcgL,changedgsrL;
    //private bool changedEcgR,changedgsrR;
    //private void Update() {
    //    if (renderLeftHand) {
    //        if (changedEcgL) {
    //            creator.DrawChart(datalistEcg);
    //            changedEcgL = false;
    //        }

    //        if (changedgsrL) {
    //            if(gsr0n7chCreator) gsr0n7chCreator.DrawChart(gsr0n7ch);
    //            if(gsr0n11chCreator) gsr0n11chCreator.DrawChart(gsr0n11ch);
    //            if(gsr1n0chCreator) gsr1n0chCreator.DrawChart(gsr1n0ch);
    //            if(gsr1n12chCreator) gsr1n12chCreator.DrawChart(gsr1n12ch);
    //            changedgsrL = false;
    //        }

    //    }
    //    if (renderRightHand) {
    //        if (changedEcgR) {
    //            EcgRight.DrawChart(datalistEcg);
    //            changedEcgR = false;
    //        }

    //        if (changedgsrR) {
    //            if(gsr1n0chCreator) gsr1n0chCreator.DrawChart(gsr1n0ch);
    //            if(gsr1n12chCreator) gsr1n12chCreator.DrawChart(gsr1n12ch);
    //            changedgsrR = false;
    //        }

    //    }
    //}

    public void StopListening() {
        if (!startListening) {
            return;
        }
    //    GameManager.Instance.CurrentSuitApiObject.Biometry.StopECG();
    //    GameManager.Instance.CurrentSuitApiObject.Biometry.StopGSR();
    //    GameManager.Instance.CurrentSuitApiObject.Biometry.OnECGUpdated -= Biometry_OnECGUpdated;
    //    GameManager.Instance.CurrentSuitApiObject.Biometry.OnGSRUpdated -= Biometry_OnGSRUpdated;
    }
   
}
