using System;
using UnityEngine;

public class TweenScale : MonoBehaviour {
    [SerializeField] private TweenType type;
    [SerializeField] private Vector3 startScale = new Vector3();
    [SerializeField] private Vector3 endScale = new Vector3();
    [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    [SerializeField] private float delayTime;
    [SerializeField] private float time;
    private float innerTime;
    private void OnEnable()
    {
        transform.localScale = startScale;
        innerTime = 0;
    }

    private void OnDisable()
    {

    }

    public void ResetToBeginning()
    {
        transform.localScale = startScale;
        innerTime = 0;
        enabled = true;
    }

    private void Update()
    {
        innerTime += Time.deltaTime;
        if (innerTime < delayTime)
            return;
        transform.localScale =
            Vector3.Lerp(startScale, endScale, curve.Evaluate((innerTime - delayTime) / time));
        if (innerTime - delayTime > time)
        {
            switch (type) {
                case TweenType.simple:
                    transform.localScale = Vector3.Lerp(startScale, endScale, curve.Evaluate(1f));
                    enabled = false;
                    break;
                case TweenType.loop:
                    innerTime -= time;
                    break;
            }
            
        }
    }

    void Reset() {
        startScale = transform.localScale;
        endScale = transform.localScale;
    }
}

public enum TweenType {
    simple,
    loop
}