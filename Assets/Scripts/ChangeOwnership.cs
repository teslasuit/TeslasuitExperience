using UnityEngine;
using UnityEngine.Events;
using VRTK;
[RequireComponent(typeof(PhotonView),typeof(VRTK_InteractableObject))]
public class ChangeOwnership : MonoBehaviour {

    [SerializeField] private UnityEvent OnActivate;
    [SerializeField] private UnityEvent OnDeActivate;
    [SerializeField] private PhotonView[] views;
    private PhotonView _photonView;
    private VRTK_InteractableObject interactable;
    private Rigidbody rigidbody;
    private void Awake() {
        _photonView = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody>();
        interactable = GetComponent<VRTK_InteractableObject>();
        startKinem = rigidbody.isKinematic;
    }

    public bool setKinem = true;
    public bool startKinem;
    [SerializeField] private bool notReturnOwnership=false;
    private void OnEnable() {
        interactable.InteractableObjectGrabbed += OnGrab;
        interactable.InteractableObjectUngrabbed += OnRelease;
    }
    private void OnDisable() {
        interactable.InteractableObjectGrabbed -= OnGrab;
        interactable.InteractableObjectUngrabbed -= OnRelease;
    }
   

    public void OnGrab(object sender, InteractableObjectEventArgs e){
        foreach (var view in views){
            //view.TransferOwnership(0);
            view.TransferOwnership(PhotonNetwork.player);
        }
        _photonView.RPC("turn", PhotonTargets.Others, false, PhotonNetwork.player);
        
        
    }
    public void OnRelease(object sender, InteractableObjectEventArgs e){
        if(!notReturnOwnership)
            foreach (var view in views){
                view.TransferOwnership(0);
                //view.TransferOwnership(PhotonNetwork.player);
            }
        _photonView.RPC("turn", PhotonTargets.Others, true, PhotonPlayer.Find(0));
    }

    [PunRPC]
    private void turn(bool state, PhotonPlayer player) {
        if (state) { 
            OnActivate.Invoke();
            if(setKinem) rigidbody.isKinematic = startKinem;

        }else {
            
            OnDeActivate.Invoke();
            if(setKinem) rigidbody.isKinematic = true;

        }
        //if (PhotonNetwork.isMasterClient)
        //    foreach (var view in views){
        //        //view.TransferOwnership(0);
        //        view.TransferOwnership(player);
        //    }
    }   //
}
