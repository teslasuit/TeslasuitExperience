using System.Collections;
using UnityEngine;
using VRTK;

public class CubeHelper : MonoBehaviour {
    private VRTK_InteractableObject intaractable;
    private void Awake() {
        _view = GetComponent<PhotonView>();
        intaractable = GetComponent<VRTK_InteractableObject>();
        intaractable.InteractableObjectGrabbed += Intaractable_InteractableObjectGrabbed;
    }

    private void Intaractable_InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e) {
        StopAllCoroutines();
        intaractable.InteractableObjectUngrabbed += Intaractable_InteractableObjectUngrabbed;
        ChangeTranformViwProperty(PhotonTransformViewPositionModel.InterpolateOptions.EstimatedSpeed);
        
    }

    private void Intaractable_InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e) {
        intaractable.InteractableObjectUngrabbed -= Intaractable_InteractableObjectUngrabbed;
        StartCoroutine(waitFallCoroutine());
    }

    private IEnumerator waitFallCoroutine() {
        var rb=GetComponent<Rigidbody>();
        while (rb.velocity.magnitude>0.1f) {
            yield return null;
        }
        ChangeTranformViwProperty(PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues);
    }

    public void ChangeToNotKinematic() {
        intaractable.PreviousKinematicState = false;
        _view.RPC("RemoteChangeToNotKinematic", PhotonTargets.Others);

    }
    public void ChangeToNullParent()
    {
        intaractable.PreviousParent = null;
        _view.RPC("RemoteChangeToNullParent", PhotonTargets.Others);
    }
    
    public void ChangeMaterial() {
        //Debug.Log("Change Material call");
        //GetComponent<Renderer>().material = mat;
    }

    private bool first;
    public void NotifyToInstanciate() {
        if (!first) {
            first = true;
            InstanceCubesController.Instance.InstantiatNewCube();
        }
    }

    public void ReceiveImpulse(Vector3 impulse) {
        _view.RPC("RemoteImpulse", PhotonTargets.MasterClient,impulse);
    }

    [PunRPC]
    private void RemoteImpulse(Vector3 impulse) {
        GetComponent<Rigidbody>().AddForce(impulse);
    }

    [PunRPC]
    private void RemoteChangeToNullParent() {
        intaractable.PreviousParent = null;
    }

    [PunRPC]
    private void RemoteChangeToNotKinematic() {
        GetComponent<Rigidbody>().isKinematic = false;
    }
    private PhotonView _view;

    private void ChangeTranformViwProperty(PhotonTransformViewPositionModel.InterpolateOptions options) {
        GetComponent<PhotonTransformView>().m_PositionModel.InterpolateOption = options;
        _view.RPC("RemoteChangeTranformViwProperty", PhotonTargets.Others, options);
    }

    [PunRPC]
    private void RemoteChangeTranformViwProperty(PhotonTransformViewPositionModel.InterpolateOptions options) {
        GetComponent<PhotonTransformView>().m_PositionModel.InterpolateOption = options;
    }



}
