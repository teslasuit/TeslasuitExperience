using System;
using System.Threading.Tasks;
using TeslasuitAPI;
using TsSDK;
using UnityEngine;
using UnityEngine.XR;

public class OverheatableShootOnClick : MonoBehaviour
{
    [SerializeField] protected float timeBeetweenShoots = 0.18f;

    [SerializeField] public ShootOnClick.SpaunPointsPool[] SpaunPoints;
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
    [Header("Overheatable options")]
    [SerializeField] private float tempOneShoot = 35f;
    [SerializeField] private float tempkoef = 0.2f;
    [SerializeField] private float TempForOverheat = 100f;
    [SerializeField] private instanceMaterial[] materials;
    [SerializeField] private float MaxTemp = 150f;
    [SerializeField] private float flashtime = 0.4f;
    [SerializeField] private AudioSource[] OverHeatAudioSource;
    [SerializeField] private AudioClip[] OverHeatAudioClips;
    [Serializable]
    private class instanceMaterial {
        public Renderer renderer = null;
        public int numOfMat = 0;
        [HideInInspector] public Color startColor;
    }
    private int hapticLayer;
    private void Start() {
        hapticLayer = LayerMask.NameToLayer("HapticReceiver");
        oculusInput = XRSettings.loadedDeviceName == "Oculus";
        _view = GetComponent<PhotonView>();
        heat = new float[SpaunPoints.Length];
        overHeated = new bool[SpaunPoints.Length];
        overHeatTimer = new float[SpaunPoints.Length];
        Overnorm = new bool[SpaunPoints.Length];
        for (int i = 0; i < Overnorm.Length; i++)
            Overnorm[i] = true;
        foreach (var VARIABLE in materials) {
            VARIABLE.startColor = VARIABLE.renderer.materials[VARIABLE.numOfMat].GetColor("_EmissionColor");

        }
    }
    [SerializeField] protected bool inHand = true;
    public bool InHand
    {
        set { inHand = value; }
        get { return inHand; }
    }
    private bool oculusInput;
    protected PhotonView _view;
    protected int order = 0;
    protected float prevShootTime;
    public float PrevShootTime { get { return prevShootTime; } }
    public event Action Vibration;
    public event Action Shoot;

    protected void vibroCall()
    {
        if (Vibration != null)
        {
            Vibration();
        }
    }
    protected void shootCall()
    {
        if (Shoot != null) Shoot();
    }


    private float[] heat;
    private float TimeBeetweenShootKoef = 1f;
    private bool[] overHeated;
    private float[] overHeatTimer;
    private bool[] Overnorm;
    public Hand currentHand;

    protected bool FireDown
    {
        get
        {
            if (oculusInput)
            {
                if (currentHand)
                    return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, currentHand.hand == HumanHand.Left ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
            }
            else
            {
                if (currentHand)
                    return currentHand.GetDownTrigger();
            }
            return false;

        }
    }

    protected float Fire
    {
        get
        {
            if (oculusInput)
            {
                if (currentHand)
                    return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, currentHand.hand == HumanHand.Left ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
            }
            else
            {
                if (currentHand)
                    return currentHand.GetTrigger();
            }
            return 0f;
        }
    }
    [SerializeField] public TsHapticMaterialAsset HapticMaterialNew;
    [SerializeField] protected GameObject BulletPrefab;
    [SerializeField] protected GameObject hitPrefab;
    [SerializeField] private bool delayedHaptic = true;
    new void Update() {
        if (heat[order] > TempForOverheat)
        {
            TimeBeetweenShootKoef = heat[order] / TempForOverheat + 0.3f;
        }
        else
        {
            TimeBeetweenShootKoef = 1f;
        }
        if (inHand)
        {
            if (FireDown && !overHeated[order]) {
                tempAdd();
                spawnBullet();
            }
            else if (Fire > 0.6 && Time.time - prevShootTime >= timeBeetweenShoots * TimeBeetweenShootKoef && !overHeated[order]) {
                tempAdd();
                spawnBullet();
            }

        }
        recalculateTemp();
    }

    private void tempAdd() {
        heat[order] += tempOneShoot;
        if (heat[order] > MaxTemp) {
            overHeated[order] = true; //Debug.Log(order + " OverHeated");
            if (OverHeatAudioSource.Length > order && OverHeatAudioClips.Length > 0 && OverHeatAudioSource[order] != null && OverHeatAudioClips[0] != null) {
                OverHeatAudioSource[order].clip = OverHeatAudioClips[0];
                OverHeatAudioSource[order].loop = true;
                OverHeatAudioSource[order].Play();
            }
        }
    }


    private void recalculateTemp()
    {
        for (int i = 0; i < heat.Length; i++)
        {
            heat[i] = Mathf.Lerp(heat[i], 0, Time.deltaTime * tempkoef);
            if (!overHeated[i])
            {
                if (heat[i] > TempForOverheat - 40)
                {
                    ChangeColor(i);
                    Overnorm[i] = false;
                }
                else if (!Overnorm[i])
                {
                    ChangeToStartColor(i);
                    Overnorm[i] = true;
                }
            }
            else
            {
                if (heat[i] < 40)
                {
                    overHeated[i] = false;
                    if (OverHeatAudioSource.Length > i && OverHeatAudioClips.Length > 1 && OverHeatAudioSource[i] != null &&
                        OverHeatAudioClips[1] != null)
                    {
                        OverHeatAudioSource[i].Stop();
                        OverHeatAudioSource[i].clip = OverHeatAudioClips[1];
                        OverHeatAudioSource[i].loop = false;
                        OverHeatAudioSource[i].Play();
                    }
                }
                else
                {
                    overHeatTimer[i] += Time.deltaTime;
                    if (overHeatTimer[i] < flashtime * heat[i] / MaxTemp)
                    {
                        if (heat[i] > TempForOverheat - 40)
                        {
                            ChangeColor(i);
                        }
                        else
                        {
                            ChangeToStartColor(i);
                        }
                    }
                    else if (overHeatTimer[i] < 2 * flashtime * heat[i] / MaxTemp)
                    {
                        ChangeToNoColor(i);
                    }
                    else
                    {
                        overHeatTimer[i] = 0;
                    }
                }
            }
        }
    }

    private new void spawnBullet()
    {

        if (!rayCastShooting)
        {
            for (int i = 0; i < SpaunPoints[order].spaunPoints.Length; i++)
            {
                var po = Instantiate(BulletPrefab, SpaunPoints[order].spaunPoints[i].position, SpaunPoints[order].spaunPoints[i].rotation);
                //po.SetTransform(, , Vector3.one);
                po.gameObject.GetComponent<Rigidbody>().velocity = SpaunPoints[order].spaunPoints[i].forward * BulletVelocity;

                _view.RPC("spaunFakeBullet", PhotonTargets.Others, SpaunPoints[order].spaunPoints[i].position, SpaunPoints[order].spaunPoints[i].rotation, SpaunPoints[order].spaunPoints[i].forward);
            }

        }
        else
        {
            for (int i = 0; i < SpaunPoints[order].spaunPoints.Length; i++)
            {
                Ray ray = new Ray(SpaunPoints[order].spaunPoints[i].position, SpaunPoints[order].spaunPoints[i].forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, maxLen, mask))
                {
                    var target = hit.collider.GetComponent<Targets>();
                    if (target != null)
                    {
                        target.TakeDamage((int)damage, hit.point);
                    }
                    //Debug.Log(hit.transform.name);
                    if (hit.collider.gameObject.layer == hapticLayer) 
                    {
                        var handler = hit.collider.transform.GetComponent<TsHapticCollisionHandler>();

                        if (handler && HapticMaterialNew && handler.HapticPlayer)
                        {
                            if (handler.HapticPlayer.PlayerHandle.GetPlayable(HapticMaterialNew.Instance) is
                                IHapticMaterialPlayable materialPlayable)
                            {
                                handler.AddImpact(materialPlayable, 1.0f, 200);
                            }
                        }

                    }
                    if (hit.collider.attachedRigidbody != null)
                    {
                        if (!hit.collider.attachedRigidbody.isKinematic)
                        {
                            hit.collider.attachedRigidbody.AddForce(SpaunPoints[order].spaunPoints[i].forward * forceMultiplayer);
                            var cube = hit.collider.GetComponent<CubeHelper>();
                            if (cube)
                                cube.ReceiveImpulse(SpaunPoints[order].spaunPoints[i].forward * forceMultiplayer);

                            //Debug.Log("add force "+hit.transform.name+hit.collider.attachedRigidbody.isKinematic);
                        }
                    }
                    if (hitPrefab)
                    {
                        var po = Instantiate(hitPrefab, hit.point, Quaternion.identity);
                        //po.SetTransform(hit.point, Quaternion.identity, Vector3.one);
                        if (_view) _view.RPC("instanciateHit", PhotonTargets.Others, hit.point);
                    }

                }

            }
        }

        vibroCall();
        shootCall();
        
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






    private void ChangeColor(int i)
    {
        float kof = 0.75f + (heat[i] - TempForOverheat + 40) / (MaxTemp - TempForOverheat + 30) * 0.25f;
        if (kof > 1f)
            kof = 1f;
        if (kof < 0.75f)
            kof = 0.75f;
        //if (i==0)Debug.Log(kof+" "+ heat[i]);
        float hue = 120f / 360f * (1 - kof);
        var k = i;
        //while (mat.Length > k) {
        //    if(mat[k])
        //        mat[k].SetColor(Shader.PropertyToID("_EmissionColor"),Color.HSVToRGB(hue, 1, 1));
        //    k += SpaunPoints.Length;
        //}
        while (materials.Length > k)
        {
            if (materials[k].numOfMat == 0)
                materials[k].renderer.material.SetColor("_EmissionColor", Color.HSVToRGB(hue, 1, 1));
            else
            {
                var mats = materials[k].renderer.materials;
                mats[materials[k].numOfMat].SetColor("_EmissionColor", Color.HSVToRGB(hue, 1, 1));
                materials[k].renderer.materials = mats;
            }
            k += SpaunPoints.Length;
        }

    }
    private void ChangeToStartColor(int i)
    {
        var k = i;
        // while (mat.Length > k)
        // {
        //     if (mat[k])
        //         mat[k].SetColor(Shader.PropertyToID("_EmissionColor"), startColor);
        //     k += SpaunPoints.Length;
        // }
        while (materials.Length > k)
        {
            if (materials[k].numOfMat == 0)
                materials[k].renderer.material.SetColor("_EmissionColor", materials[k].startColor);
            else
            {
                var mats = materials[k].renderer.materials;
                mats[materials[k].numOfMat].SetColor("_EmissionColor", materials[k].startColor);
                materials[k].renderer.materials = mats;
            }
            k += SpaunPoints.Length;
        }
    }
    private void ChangeToNoColor(int i)
    {
        var k = i;
        //while (mat.Length > k)
        //{
        //    if (mat[k])
        //        mat[k].SetColor(Shader.PropertyToID("_EmissionColor"), Color.black);
        //    k += SpaunPoints.Length;
        //}
        while (materials.Length > k)
        {
            if (materials[k].numOfMat == 0)
                materials[k].renderer.material.SetColor("_EmissionColor", Color.black);
            else
            {
                var mats = materials[k].renderer.materials;
                mats[materials[k].numOfMat].SetColor("_EmissionColor", Color.black);
                materials[k].renderer.materials = mats;
            }
            k += SpaunPoints.Length;
        }
    }



    [PunRPC]
    private void spaunFakeBullet(Vector3 pos, Quaternion rot, Vector3 dir)
    {
        var po = Instantiate(BulletPrefab, pos, rot);
        //po.SetTransform(, Vector3.one, true);
        po.gameObject.GetComponent<Rigidbody>().velocity = dir * BulletVelocity;
        var bullet = po.GetComponent<BulletController>();
        if (bullet)
        {
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
        if (hitPrefab)
        {
            var po = Instantiate(hitPrefab, hitpoint, Quaternion.identity);
            var expl = po.GetComponent<HapticExplosion>();
            if (expl) expl.Run();

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
