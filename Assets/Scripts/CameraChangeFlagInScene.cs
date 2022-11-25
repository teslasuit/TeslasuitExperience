using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraChangeFlagInScene : MonoBehaviour {
    public string sceneName = "Room02-2";
    public CameraClearFlags flag= CameraClearFlags.Skybox;
    void Start () {
        if(SceneManager.GetActiveScene().name==sceneName)
	        GetComponent<Camera>().clearFlags = flag;
	}
	
}
