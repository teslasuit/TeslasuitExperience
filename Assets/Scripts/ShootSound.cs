using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(ShootOnClick))]
public class ShootSound : MonoBehaviour {
    [SerializeField] private AudioSource[] sources={};
    [SerializeField] private UnityEvent onShoot = null;
    // ReSharper disable once InconsistentNaming
    private void Awake() {
        var shoot= GetComponent<ShootOnClick>();
        if(shoot)shoot.Shoot += shootHandle;
        var overh= GetComponent<OverheatableShootOnClick>();
        if(overh)overh.Shoot += shootHandle;
    }

    private int order = 0;
    private void shootHandle() {
        if (sources.Length > order) {
            if(sources[order])sources[order].Play();
            
        }
        order += 1;
        if (order >= sources.Length)
            order = 0;
        onShoot.Invoke();
    }
}
