using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    [SerializeField] private float damage = 15;
    [SerializeField] private GameObject[] disableOnCollision;
    [SerializeField] private float delay = 3f;
    [SerializeField] private GameObject hit;
    public int FromPlayer = -1;
    public bool fake = false;
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.isTrigger)
            return;
        var po = Instantiate(hit,transform.position, transform.rotation);
        
        if (fake)
            return;

        StartCoroutine(pop());
    }

    private IEnumerator pop()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        foreach (var el in disableOnCollision)
            el.SetActive(false);
        var collider = GetComponent<Collider>();
        collider.enabled = false;
        yield return new WaitForSeconds(delay);
        foreach (var el in disableOnCollision)
            el.SetActive(true);
        Destroy(gameObject);
    }
}
