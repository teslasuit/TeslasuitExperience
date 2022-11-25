using System.Collections;
using UnityEngine;
using VRTK;

public class GunReturner : MonoBehaviour {
    [SerializeField] private GameObject Gun = null;
    [SerializeField] private GameObject ShootGun = null;
    [SerializeField] private GameObject Guitar = null;
    [SerializeField] private GameObject object4 = null;
    [SerializeField] private AudioSource onReturn = null;
    private Vector3 gunStartPos, shootGunStartPos, guitarStartPos, object4StartPos;
    private Quaternion gunQuaternion, shootQuaternion, guitarQuaternion, object4Quaternion;
    private Coroutine gunCor, shootGunCor, guitarCor, object4Cor;
    [SerializeField] private bool debugMode = false;
    private void Awake() {
        if (Guitar) { 
            guitarStartPos = Guitar.transform.position;
            guitarQuaternion = Guitar.transform.rotation;
        }
        if (ShootGun) {
            shootGunStartPos = ShootGun.transform.position;
            shootQuaternion = ShootGun.transform.rotation;
        }
        if (Gun) {
            gunStartPos = Gun.transform.position;
            gunQuaternion = Gun.transform.rotation;
        }
        if (object4)
        {
            object4StartPos = object4.transform.position;
            object4Quaternion = object4.transform.rotation;
        }
    }

    public void ForceReturn(GameObject toReturn) {
        if (toReturn == Gun)
        {
            gunCor = null;
            returnStartPos(Gun, gunStartPos, gunQuaternion);
            if (debugMode) Debug.Log("Gun force Returning");
        }
        else if (toReturn == ShootGun)
        {
            shootGunCor = null;
            returnStartPos(ShootGun, shootGunStartPos, shootQuaternion);
            if (debugMode) Debug.Log("ShootGun force Returning");
        }
        else if (toReturn == Guitar)
        {
            guitarCor = null;
            returnStartPos(Guitar, guitarStartPos, guitarQuaternion);
            if (debugMode) Debug.Log("Guitar force Returning");
        }
        else if (toReturn == object4) {
            object4Cor = null;
            returnStartPos(object4,object4StartPos,object4Quaternion);
            if (debugMode) Debug.Log("object4 force Returning");
        }
    }
    void OnTriggerEnter(Collider other) {
        if (other.gameObject == Gun) {
            if (gunCor != null) StopCoroutine(gunCor);
            gunCor = null;
            if (debugMode) Debug.Log("gun Returning stop");
        } else if (other.gameObject == ShootGun) {
            if (shootGunCor != null) StopCoroutine(shootGunCor);
            shootGunCor = null;
            if (debugMode) Debug.Log("shootGun Returning stop");
        } else if (other.gameObject == Guitar) {
            if (guitarCor != null) StopCoroutine(guitarCor);
            guitarCor = null;
            if (debugMode) Debug.Log("Guitar Returning stop");
        } else if (other.gameObject == object4) {
            if (object4Cor != null) StopCoroutine(object4Cor);
            object4Cor = null;
            if (debugMode) Debug.Log("object4 Returning stop");
        }
    }

    void OnTriggerExit(Collider other){
        if (other.gameObject == Gun) {
            if (gunCor == null) {
                gunCor = StartCoroutine(Return(Gun, gunStartPos,gunQuaternion));
                if (debugMode) Debug.Log("Gun Returning initialized");
            } else
                if (debugMode) Debug.Log("Gun Returning is still initialized");
        } else if (other.gameObject == ShootGun) {
            if (shootGunCor == null) {
                shootGunCor = StartCoroutine(Return(ShootGun, shootGunStartPos,shootQuaternion));
                if (debugMode) Debug.Log("ShootGun Returning initialized");
            } else
                if (debugMode) Debug.Log("ShootGun Returning is still initialized");
        } else if (other.gameObject == Guitar){
            if (guitarCor == null) {
                guitarCor = StartCoroutine(Return(Guitar, guitarStartPos,guitarQuaternion));
                if (debugMode) Debug.Log("Guitar Returning initialized");
            } else
                if (debugMode) Debug.Log("GUITAR Returning is still initialized");
        }else if (other.gameObject == object4){
            if (object4Cor == null)
            {
                object4Cor = StartCoroutine(Return(object4, object4StartPos, object4Quaternion));
                if (debugMode) Debug.Log("object4 Returning initialized");
            }
            else
            if (debugMode) Debug.Log("object4 Returning is still initialized");
        }
    }

    private IEnumerator Return(GameObject gun, Vector3 pos,Quaternion rot) {
        yield return new WaitForSeconds(5f);
        var inter = Gun.GetComponent<VRTK_InteractableObject>();
        if (inter&&inter.IsGrabbed())
            inter.ForceStopInteracting();
        returnStartPos(gun, pos, rot);
        if (onReturn) {
            onReturn.transform.position = pos;
            onReturn.transform.rotation = rot;
            
            onReturn.Play();

        }
        
        if (Gun == gun) gunCor = null;
        else if (ShootGun == gun) shootGunCor = null;
        else if (Guitar == gun) guitarCor = null;
        else if (object4 == gun) object4Cor = null;
    }

    private static void returnStartPos(GameObject gun, Vector3 pos, Quaternion rot) {
        var rb = gun.GetComponent<Rigidbody>();
        if (rb) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
           
        }
        gun.transform.position = pos;
        gun.transform.rotation = rot;
        if (rb){
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

        }
    }
}
