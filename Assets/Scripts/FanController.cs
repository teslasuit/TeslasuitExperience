using System.Collections.Generic;
using EasyButtons;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using TeslasuitAPI;
using TsSDK;

public class FanController : MonoBehaviour {
    [SerializeField] private Transform center;
    [SerializeField] private Transform Radius;
    [SerializeField] private Transform[] rays;
    [SerializeField] private int Npoints = 30;
    [SerializeField] private float lenth = 7f;
    public LayerMask layersToHit;
    [Range(0f,1f)]
    public float currentPower = 0.5f;

    [SerializeField] private bool debugOneFrame;
    
    public float FunPower {
        get { return currentPower; }
        set {
            currentPower = value;
            recalculateParticles();
            recalculateVolume();
            recalculateLines();
        }
    }

	void Update () {
#if UNITY_EDITOR
        //only for changing in editor mode by slider
        recalculateParticles();
	    recalculateVolume();
	    recalculateLines();
#endif
        if (vane) {
	        vane.localRotation*=Quaternion.Euler(new Vector3(0f,0f,maxspeed*currentPower*Time.deltaTime));
        }
        if (isAnybodyAtZone) {
            FanHit();
            debugOneFrame = false;
        }
	}
    [Header("Particles settings")]
    [SerializeField] private ParticleSystem particles;

    [SerializeField] private float minRate =0f;
    [SerializeField] private float maxRate = 1000f;
    [SerializeField] private float minStartSpeed = 0f;
    [SerializeField] private float maxStartSpeed = 28.5f;

    [SerializeField] private float hitDurationSeconds = 0.2f;
    [SerializeField] private float hitRadius = 10f;

    private void recalculateParticles() {
        if (particles) {
            var emis = particles.emission;
            var rate=emis.rateOverTime;
            rate.constant = Mathf.Lerp(minRate,maxRate,currentPower);
            emis.rateOverTime = rate;
            var main=particles.main;
            var speed=main.startSpeed;
            speed.constant = Mathf.Lerp(minStartSpeed, maxStartSpeed, currentPower);
            main.startSpeed = speed;
        }
        
    }

    [Header("Audio settings")]
    [SerializeField] private AudioSource fanSource;

    [SerializeField] private AnimationCurve pitchCurve;
    [SerializeField] private AnimationCurve volumeCurve;
    private void recalculateVolume() {
        if (fanSource) {
            fanSource.volume = volumeCurve.Evaluate(currentPower);
            fanSource.pitch = pitchCurve.Evaluate(currentPower);
        }
    }


    [Header("Tesla settings")]
    [SerializeField] public TsHapticMaterialAsset hapticAssetNew;

    [Header("Rotation settings")]
    [SerializeField] private Transform vane;

    [SerializeField] private float maxspeed=80f;


    [Header("Lines settings")]
    [SerializeField] private Cloth[] lines;

    

    [SerializeField] private Vector3 externalAcceleration = new Vector3(-50f,0f,0f);
    [SerializeField] private Vector3 randomAcceleration=new Vector3(10f,10f,10f);
    private void recalculateLines() {
        for (int i = 0; i < lines.Length; i++) {
            lines[i].externalAcceleration = externalAcceleration*currentPower;
            lines[i].randomAcceleration = randomAcceleration * currentPower;
        }
    }
   
    public float totalPower = 0.0f;

    private void Hit(Transform point)
    {
        RaycastHit hit;
        var ray = new Ray(point.position, point.forward);
        if (Physics.Raycast(ray, out hit, lenth, layersToHit))
        {
            //if (hit.transform.CompareTag("Player"))
            { //tag on every body collider

                var durationCoef = 1f - hit.distance / lenth;
                var radiusCoef = 1f - Vector3.Distance(center.position, point.position) /
                                 Vector3.Distance(center.position, Radius.position);
                float windForce = Mathf.Sqrt(currentPower * radiusCoef * durationCoef);
                if(debugOneFrame) Debug.Log("[FanController] Debug: " + hit.transform.name + "   " + windForce);

                totalPower = windForce;

                var handler = hit.transform.GetComponent<TsHapticCollisionHandler>();
                if (handler && hapticAssetNew && handler.HapticPlayer)
                {
                    if (handler.HapticPlayer.PlayerHandle.GetPlayable(hapticAssetNew.Instance) is
                        IHapticMaterialPlayable materialPlayable)
                    {
                        float _0_1_force = windForce;
                        float force = Mathf.Clamp(_0_1_force, 0, 1.0f);


                        if(force > 0.01f)
                        {
                            handler.AddImpact(materialPlayable, force, 100);
                        }

#if UNITY_EDITOR
                        if (!hit.transform.name.EndsWith("+"))hit.transform.name += "+";
                        //Debug.Log("raycast ok"+haptHit.raycastHit.collider.transform.parent.name+"/"+haptHit.raycastHit.collider.transform.name+ haptHit.raycastHit.collider.GetInstanceID());
#endif
                    }
                } else {
                    Debug.LogWarning("[FanController]Do not find HapticSkinnedMesh or HapticMaterialAsset is null");
                }
            }
        }
    }

    private void FanHit()
    {
        for (int i = 0; i < rays.Length; i++)
        {
            Hit(rays[i]);
        }

    }

    private bool isAnybodyAtZone;
    public void InZone() {
        isAnybodyAtZone = true;
    }

    public void OutOfZone() {
        isAnybodyAtZone = false;
    }


#region EDITOR tools

    [Button]
    private void generatePoints() {
        var rad = Vector3.Distance(center.position, Radius.position);
        var h = Mathf.Sqrt(Mathf.PI * rad * rad / Npoints);
        Debug.Log(rad+"   "+h);
        var k = rad / h + 1;
        List<Transform> List= new List<Transform>();
        for (int i = 0; i < k; i++) {
            for (int j = 0; j < k; j++) {
                if (i == 0 && j == 0) {
                    var go = new GameObject();
                    go.transform.parent = center;
                    go.transform.localPosition=Vector3.zero;
                    go.transform.localRotation=Quaternion.identity;
                    go.name = i+" "+j;
                    List.Add(go.transform);
                } else if(i == 0){
                    var go = new GameObject();
                    go.transform.parent = center;
                    go.transform.localPosition = new Vector3(0f,j*h,0f);
                    go.transform.localRotation = Quaternion.identity;
                    go.name = i + " " + j;
                    if (Vector3.Distance(center.position, go.transform.position) <= rad) {
                        List.Add(go.transform);
                    } else {
                        DestroyImmediate(go);
                    }
                    go = new GameObject();
                    go.transform.parent = center;
                    go.transform.localPosition = new Vector3(0f, -j * h, 0f);
                    go.transform.localRotation = Quaternion.identity;
                    go.name = i + " " + -j;
                    if (Vector3.Distance(center.position, go.transform.position) <= rad){
                        List.Add(go.transform);
                    }else{
                        DestroyImmediate(go);
                    }
                } else if (j == 0) {
                    var go = new GameObject();
                    go.transform.parent = center;
                    go.transform.localPosition = new Vector3(i * h, 0f, 0f);
                    go.transform.localRotation = Quaternion.identity;
                    go.name = i + " " + j;
                    if (Vector3.Distance(center.position, go.transform.position) <= rad){
                        List.Add(go.transform);
                    }else{
                        DestroyImmediate(go);
                    }
                    go = new GameObject();
                    go.transform.parent = center;
                    go.transform.localPosition = new Vector3(-i * h, 0f, 0f);
                    go.transform.localRotation = Quaternion.identity;
                    go.name = -i + " " + j;
                    if (Vector3.Distance(center.position, go.transform.position) <= rad){
                        List.Add(go.transform);
                    }else{
                        DestroyImmediate(go);
                    }
                } else {
                    var go = new GameObject();
                    go.transform.parent = center;
                    go.transform.localPosition = new Vector3(i * h, j * h, 0f);
                    go.transform.localRotation = Quaternion.identity;
                    go.name = i + " " + j;
                    if (Vector3.Distance(center.position, go.transform.position) <= rad){
                        List.Add(go.transform);
                    }else{
                        DestroyImmediate(go);
                    }
                    go = new GameObject();
                    go.transform.parent = center;
                    go.transform.localPosition = new Vector3(-i * h, j * h, 0f);
                    go.transform.localRotation = Quaternion.identity;
                    go.name = -i + " " + j;
                    if (Vector3.Distance(center.position, go.transform.position) <= rad){
                        List.Add(go.transform);
                    }else{
                        DestroyImmediate(go);
                    }
                    go = new GameObject();
                    go.transform.parent = center;
                    go.transform.localPosition = new Vector3(-i * h, -j * h, 0f);
                    go.transform.localRotation = Quaternion.identity;
                    go.name = -i + " " + -j;
                    if (Vector3.Distance(center.position, go.transform.position) <= rad){
                        List.Add(go.transform);
                    }else{
                        DestroyImmediate(go);
                    }
                    go = new GameObject();
                    go.transform.parent = center;
                    go.transform.localPosition = new Vector3(i * h, -j * h, 0f);
                    go.transform.localRotation = Quaternion.identity;
                    go.name = i + " " + -j;
                    if (Vector3.Distance(center.position, go.transform.position) <= rad){
                        List.Add(go.transform);
                    }else{
                        DestroyImmediate(go);
                    }
                }
            }
        }

        for (int i = 0; i < rays.Length; i++) {
            DestroyImmediate(rays[i].gameObject);
        }
        rays = List.ToArray();
    }
    /*
    void OnDrawGizmos() {
        var rad = Vector3.Distance(center.position, Radius.position);
        foreach (var ray in rays) {
            var clr = Color.Lerp(Color.red, Color.green, Mathf.Pow(Vector3.Distance(center.position, ray.position) / rad,2));
            Debug.DrawLine(ray.position,ray.position+ray.forward*lenth,clr);
            

        }
    }*/
#endregion
}
