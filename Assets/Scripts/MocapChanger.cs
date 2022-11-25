using UnityEngine;

public class MocapChanger : MonoBehaviour {
    [SerializeField] private bool onEnableSubscribe;

    private void OnEnable() {
        if (onEnableSubscribe) {
            InitAndSubscribe();
        }
    }
    private void OnDisable() {
        if (subscribed)
            GameManager.Instance.OnMocapChange -= ChangeMocap;
    }

    private bool subscribed;

    public void InitAndSubscribe() {
        ChangeMocap(GameManager.Instance.IKMocap);
        if(!subscribed) GameManager.Instance.OnMocapChange += ChangeMocap;
        subscribed = true;
    }

    private void ChangeMocap(bool ik) {
        if (ik) {
            if (IkMocap) IkMocap.SetActive(true);
            if (TeslaMocap) TeslaMocap.SetActive(false);
            foreach (var syncRotation in syncers) {
                syncRotation.enabled = false;
            }
        } else {
            if (IkMocap) IkMocap.SetActive(false);
            if (TeslaMocap) TeslaMocap.SetActive(true);
            foreach (var syncRotation in syncers) {
                syncRotation.enabled = true;
            }
        }
    }
    [SerializeField] private GameObject IkMocap;
    [SerializeField] private GameObject TeslaMocap;
    [SerializeField] private SyncRotation[] syncers;
}
