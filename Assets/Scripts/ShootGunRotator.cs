using UnityEngine;

public class ShootGunRotator : MonoBehaviour {

    [SerializeField] private ShootOnClick soc=null;
    [SerializeField] private OverheatableShootOnClick ovsoc=null;
    [SerializeField] private float speedLerp = 5f;
    [SerializeField] private float speedLerpAdd = 20f;
    [SerializeField] private float addAngel = 5f;
    [SerializeField] private float time = 0.05f;
    private Quaternion adder;
    private float timer;
    private void OnEnable()
    {
        if (soc)
            soc.Shoot += AddDegrees;
        if (ovsoc)
            ovsoc.Shoot += AddDegrees;
    }

    private void OnDisable()
    {
        if (soc)
            soc.Shoot -= AddDegrees;
        if (ovsoc)
            ovsoc.Shoot -= AddDegrees;
    }

    private void Update()
    {
        if (Time.time - timer < time)
            transform.localRotation = Quaternion.Slerp(transform.localRotation, adder, Mathf.Clamp01(Time.deltaTime * speedLerpAdd));
        else
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Mathf.Clamp01(Time.deltaTime * speedLerp));
    }

    private void AddDegrees()
    {
        timer = Time.time;
        adder = transform.localRotation * Quaternion.Euler(-addAngel, 0f, 0f);
    }
}
