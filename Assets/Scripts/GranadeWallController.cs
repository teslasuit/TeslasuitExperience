using UnityEngine;

public class GranadeWallController : MonoBehaviour {

    [SerializeField] private GameObject Wall;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        var gc = other.GetComponent<GrenadeController>();
        if (gc && gc.IsDetonate)
        {
            gc.GrenadeDetonate += HandleDetonate;
            Debug.Log(other.gameObject.name + " wall destroyed");
        }

    }

    void HandleDetonate()
    {
        if (Wall) Wall.SetActive(true);
        gameObject.SetActive(false);

    }
}
