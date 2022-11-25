using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MagicInstanciatingController : MonoBehaviour {
    public static MagicInstanciatingController Instance;
    [SerializeField] private bool local = false;
    [SerializeField] private float timeBeforeInst = 0.5f;
    

    [Header( "Magic Fire")]
    [SerializeField] private GameObject resourcePrefab1;
    [SerializeField] private Transform instancePoint1;

    [Header("Magic Black")]
    [SerializeField] private GameObject resourcePrefab2;
    [SerializeField] private Transform instancePoint2;

    [Header("Magic Electric")]
    [SerializeField] private GameObject resourcePrefab3;
    [SerializeField] private Transform instancePoint3;

    [Header("Ice Electric")]
    [SerializeField] private GameObject resourcePrefab4;
    [SerializeField] private Transform instancePoint4;
    private void Awake() {
        Instance = this;
    }

    [SerializeField] private UnityEvent onCallToInstaciate;
    public void InstantiatNewMagic(MagicType type) {
        switch (type) {
            case MagicType.Fire:
                StartCoroutine(instantiateCor(resourcePrefab1,instancePoint1));
                break;
            case MagicType.Black:
                StartCoroutine(instantiateCor(resourcePrefab2, instancePoint2));
                break;
            case MagicType.Electric:
                StartCoroutine(instantiateCor(resourcePrefab3, instancePoint3));
                break;
           case MagicType.Ice:
                StartCoroutine(instantiateCor(resourcePrefab4, instancePoint4));
                break;
        }
        onCallToInstaciate.Invoke();
    }

    
    private IEnumerator instantiateCor(GameObject resourcePrefab, Transform point)
    {
        yield return new WaitForSeconds(timeBeforeInst);
        if (!local) { 
        var go = PhotonNetwork.Instantiate(resourcePrefab.name, point.position, point.rotation,
            group: 0);

        } else {
            Instantiate(resourcePrefab, point.position, point.rotation);
        }
    }
    
}

public enum MagicType {
    Fire,
    Black,
    Electric,
    Ice
}