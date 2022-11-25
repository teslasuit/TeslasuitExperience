using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class GrenadeHelper : MonoBehaviour {

    private PhotonView _view;
    public void ChangeGroupToSecond() {
        secondGroup = true;
        _view.RPC("changeToSecond",PhotonTargets.Others);
    }
    [PunRPC]
    private void changeToSecond() {
        secondGroup = true;
    }

    [SerializeField]private bool secondGroup;

    private bool first;
    public void NotifyToInstanciate()
    {
        if (!first)
        {
            first = true;
            InstanciateGrenades.Instance.InstantiatNewGrenade(secondGroup);
        }
    }


    private VRTK_InteractableObject intaractable;
    private void Awake()
    {
        intaractable = GetComponent<VRTK_InteractableObject>();
        intaractable.InteractableObjectGrabbed += Intaractable_InteractableObjectGrabbed;
    }

    private void Intaractable_InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        StopAllCoroutines();
        intaractable.InteractableObjectUngrabbed += Intaractable_InteractableObjectUngrabbed;
        ChangeTranformViwProperty(PhotonTransformViewPositionModel.InterpolateOptions.EstimatedSpeed);

    }

    private void Intaractable_InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        intaractable.InteractableObjectUngrabbed -= Intaractable_InteractableObjectUngrabbed;
        StartCoroutine(waitFallCoroutine());
    }

    private void ChangeTranformViwProperty(PhotonTransformViewPositionModel.InterpolateOptions options)
    {
        GetComponent<PhotonTransformView>().m_PositionModel.InterpolateOption = options;
        _view?.RPC("RemoteChangeTranformViwProperty", PhotonTargets.Others, options);
    }

    [PunRPC]
    private void RemoteChangeTranformViwProperty(PhotonTransformViewPositionModel.InterpolateOptions options)
    {
        GetComponent<PhotonTransformView>().m_PositionModel.InterpolateOption = options;
    }
    private IEnumerator waitFallCoroutine()
    {
        var rb = GetComponent<Rigidbody>();
        while (rb.velocity.magnitude > 0.1f)
        {
            yield return null;
        }
        ChangeTranformViwProperty(PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues);
    }

}
