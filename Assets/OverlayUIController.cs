
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OverlayUIController : MonoBehaviour
{
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        groupRectTransform.localScale=Vector3.zero;
        buttonRect.localEulerAngles = new Vector3(0f, 0f, 180f);
        status = false;
    }
    [SerializeField] private RectTransform groupRectTransform;
    [SerializeField] private RectTransform buttonRect;

    public static OverlayUIController Instance=null;
    private bool status;
    public void UpDawnArrowClick() {
        StopAllCoroutines();
        StartCoroutine(scale(status? Vector3.zero:Vector3.one));
        buttonRect.localEulerAngles=new Vector3(0f,0f, status ? 180f : 0f);
        status = !status;
    }

    private IEnumerator scale(Vector3 target,float time=0.25f) {
        float startTime=Time.time;
        Vector3 startScale=groupRectTransform.localScale;
        while (Time.time-startTime< time) {
            groupRectTransform.localScale = Vector3.Lerp(startScale, target, (Time.time - startTime) / time);
            yield return null;
        }

        groupRectTransform.localScale = target;
    }

    [SerializeField] private Text nameText;
    [SerializeField] private Text connectionText;

    private void Update() {
        if (status) {
            if (GameManager.Instance == null || GameManager.Instance.CurrentSuitBehaviour == null){
                connectionText.text = "Connection status: No connection";
                return;
            }


            if (GameManager.Instance.CurrentSuitBehaviour.Device != null && nameText) nameText.text = "Suit name: "+GameManager.Instance.CurrentSuitBehaviour.Device.Ssid;

            if (GameManager.Instance.CurrentSuitBehaviour.Device != null && connectionText) {
                connectionText.text = "Connection status: Good connection";
                connectionText.color = Color.green;
            }
        }
    }

    public void RestartButtonClick() {
        UpDawnArrowClick();
        ExitFromRoom.ExitToSetupScene();
    }
}
