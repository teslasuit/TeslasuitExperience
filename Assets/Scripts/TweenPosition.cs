using UnityEngine;

public class TweenPosition : MonoBehaviour {

    [SerializeField] private TweenType type = TweenType.simple;
    [SerializeField] private Vector3 startPosition=new Vector3();
    [SerializeField] private Vector3 endPosition=new Vector3();
    [SerializeField] private AnimationCurve curve=new AnimationCurve(new Keyframe(0f,0f),new Keyframe(1f,1f));
    [SerializeField] private float delayTime;
    [SerializeField] private float time;
    private float innerTime;
    private void OnEnable() {
        transform.localPosition = startPosition;
        innerTime = 0;
    }

    private void OnDisable() {

    }

    public void ResetToBeginning() {
        transform.localPosition = startPosition;
        innerTime = 0;
        forwardPlay = true;
        enabled = true;
    }

    private bool forwardPlay=true;
    public void PlayReverse() {
        transform.localPosition = endPosition;
        innerTime = 0;
        forwardPlay = false;
        enabled = true;
    }

    private void Update() {
        innerTime += Time.deltaTime;
        if(innerTime<delayTime)
            return;
        if (forwardPlay)
            transform.localPosition =
                Vector3.Lerp(startPosition, endPosition, curve.Evaluate((innerTime - delayTime) / time));
        else {
            transform.localPosition =
                Vector3.Lerp(startPosition, endPosition, curve.Evaluate(1f-(innerTime - delayTime) / time));
        }
        if (innerTime - delayTime > time) {
            switch (type)
            {
                case TweenType.simple:
                    if(forwardPlay)
                        transform.localPosition = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(1f));
                    else
                        transform.localPosition = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(0f));
                    enabled = false;
                    break;
                case TweenType.loop:
                    innerTime -= time;
                    break;
            }
        }
    }
    void Reset()
    {
        startPosition = transform.localPosition;
        endPosition = transform.localPosition;
    }
}
