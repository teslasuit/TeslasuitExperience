using UnityEngine;

public class TweenRotation : MonoBehaviour {

    [SerializeField] private TweenType type=TweenType.simple;
    [SerializeField] private Vector3 startRotation = new Vector3();
    [SerializeField] private Vector3 endRotation = new Vector3();
    [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    [SerializeField] private float delayTime;
    [SerializeField] private float time;
    private float innerTime;
    private void OnEnable()
    {
        transform.localEulerAngles = startRotation;
        innerTime = 0;
    }

    private void OnDisable()
    {

    }

    public void ResetToBeginning()
    {
        transform.localEulerAngles = startRotation;
        innerTime = 0;
        enabled = true;
    }

    private void Update()
    {
        innerTime += Time.deltaTime;
        if (innerTime < delayTime)
            return;
        transform.localEulerAngles =
            Vector3.Lerp(startRotation, endRotation, curve.Evaluate((innerTime - delayTime) / time));

        if (innerTime - delayTime > time){
            switch (type){
                case TweenType.simple:
                    transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, curve.Evaluate(1f));
                    enabled = false;
                    break;
                case TweenType.loop:
                    innerTime -= time;
                    break;
            }

        }
    }

    void Reset() {
        startRotation = transform.localEulerAngles;
        endRotation = transform.localEulerAngles;
    }
}
