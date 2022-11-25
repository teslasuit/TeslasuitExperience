using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstanciateGrenades : MonoBehaviour {

    [SerializeField] private float timeBeforeInst = 0.5f;

    [SerializeField] private GameObject resourcePrefab;
    [SerializeField] private Transform firstGroupTransform;
    [SerializeField] private Transform secondGroupTransform;
    public static InstanciateGrenades Instance;
    private void Awake() {
        Instance = this;
    }

    [SerializeField] private UnityEvent onInstanciateCall;
    public void InstantiatNewGrenade(bool secondGroup=false)
    {
        StartCoroutine(instantiateCor(secondGroup));
        onInstanciateCall.Invoke();
    }
    private IEnumerator instantiateCor(bool secondgroup) {
        yield return new WaitForSeconds(timeBeforeInst);
        var go = PhotonNetwork.Instantiate(resourcePrefab.name, (secondgroup?secondGroupTransform:firstGroupTransform).position, 
            (secondgroup ? secondGroupTransform : firstGroupTransform).rotation,
            group: 0);
        go.transform.SetParent(transform);
    }
    
}
