using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeslasuitAPI;
using TsSDK;
using UnityEngine;

public class HapticTriggerObject : MonoBehaviour {
    private int hapticLayer;
    [SerializeField] public TsHapticMaterialAsset materialNew;
    private void Awake() {
        hapticLayer=LayerMask.NameToLayer("HapticReceiver");
    }
    private void OnTriggerStay(Collider collider) {
        if (collider.gameObject.layer == hapticLayer)
        {
            Vector3 toThisVector = collider.bounds.center - transform.position;
            RaycastHit[] hits = Physics.RaycastAll(collider.bounds.center, toThisVector, toThisVector.magnitude);
            if (hits.Length == 0)
            {
                int k = -1;
                for (int i = 0; i < triggerHits.Count; i++)
                {
                    if (triggerHits[i].col == collider)
                    {
                        if (Time.time - triggerHits[i].timePrevHit < 0.2f)
                            return;
                        k = i;
                        break;
                    }
                }

                var handler = collider.transform.GetComponent<TsHapticCollisionHandler>();
                if (handler && materialNew && handler.HapticPlayer)
                {
                    if (handler.HapticPlayer.PlayerHandle.GetPlayable(materialNew.Instance) is
                        IHapticMaterialPlayable materialPlayable)
                    {
                        handler.AddImpact(materialPlayable, 1.0f, 200);
                    }
                    Debug.Log("[HapticTriggerObject]Hit!");
                    if (k != -1)
                    {
                        triggerHits[k].timePrevHit = Time.time;
                    }
                    else
                    {
                        triggerHits.Add(new TriggerHit() {col = collider, timePrevHit = Time.time});
                    }
                }

            }
        }
    }

    private List<TriggerHit> triggerHits=new List<TriggerHit>();

}

public class TriggerHit {
    public Collider col;
    public float timePrevHit;
}
