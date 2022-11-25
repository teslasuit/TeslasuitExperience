using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class IntroCallToAction : MonoBehaviour {
    [Header("intro")]
    [SerializeField] private float introWait=3f;
    [SerializeField] private AudioSource introSource;
    [SerializeField] private UnityEvent onFinishIntro;
    [Header("1 step")]
    [SerializeField] private AudioSource Step1Source;
    [SerializeField] private UnityEvent onFinish1;
    [Header("2 step")]
    [SerializeField] private UnityEvent onFinish2;
    [Header("3 step")]
    [SerializeField] private AudioSource Step3Source;
    [SerializeField] private UnityEvent onStartEvent3;
    [SerializeField] private UnityEvent onFinish3;
    [SerializeField] private UnityEvent onFinish4;
    [Header("5 step")]
    [SerializeField] private AudioSource Step5Source;
    [SerializeField] private UnityEvent onStartEvent5;
    [SerializeField] private UnityEvent onFinish5;
    [Header("6 step")]
    [SerializeField] private AudioSource Step6Source;
    [SerializeField] private UnityEvent onStartEvent6;
    [SerializeField] private UnityEvent onFinish6;
    [Header("7 step")]
    [SerializeField] private AudioSource Step7Source;
    [SerializeField] private UnityEvent onStartEvent7;
    [SerializeField] private UnityEvent onFinish7;
    [Header("8 step")]
    [SerializeField] private AudioSource Step8Source;
    [SerializeField] private UnityEvent onStartEvent8;
    [SerializeField] private UnityEvent onFinish8;
    [Header("9 step")]
    [SerializeField] private AudioSource Step9Source;
    [SerializeField] private UnityEvent onStartEvent9;
    [SerializeField] private UnityEvent onFinish9;
    [Header("10 step")]
    [SerializeField] private AudioSource Step10Source;
    [SerializeField] private UnityEvent onStartEvent10;
    [SerializeField] private UnityEvent onFinish10;

    private void Start() {
        //GameManager.Instance.OnConnect += OnFinish1Step;
        StartCoroutine(introCor());
    }

    private IEnumerator introCor() {
        yield return new WaitForSeconds(introWait);
        introSource.Play();
        yield return null;
        
        while (introSource.isPlaying) {
            yield return null;
        }
        onFinishIntro.Invoke();
        yield return new WaitForSeconds(0.5f);
        Step1Source.Play();
        intro = true;
    }




    private bool intro;
    private bool step1;
    private bool step2;
    private bool step3;
    private bool step4;
    private bool step5;
    private bool step6;
    private bool step7;
    private bool step8;
    private bool step9;
    private bool step10;
    public void OnFinish1Step() {
        //Click on stick
        if (!step1&& intro) {
            onFinish1.Invoke();
            step1 = true;
        }
        
    }
    public void OnFinish2Step() {
        //teleport
        if (!step2) {
            step1 = true;
            onFinish2.Invoke();
            step2 = true;
            StartCoroutine(waitCor(Step1Source, () => {
                onStartEvent3.Invoke();
                StartCoroutine(waitCor(0.5f, () => { Step3Source.Play(); }));
            }));
        }

        
    }

    private IEnumerator waitCor(AudioSource source, Action action) {
        while (source.isPlaying) {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        action.Invoke();
    }

    private IEnumerator waitCor(float waitTime, Action action) {
        yield return new WaitForSeconds(waitTime);
        action.Invoke();
    }



    public void OnFinish3Step() {
        if (!step3) {
            onFinish3.Invoke();
            step3 = true;
        }
    }
    public void OnFinish4Step() {
        if (!step4) {
            onFinish4.Invoke();
            step4 = true;
            StartCoroutine(waitCor(Step3Source, () => { onStartEvent5.Invoke(); }));

        }
    }
    public void OnFinish5Step() {
        if (!step5) {
            onFinish5.Invoke();
            step5 = true;
            StartCoroutine(waitCor(Step5Source, () => { onStartEvent6.Invoke(); }));
        }
        
    }

    public void OnFinish6Step() {
        if (!step6) {
            onFinish6.Invoke();
            step6 = true;
            StartCoroutine(waitCor(Step6Source, () => { onStartEvent7.Invoke(); }));

        }
    }
    public void OnFinish7Step() {
        if (!step7) {
            onFinish7.Invoke();
            step7 = true;
            StartCoroutine(waitCor(Step7Source, () => {onStartEvent8.Invoke(); }));
        }
        

    }
    public void OnFinish8Step()
    {
        if (!step8)
        {
            onFinish8.Invoke();
            step8 = true;
            StartCoroutine(waitCor(Step8Source, () => {onStartEvent9.Invoke();}));
        }
        

    }
    public void OnFinish9Step() {
        if (!step9){
            onFinish9.Invoke();
            step9 = true;
        
            StartCoroutine(waitCor(Step9Source, () => {
                onStartEvent10.Invoke();
            }));

        }

    }
    public void OnFinish10Step() {
        if (!step10)
        {
            onFinish10.Invoke();
            step10 = true;
        
            StartCoroutine(waitCor(Step10Source, () => {
                //onStartEvent11.Invoke();
            }));

        }

    }
}
