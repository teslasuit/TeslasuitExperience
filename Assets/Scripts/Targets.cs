using System.Collections.Generic;
using UnityEngine;

public class Targets : MonoBehaviour,IDestroyable {

    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private Transform targetCenter;
    [SerializeField] private Transform targetCenterSide;
    [SerializeField] private Transform BodyRadius;
    [SerializeField] private Transform targetHeadCenter;
    [SerializeField] private Transform HeadRadius;
    [SerializeField] private Transform targetHeadSide;
    [Header("Border")]
    [SerializeField] private Transform Ymax;
    [SerializeField] private Transform Ymin;
    public Vector3 add=new Vector3(0f,0.001f,0f);
    private float radiusBody;
    private float radiusHead;
    private bool drawHit;

    private PhotonView _view;
    private void Awake() {
        radiusBody = (targetCenter.position - BodyRadius.position).magnitude;
        radiusHead = (targetHeadCenter.position - HeadRadius.position).magnitude;
        _view = GetComponent<PhotonView>();
    }

    private List<GameObject> hitPool = new List<GameObject>();

    public void TakeDamage(int amount, Vector3 hitPoint) {
        drawHit = false;
        var headres = (targetHeadCenter.position - hitPoint).magnitude;
        var headresSide = (targetHeadSide.position - hitPoint).magnitude;
        if (headres < headresSide) {
            if (headres > radiusHead) {
                var res = (targetCenter.position - hitPoint).magnitude;
                if (res < radiusBody) {
                    drawHit = true;
                }
            }else{
                drawHit = true;
            }
            if (!drawHit) {
                if (Ymax != null && hitPoint.y < Ymax.position.y)
                    drawHit = true;
                if (Ymin != null && hitPoint.y > Ymin.position.y)
                    drawHit = true;
            }
            if (drawHit) {
                var hole = Instantiate(hitPrefab);
                hole.transform.position = hitPoint+ add;
                hole.transform.rotation = targetCenter.rotation;
                hole.transform.parent = transform;
                var localpos = hole.transform.localPosition;
                localpos.y = add.y;
                hole.transform.localPosition = localpos;
                //Debug.Log(localpos+"  "+ hole.transform.localPosition);
                hole.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
                hole.SetActive(true);
                hitPool.Add(hole);
                _view.RPC("DrawHole", PhotonTargets.Others, hitPoint);
            }
        } else {
            if (headresSide > radiusHead) {
                var res = (targetCenterSide.position - hitPoint).magnitude;
                if (res < radiusBody) {
                    drawHit = true;
                    

                }
            } else{
                
                drawHit = true;
            }
            if (!drawHit) {
                if (Ymax != null && hitPoint.y < Ymax.position.y)
                    drawHit = true;
                if (Ymin != null && hitPoint.y > Ymin.position.y)
                    drawHit = true;
            }
            if (drawHit) {
                var hole = Instantiate(hitPrefab);
                hole.transform.position = hitPoint+ add;
                hole.transform.rotation = targetCenterSide.rotation;
                hole.transform.parent = transform;
                hole.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                var localpos = hole.transform.localPosition;
                localpos.y = add.y;
                hole.transform.localPosition = localpos;

                hitPool.Add(hole); 
                _view.RPC("DrawHole",PhotonTargets.Others, hitPoint);
            }
        }
    }

    public void resetTarget() {
        foreach (var VARIABLE in hitPool){
            Destroy(VARIABLE);
        }
        hitPool = new List<GameObject>();
    }

    [PunRPC]
    private void DrawHole(Vector3 pos) {
        var hole = Instantiate(hitPrefab,transform);
        hole.transform.SetPositionAndRotation(pos + add,targetCenterSide.rotation);                           
        hole.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        hitPool.Add(hole);
    }
}
