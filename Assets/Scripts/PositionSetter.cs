using System.Collections;
using EasyButtons;
using UnityEngine;
using UnityEngine.XR;
using VRTK;

public class PositionSetter : MonoBehaviour {

    private VRTK_InteractableObject vrtkobject;
    [SerializeField] private bool setPosition;
    [Header("Oculus Position")]
    [SerializeField] private Vector3 localPosition;
    [SerializeField] private Vector3 localRotation;

    [SerializeField] private Vector3 localRightPosition;
    [SerializeField] private Vector3 localRightRotation;
    [Header("Vive Position")]
    [SerializeField] private Vector3 localVivePosition;
    [SerializeField] private Vector3 localViveRotation;


    [SerializeField] private bool setHand;
    [SerializeField] private TriggerRot trigger;
    [SerializeField] private VibrationHandler vibro;

    [SerializeField] private bool ChangeToNotHoldOnVive;

    [Header("Two Hand Options")]
    [SerializeField] private bool twoHand;
    [SerializeField] private LookAtTarget lookAtTarget;
    [SerializeField] private RotateInOpposite rotationInOpposite;
    [SerializeField] private TwoHand twoHandRot;


    private void OnEnable(){
        vrtkobject = GetComponent<VRTK_InteractableObject>();
        if (vrtkobject)
        {
            vrtkobject.InteractableObjectGrabbed += HandleGrab;
            vrtkobject.InteractableObjectUngrabbed += HandleUnGrab;
        }

        click = GetComponent<ShootOnClick>();
        overhClick = GetComponent<OverheatableShootOnClick>();
        if (XRSettings.loadedDeviceName=="OpenVR"&&ChangeToNotHoldOnVive) {
            if (vrtkobject) vrtkobject.holdButtonToGrab = false;
        }
    }


    private void OnDisable(){
        if (vrtkobject){
            vrtkobject.InteractableObjectGrabbed -= HandleGrab;
            vrtkobject.InteractableObjectUngrabbed -= HandleUnGrab;
        }
    }

    private ShootOnClick click;
    private OverheatableShootOnClick overhClick;
    private void HandleGrab(object sender, InteractableObjectEventArgs e){
        if (setPosition) {
            StartCoroutine(setLocalTransform());
        }

        if (setHand) {
            if (click) {
                click.InHand = true;
                
            }
            if (overhClick) {
                overhClick.InHand = true;

            }

            if (trigger||click|| vibro||overhClick) StartCoroutine(setHandToShootOnClick());
            if (trigger)
                trigger.enabled = true;
            

        }

        if (twoHand) {
            StartCoroutine(setTwoHandScripts());
        }
    }
    private void HandleUnGrab(object sender, InteractableObjectEventArgs e){
        if (setHand)
        {
            if (click) {
                click.InHand = false;
                click.currentHand = null;
            }
            if (overhClick)
            {
                overhClick.InHand = false;
                overhClick.currentHand = null;
            }
            if (trigger) {
                trigger.HandDevice = null;
                trigger.enabled = false;
            }

            if (vibro) vibro.Device = null;
            if (twoHand) {
                if (lookAtTarget) lookAtTarget.enabled = false;
                if (rotationInOpposite) rotationInOpposite.enabled = false;
                if (twoHandRot) twoHandRot.enabled = false;
            }
        }

        if (debugRb) {
            StartCoroutine(debugVelocity());
        }
    }

    private IEnumerator debugVelocity() {
        var rb=GetComponent<Rigidbody>();
        while (true) {
            Debug.Log(rb.velocity);
            yield return null;
        }
    }
    [SerializeField] private bool debugRb;
    private IEnumerator setTwoHandScripts() {
        yield return null;
        var hand = GetComponentInParent<Hand>();
        if (lookAtTarget) {

            lookAtTarget.target = hand.AnotherHandTransform;
            lookAtTarget.enabled = true;
        }

        if (twoHandRot) {
            twoHandRot.holdControl= hand.AnotherHandTransform.gameObject;
            twoHandRot.rHand = hand.gameObject;
            twoHandRot.enabled = true;
        }
        if (rotationInOpposite) {

            rotationInOpposite.Target = hand.transform;
            rotationInOpposite.enabled = true;
        }
    }


    private IEnumerator setLocalTransform(){
        yield return null;
        switch (XRSettings.loadedDeviceName) {
            case "Oculus":
                switch (GetComponentInParent<Hand>().hand){
                case HumanHand.Left:
                    transform.localPosition = localPosition;
                    transform.localEulerAngles = localRotation;
                    break;
                case HumanHand.Right:
                    transform.localPosition = localRightPosition;
                    transform.localEulerAngles = localRightRotation;
                    break;
                }
                break;
            case "OpenVR":
                transform.localPosition = localVivePosition;
                transform.localEulerAngles = localViveRotation;
                break;

        }
        
    }

    private IEnumerator setHandToShootOnClick() {
        yield return null;
        var hand= GetComponentInParent<Hand>();
        if(click) click.currentHand = hand;
        if(overhClick) overhClick.currentHand = hand;
        if(trigger)trigger.HandDevice = hand;
        if (vibro) vibro.Device = hand;

    }
    [Button]
    private void saveHandPositionAndRotation() {

        switch (XRSettings.loadedDeviceName)
        {
            case "Oculus":
                switch (GetComponentInParent<Hand>().hand){
                    case HumanHand.Left:
                        localPosition = transform.localPosition;
                        localRotation = transform.localEulerAngles;
                        break;
                    case HumanHand.Right:
                        localRightPosition = transform.localPosition;
                        localRightRotation = transform.localEulerAngles;
                        break;
                }
                break;
            case "OpenVR":
                localVivePosition=transform.localPosition;
                localViveRotation=transform.localEulerAngles;
                break;

        }

        
    }
}
