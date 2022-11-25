using System.Collections;
using TsSDK;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class IntroTeleportController : MonoBehaviour {

    [SerializeField] private Transform EffectToScale;
    [SerializeField] private Vector3 TargetScale=new Vector3(5f,5f,5f);
    [SerializeField] private float timeForScale=2f;
    [SerializeField] private UnityEvent onStartEffect;
    [SerializeField] private float timeForChangeScene=2.5f;
    private bool started;

    [SerializeField] public TsHapticAnimationAsset animationAssetNew;


    private void Awake() {
        if(animationAssetNew==null)Debug.LogWarning("[IntroTeleportController] animationAsset on "+gameObject.name+" is empty");
    }

    private void OnTriggerEnter(Collider collider) {
        //Debug.Log("Intro "+collider.name);
        if (!started&&collider.CompareTag("Player")) {
            switch (collider.name[0]) {
                case 'R':
                    //TODO Start animation from R hand
                    break;
                case 'L':
                    //TODO Start animation from L hand
                    break;
                case 'C':
                    //TODO Start animation from Body
                    break;
            }
            Debug.Log("Intro start");
            onStartEffect.Invoke();
            //if (GameManager.Instance && GameManager.Instance.CurrentSuitApiObject != null&&GameManager.Instance.CurrentSuitApiObject.IsAvailable) { 
            //    animId = animationAsset;
            //    GameManager.Instance.CurrentSuitApiObject.Haptic.SourceMapping.Play(animationAsset);
            //}else {
            //    Debug.LogError("[IntroTeleportController] handle is null");
            //}
            var player = GameManager.Instance.GetCurrentHapticPlayer();
            if (player != null)
            {
                
                currentPlayable = GameManager.Instance.GetPlayable(animationAssetNew.Instance);
                player.Play(currentPlayable);
            }else {
                Debug.LogError("[IntroTeleportController] handle is null");
            }

            StartCoroutine(scalingCor());
            StartCoroutine(changeScene());
            started = true;
        }
    }

    private IHapticPlayable currentPlayable = null;
    private IEnumerator scalingCor() {
        float startTime = Time.time;
        Vector3 startValue = EffectToScale.localScale;
        while (Time.time-startTime<timeForScale) {
            EffectToScale.localScale = Vector3.Lerp(startValue, TargetScale, (Time.time - startTime) / timeForScale);
            yield return null;
        }
        //if (GameManager.Instance && GameManager.Instance.CurrentSuitApiObject != null && animId != null) { 
        //    GameManager.Instance.CurrentSuitApiObject.Haptic.SourceMapping.Stop(animId);
        //    animId = null;
        //}

        var player = GameManager.Instance.GetCurrentHapticPlayer();
        if (player != null && currentPlayable != null)
        {
            currentPlayable.Stop();
            currentPlayable = null;
        }

        if (XRSettings.loadedDeviceName == "Oculus") {
            var fade = FindObjectOfType<OVRScreenFade>();
            if(fade)fade.FadeOut();
            
        }

        StartCoroutine(fadeAudio());
    }

    private IEnumerator changeScene() {
        yield return new WaitForSeconds(timeForChangeScene);
        if (async != null)
            async.allowSceneActivation = true;
        else {
            SceneManager.LoadScene("NewEnvironment");
        }

    }

    public void StartAsyncLoad()
    {
        if (async == null) {
            Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
            async=SceneManager.LoadSceneAsync("NewEnvironment");
            async.allowSceneActivation = false;
        }
        
    }


    private AsyncOperation async=null;
    [SerializeField] private AudioSource[] sources;

    private IEnumerator fadeAudio() {
        float startTime = Time.time;
        while (Time.time-startTime<0.75f) {
            for (int audio = 0; audio < sources.Length; audio++) {
                sources[audio].volume = Mathf.Lerp(sources[audio].volume, 0f, Time.deltaTime);
            }

            yield return null;
        }
        for (int audio = 0; audio < sources.Length; audio++) {
            sources[audio].volume = 0f;
        }
    }

}
