using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GrenadeController : MonoBehaviour {


    [SerializeField] private GameObject explosion;
    [SerializeField] private MeshRenderer mesh;
    [SerializeField]
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    private Color onColor = Color.white;
    [SerializeField] private float timeDelay = 3f;
    [SerializeField] private AudioSource source;
    [SerializeField] private UnityEvent OnExplosion;
    private float timer;
    private Color color;
    private Material mat;
    private bool detonate;
    public event Action GrenadeDetonate;
    public bool IsDetonate
    {
        get { return detonate; }
    }
    private PhotonView photonView;
    private void Awake() {
        photonView = GetComponent<PhotonView>();
    }
    public void Detonate()
    {
        if (detonate)
            return;
        detonate = true;
        mat = mesh.material;
        if (mat) color = mat.GetColor("_EmissionColor");
        timer = Time.time;

        StartCoroutine(StartDetonate());
        photonView.RPC("doActivation", PhotonTargets.AllViaServer);
    }

    [SerializeField] private UnityEvent onRemoteActivation;
    [PunRPC]
    private void doActivation() {
        if(detonate)
            return;
        detonate = true;
        mat = mesh.material;
        if (mat) color = mat.GetColor("_EmissionColor");
        timer = Time.time;
        onRemoteActivation.Invoke();
        StartCoroutine(StartDetonate());
    }



    private IEnumerator StartDetonate()
    {
        var del = timeDelay / 15f;
        //var del2= timeDelay / 10f;
        var i = 0;
        while (Time.time - timer < timeDelay)
        {
            var f = Time.time;
            while (Time.time - f < del)
                yield return null;
            ChangeColor(true);
            if (i == 3) //{
                del *= 0.75f;
            //    del2 *= 2f;
            //}
            f = Time.time;
            while (Time.time - f < del)
                yield return null;

            ChangeColor(false);
            if (i == 5)
                del *= 0.5f;
            if (source) source.Play();
            i++;
        }
        
        //if (explosion)
        //    Instantiate(explosion, transform.position, transform.rotation);
        if (mat)
            mat.SetColor("_EmissionColor", color);
        
        //Destroy(gameObject);
        photonView.RPC("doDetonate", PhotonTargets.AllViaServer);
    }

    private void ChangeColor(bool i)
    {

        if (i)
        {
            //Debug.Log("On");
            if (mesh) mesh.material.SetColor("_EmissionColor", onColor);
        }
        else
        {
            //Debug.Log("Off");
            if (mesh) mesh.material.SetColor("_EmissionColor", color);
        }

    }

    private bool detonated;
    [PunRPC]
    private void doDetonate() {
        detonated = true;
        Debug.Log("Boom!");
        OnExplosion.Invoke();
        if (GrenadeDetonate != null) GrenadeDetonate();
        var go = Instantiate(explosion, transform.position, transform.rotation);
        go.SetActive(true);
        
        if (photonView.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
            //NotifyToInstanciate();
        }
    }

}
