using System.Threading.Tasks;
using TeslasuitAPI;
using TsSDK;
using UnityEngine;

public class HapticExplosion : MonoBehaviour {

    public LayerMask layer;
    //public HapticMaterialAsset material;
    public TsHapticMaterialAsset materialNew;
    public float explosionDuration = 1.0f;
    public float explosionRadius = 10.0f;

  

    private Collider[] colliders = new Collider[32];

    public void Run()
    {
        Debug.Log("[HapticExplosion]Run call");
        int count = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, colliders, layer.value);
        if (count>0) {
            for (int i=0; i<count; i++) 
            {
                Vector3 toThisVector = colliders[i].bounds.center - transform.position;
                if (Physics.Raycast(new Ray(transform.position, toThisVector), out var hit, explosionRadius, layer.value))
                {
                    var handler = hit.transform.GetComponent<TsHapticCollisionHandler>();
                    if (handler && materialNew && handler.HapticPlayer)
                    {
                        if (handler.HapticPlayer.PlayerHandle.GetPlayable(materialNew.Instance) is
                            IHapticMaterialPlayable materialPlayable)
                        {
                            handler.AddImpact(materialPlayable, 1.0f, 500);
                        }
                    }
                }

            }
        }

    }
}
