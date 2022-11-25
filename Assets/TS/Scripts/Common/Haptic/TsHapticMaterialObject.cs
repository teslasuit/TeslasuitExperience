using TsSDK;
using UnityEngine;

public class TsHapticMaterialObject : MonoBehaviour
{
    [SerializeField]
    private TsHapticMaterialAsset m_hapticMaterial;


    private void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var collisionHandler = collision.gameObject.GetComponent<TsHapticCollisionHandler>();

        if(collisionHandler != null)
        {
            collisionHandler.HapticPlayer.Play(m_hapticMaterial.Instance);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        var collisionHandler = collision.gameObject.GetComponent<TsHapticCollisionHandler>();

        if (collisionHandler != null)
        {
            collisionHandler.HapticPlayer.Stop(m_hapticMaterial.Instance);
        }
    }

}
