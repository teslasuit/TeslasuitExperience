using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GramophoneController : MonoBehaviour {
    private PhotonView _view;
    [SerializeField] private MusicPlate[] plates;
    [SerializeField] private HapticAudioEqualizer audio;
    [Range(0f, 1f)]
    public float currentPower = 0.5f;

    [SerializeField] private JukeBoxButton[] buttons;
    public float GrammaphonePower {
        get { return currentPower; }
        set{
            currentPower = value;
            recalculateVolume();
            if(audio)audio.Volume = value;
        }
    }

    private void recalculateVolume(){
        if (source){
            source.volume = currentPower;
        }
    }

    private void Start() {
        _view = GetComponent<PhotonView>();
        rotator.TurnEmission(false);
        rotator.ResetOffset();
    }

    public void PlayerInZone() {
        //TODO start/continue haptic effect
    }

    public void PlayerOutOfZone() {
        //TODO stop haptic effect
    }


    public void StartPlay(int number) {

        if (plates.Length > number) { 
            StartPlaying(plates[number].Clip,plates[number].name,plates[number].cover,number);
            if (_view) _view.RPC("PlayRemote", PhotonTargets.Others, number);

        }
    }


    [SerializeField] private AudioSource source = null;
    private void StartPlaying(AudioClip clip,string musicname,Sprite spritecover,int k) {
        foreach (JukeBoxButton button in buttons) {
            button.ReturnToDefaultPos();
            button.SetPushed(k);
        }
        if(source.isPlaying)
            source.Stop();
        source.clip = clip;
        source.Play();
        jukeboxAnimator.enabled = true;
        rotator.enabled = true;
        rotator.TurnEmission(true);
        StartCoroutine(plaEnumerator());
        defaultText.SetActive(false);
        cover.gameObject.SetActive(true);
        cover.sprite = spritecover;
        nameText.gameObject.SetActive(true);
        nameText.text = musicname;
        rotateText = musicname.Length > kLetters;
        if (rotateText) {
            nameText.text += "     ";
            Debug.Log(nameText.text.Remove(0, 1)+"   "+ (nameText.text));
        }
    }
    
    private bool rotateText;
    private float lastRotate;
    private IEnumerator plaEnumerator() {
        yield return null;
        while (source.isPlaying) {
            yield return null;
            if (rotateText&&Time.time-lastRotate>1f/TextSpeed) {
                char ch=nameText.text[0];
                var str2= nameText.text.Remove(0, 1);
                nameText.text = str2 + ch;
                lastRotate = Time.time;
            }
        }

        jukeboxAnimator.enabled = false;
        rotator.enabled = false;
        defaultText.SetActive(true);
        cover.gameObject.SetActive(false);
        nameText.gameObject.SetActive(false);
    }





    [Header("Animation")]
    
    [SerializeField] private Animator jukeboxAnimator;

    [SerializeField] private UVRotator rotator;

    [Header("Display")]
    [SerializeField] private GameObject defaultText;

    [SerializeField] private Text nameText;
    [SerializeField] private Image cover;
    [SerializeField] private int kLetters = 20;
    [SerializeField] private float TextSpeed=5f;

    public void Stop() {
        stop();
        if(_view)_view.RPC("StopRemote", PhotonTargets.Others);
    }

    private void stop() {
        foreach (JukeBoxButton button in buttons){
            button.ReturnToDefaultPos();
        }
        if (source.isPlaying) source.Stop();
        source.clip = null;
        jukeboxAnimator.enabled = false;
        rotator.enabled = false;
        rotator.TurnEmission(false);
        StopAllCoroutines();

        defaultText.SetActive(true);
        cover.gameObject.SetActive(false);
        nameText.gameObject.SetActive(false);
    }

    [PunRPC]
    public void PlayRemote(int k) {
        if (plates.Length > k){
            StartPlaying(plates[k].Clip, plates[k].name, plates[k].cover,k);
            return;
            
        }
        Debug.LogWarning("[GramophoneController] Do not find plate");
    }

    [PunRPC]
    private void StopRemote() {
        stop();
    }

}
