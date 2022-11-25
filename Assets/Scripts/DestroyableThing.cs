using UnityEngine;
using UnityEngine.Events;

public class DestroyableThing : MonoBehaviour,IDestroyable {
    [SerializeField] private GameObject destrPrefab;
    [SerializeField] private UnityEvent OnDestroy;
    public void TakeDamage(int amount, Vector3 hitPoint) {
        if(destroyed)
            return;
        gameObject.SetActive(false);
        Instantiate(destrPrefab,transform.position,transform.rotation).SetActive(true);
        OnDestroy.Invoke();
        Destroy(gameObject);
        destroyed = true;
    }

    private bool destroyed;
    private void OnCollisionEnter(Collision collision) {
        TakeDamage((int)collision.impulse.magnitude,collision.contacts[0].point);
    }
}
