using System;
using System.Threading.Tasks;
using TeslasuitAPI;
using TsSDK;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class ShootOnClick : MonoBehaviour {
    [SerializeField] protected float timeBeetweenShoots = 0.18f;

    [SerializeField] public SpaunPointsPool[] SpaunPoints;
    [SerializeField] protected GameObject[] MuzzleFlash;
    [SerializeField] protected TweenPosition[] tp;
    [SerializeField] protected TweenRotation[] tweenRot;
    [SerializeField] protected float BulletVelocity = 1000f;
    [SerializeField] protected AudioSource[] AFx;
    [SerializeField] protected ParticleSystem[] PSs;
    [SerializeField] protected AudioSource failSource;
    [SerializeField] private AudioSource reloadSource;
    [Header("Raycast Params")]
    [SerializeField]
    protected bool rayCastShooting = false;
    [SerializeField] protected float maxLen = 50f;
    [SerializeField] protected float damage = 20f;
    [SerializeField] protected float forceMultiplayer = 200f;
    [SerializeField] protected LayerMask mask;

    [Serializable]
    public struct SpaunPointsPool
    {

        public Transform[] spaunPoints;

    }

    private int hapticLayer ;
    private void Start() {
        hapticLayer = LayerMask.NameToLayer("HapticReceiver");
        oculusInput = XRSettings.loadedDeviceName == "Oculus";
        _view = GetComponent<PhotonView>();
    }

    private bool oculusInput;
    protected PhotonView _view;
    [SerializeField] protected bool inHand = true;
    public bool InHand
    {
        set { inHand = value; }
        get { return inHand; }
    }

    protected int order = 0;
    protected float prevShootTime;
    public float PrevShootTime { get { return prevShootTime; } }
    public event Action Vibration;
    public event Action Shoot;

    protected void Update()
    {
        if (inHand) { 
            if (FireDown){
                spawnBullet();
            } else if (Fire > 0.6 && Time.time - prevShootTime >= timeBeetweenShoots){
                spawnBullet();
            }

        }
            

    }

    protected void vibroCall() {
        if (Vibration != null)
        {
            Vibration();
        }
    }
    protected void shootCall()
    {
        if (Shoot != null) Shoot();
    }


    public Hand currentHand;

    protected bool FireDown {
        get {
            if (oculusInput) { 
                if (currentHand)
                    return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, currentHand.hand==HumanHand.Left?OVRInput.Controller.LTouch:OVRInput.Controller.RTouch);
            }else {
                if (currentHand)
                    return currentHand.GetDownTrigger();
            }
            return false;

        }
    }

    protected float Fire {
        get {
            if (oculusInput) { 
                if (currentHand)
                    return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, currentHand.hand == HumanHand.Left ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
            }else{
                if (currentHand)
                    return currentHand.GetTrigger();
            }
            return 0f;
        }
    }

    //[SerializeField] public HapticMaterialAsset HapticMaterial;
    [SerializeField] public TsHapticMaterialAsset HapticMaterialNew;
    [SerializeField] protected GameObject BulletPrefab;
    [SerializeField] protected GameObject hitPrefab;
    [SerializeField] private bool delayedHaptic = true;
    protected void spawnBullet()
    {
        
        if (!rayCastShooting) {
            for (int i = 0; i < SpaunPoints[order].spaunPoints.Length; i++)
            {
                var po = Instantiate(BulletPrefab,SpaunPoints[order].spaunPoints[i].position,SpaunPoints[order].spaunPoints[i].rotation);
                //po.SetTransform(, , Vector3.one);
                po.gameObject.GetComponent<Rigidbody>().velocity = SpaunPoints[order].spaunPoints[i].forward * BulletVelocity;

                _view.RPC("spaunFakeBullet", PhotonTargets.Others, SpaunPoints[order].spaunPoints[i].position, SpaunPoints[order].spaunPoints[i].rotation, SpaunPoints[order].spaunPoints[i].forward);
            }

        } else{
            for (int i = 0; i < SpaunPoints[order].spaunPoints.Length; i++)
            {
                Ray ray = new Ray(SpaunPoints[order].spaunPoints[i].position, SpaunPoints[order].spaunPoints[i].forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, maxLen, mask))
                {
                    var target = hit.collider.GetComponent<IDestroyable>();
                    if (target != null) {
                        target.TakeDamage((int)damage,hit.point);
                    }
                    //Debug.Log(hit.transform.name);
                    //var obj = hit.collider.GetComponent<HapticMeshArea>();
                    if (hit.collider.gameObject.layer== hapticLayer)
                    {
                        //if(obj.attachedHapticMesh.suitIndex==SuitManager.SuitIndex.None)
                        //    obj.attachedHapticMesh.HitDelayed(ray,1f, HapticMaterial,0.25f);
                        //else
                        //obj.attachedHapticMesh.HitDelayed(ray, 1f, HapticMaterial, 0.25f);
                        //if (HapticHitRaycaster.Raycast(ray, out var hHit, maxLen)) {
                        //
                        //    Task<HapticHitInfo> task = Task.FromResult<HapticHitInfo>(new HapticHitInfo(HapticHitEvent.HitEnter, HapticConstants.MaxForce, 250, HapticMaterial));
                        //    hit.collider.gameObject.GetComponentInParent<HapticMesh>().Hit(new HapticPolygonCollision(task,hHit.channelPoly));
                        //}

                        var handler = hit.collider.transform.GetComponent<TsHapticCollisionHandler>();

                        if (handler && HapticMaterialNew && handler.HapticPlayer)
                        {
                            if (handler.HapticPlayer.PlayerHandle.GetPlayable(HapticMaterialNew.Instance) is
                                IHapticMaterialPlayable materialPlayable)
                            {
                                handler.AddImpact(materialPlayable, 1.0f, 250);
                            }
                        }
                        
                        //obj.TakeDamage(damage * damageMult, hit.point, false, damageType);

                    }
                    if (hit.collider.attachedRigidbody != null){
                        if (!hit.collider.attachedRigidbody.isKinematic){
                            hit.collider.attachedRigidbody.AddForce(SpaunPoints[order].spaunPoints[i].forward * forceMultiplayer);
                            var cube = hit.collider.GetComponent<CubeHelper>();
                            if (cube)
                                cube.ReceiveImpulse(SpaunPoints[order].spaunPoints[i].forward * forceMultiplayer);

                            //Debug.Log("add force "+hit.transform.name+hit.collider.attachedRigidbody.isKinematic);
                        }
                    }
                    if (hitPrefab){
                        var po = Instantiate(hitPrefab, hit.point, Quaternion.identity);
                        //po.SetTransform(hit.point, Quaternion.identity, Vector3.one);
                        if (_view) _view.RPC("instanciateHit", PhotonTargets.Others, hit.point);
                    }

                }

            }
        }
        
        if (Vibration != null) Vibration();

        if (Shoot != null) Shoot();
        if (tp.Length > order)
            if (tp[order])
            {
                tp[order].ResetToBeginning();
                tp[order].enabled = true;
            }
        if (tweenRot.Length > order)
            if (tweenRot[order])
            {
                tweenRot[order].ResetToBeginning();
                tweenRot[order].enabled = true;
            }
        if (AFx.Length != 0 && AFx[order])
            AFx[order].Play();
            
        if (PSs.Length != 0 && PSs[order])
            PSs[order].Play();
        if (MuzzleFlash[order])
            MuzzleFlash[order].SetActive(true);
        if (_view)
        {
            _view.RPC("playFx", PhotonTargets.Others, order);
            _view.RPC("playSoundFX", PhotonTargets.Others, order);
        }



        order += 1;
        if (order >= SpaunPoints.Length)
            order = 0;
        prevShootTime = Time.time;
        
        
    }

    






    private float prevPlay;
    private void playFailedSound()
    {
        if (failSource && Time.time - prevPlay > 0.5f)
        {
            failSource.Play();
            prevPlay = Time.time;
        }
    }

    [PunRPC]
    private void spaunFakeBullet(Vector3 pos, Quaternion rot, Vector3 dir) {
        var po = Instantiate(BulletPrefab,pos, rot);
        //po.SetTransform(, Vector3.one, true);
        po.gameObject.GetComponent<Rigidbody>().velocity = dir * BulletVelocity;
        var bullet = po.GetComponent<BulletController>();
        if (bullet){
            bullet.fake = true;
            bullet.FromPlayer = -1;
        }
    }



    [PunRPC]
    private void PlayFailedSound()
    {
        if (failSource) failSource.Play();
    }

    [PunRPC]
    private void playSoundFX(int order)
    {
        if (AFx.Length != 0 && AFx[order] && AFx[order].gameObject.activeInHierarchy)
            AFx[order].Play();
    }
    [PunRPC]
    private void instanciateHit(Vector3 hitpoint)
    {
        if (hitPrefab){
            var po = Instantiate(hitPrefab, hitpoint, Quaternion.identity);
            var expl = po.GetComponent<HapticExplosion>();
            if(expl)expl.Run();

            //po.SetTransform(hit.point, Quaternion.identity, Vector3.one);
        }
    }

    [PunRPC]
    private void playFx(int order)
    {
        if (tp[order])
        {
            tp[order].ResetToBeginning();
            tp[order].enabled = true;
        }
        if (tweenRot.Length > order)
        {
            if (tweenRot[order])
            {
                tweenRot[order].ResetToBeginning();
                tweenRot[order].enabled = true;
            }
        }
        if (MuzzleFlash[order])
            MuzzleFlash[order].SetActive(true);
    }
}
