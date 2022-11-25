
using System;
using System.Collections;
using TeslasuitAPI;
using TsSDK;
using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class MagicController : MonoBehaviour {
    [SerializeField] private GameObject prefab;
    [SerializeField] private float safeTimeAfterRelease = 0.5f;
    [SerializeField] private ParticleSystem particles;
    private PhotonView photonView;
    private float timeRelease;
    private bool grabbed;
    private bool detonated;
    private bool firstGrab;
    private void Awake() {
        photonView = GetComponent<PhotonView>();
        if (animationLeftHandAssetNew == null) Debug.LogWarning("[MagicController] animationLeftHandAssetNew on " + gameObject.name + " is empty");
        if (animationRightHandAssetNew == null) Debug.LogWarning("[MagicController] animationRightHandAssetNew on " + gameObject.name + " is empty");
    }

    public void OnGrab() {
        grabbed = true;
        StopAllCoroutines();
        if (!firstGrab) {
            GetComponent<VRTK_InteractableObject>().PreviousKinematicState = false;
        }
        firstGrab = true;
        if(!GameManager.Instance || GameManager.Instance.CurrentSuitBehaviour == null){
            Debug.LogError("[MagicController] handle is null ");
            return;
        }
        StartCoroutine(checkHand());
    }

    private IEnumerator checkHand() {
        yield return null;
        var hand = GetComponentInParent<Hand>();
        var player = GameManager.Instance.GetCurrentHapticPlayer();
        
        if (hand && player != null)
            switch (hand.hand) {
                case HumanHand.Left:
                    if (animationLeftHandAssetNew)
                    {
                        currentPlayable = GameManager.Instance.GetPlayable(animationLeftHandAssetNew.Instance);
                        currentPlayable.IsLooped = true;
                        currentPlayable.Play();
                    }
                    break;
                case HumanHand.Right:
                    if (animationRightHandAssetNew) {
                        currentPlayable = GameManager.Instance.GetPlayable(animationRightHandAssetNew.Instance);
                        currentPlayable.IsLooped = true;
                        currentPlayable.Play();
                    }
                    break;
            }
        else {
            Debug.LogError("[MagicController]Do not find hand");

        }

    }


    private IHapticAnimation anim_id=null;
    private IHapticPlayable currentPlayable = null;
    public void OnUngrab() {
        timeRelease = Time.time;
        grabbed = false;
        var player = GameManager.Instance.GetCurrentHapticPlayer();
        if (currentPlayable != null && player != null)
        {
            player.Stop(currentPlayable);
            currentPlayable = null;
        }
        currentPlayable = null;
    }


    private void OnCollisionEnter(Collision collision) {

        if(!firstGrab)
            return;
        if (grabbed) {

        } else {
            //Debug.Log(collision.collider.gameObject.name);
            if (Time.time - timeRelease <safeTimeAfterRelease) {
                if (photonView) { 
                    if (photonView.isMine)
                        StartCoroutine(detonateIn(safeTimeAfterRelease - (Time.time - timeRelease)));
                }else
                    StartCoroutine(detonateIn(safeTimeAfterRelease - (Time.time - timeRelease)));
            } else {
                if(photonView)
                    photonView.RPC("detonate",PhotonTargets.AllViaServer);
                else 
                    detonate();
            }
        }
    }

    private IEnumerator detonateIn(float time) {
        yield return new WaitForSeconds(time);
        if (!grabbed) {
            if(photonView)photonView.RPC("detonate", PhotonTargets.AllViaServer);
            else {
                detonate();
            }
        }
    }
    [PunRPC]
    private void detonate() {
        if(detonated)
            return;
        onDetonate.Invoke();
        detonated = true;
        var go=Instantiate(prefab, transform.position, transform.rotation);
        go.SetActive(true);
        particles.transform.parent = null;
        var emis=particles.emission;
        emis.enabled=false;
        Destroy(particles.gameObject,2.5f);
        if (photonView) { 
            if(photonView.isMine){
            PhotonNetwork.Destroy(gameObject);
            //NotifyToInstanciate();
            }
        }else
            Destroy(gameObject);
    }

    [SerializeField] private UnityEvent onDetonate;
    [SerializeField] private MagicType type;
    private bool firstNotify;
    public void NotifyToInstanciate() {
        if(!firstNotify)
            MagicInstanciatingController.Instance.InstantiatNewMagic(type);
        firstNotify = true;
    }

    //[SerializeField] private HapticAsset animationLeftHand;

   // [SerializeField] private HapticAsset animationRightHand;

    [SerializeField] public TsHapticAnimationAsset animationLeftHandAssetNew;
    [SerializeField] public TsHapticAnimationAsset animationRightHandAssetNew;

}
