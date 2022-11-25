using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class InstanceCubesController : MonoBehaviour {
    public static InstanceCubesController Instance; 
    //private PhotonView _view;
    [SerializeField] private GameObject resourcePrefab;
    private VRTK_InteractableObject interactable;
    private void Awake() {
        Instance = this;
           //_view = GetComponent<PhotonView>();
           interactable = GetComponent<VRTK_InteractableObject>();
    }

    private void OnEnable() {
       // interactable.InteractableObjectUsed += OnUse; 
    }

    private void OnDisable() {
        //interactable.InteractableObjectUsed -= OnUse;
    }
    


    public void InstantiatNewCube() {
        StartCoroutine(instantiateCor());
        onInstanciateCall.Invoke();
    }

    [SerializeField] private UnityEvent onInstanciateCall;
    [SerializeField] private float timeBeforeInst=0.5f;
    private IEnumerator instantiateCor() {
        yield return new WaitForSeconds(timeBeforeInst);
        var go=PhotonNetwork.Instantiate(resourcePrefab.name, transform.position, transform.rotation,
            group: 0);
        go.transform.SetParent(transform);
    }

    private void OnUse(object sender, InteractableObjectEventArgs e) {
        //var go=
            PhotonNetwork.Instantiate(resourcePrefab.name, transform.position, transform.rotation,
            group: 0);
        Debug.Log(e.interactingObject.name);
        e.interactingObject.GetComponent<VRTK_InteractGrab>().AttemptGrab();
        StartCoroutine(cor(e.interactingObject.GetComponent<VRTK_InteractGrab>()));
    }

    private IEnumerator cor(VRTK_InteractGrab grab) {
        yield return null;
        grab.AttemptGrab();
    }

    [SerializeField] private Vector3 rot;
    private void Update() {
        transform.localRotation*=Quaternion.Euler(rot*Time.deltaTime);
    }
}
