using System;
using System.Collections;
using UnityEngine;


public class TutorialViveControllerHelper : TutorialMotionControllerHelper {
    [ColorUsage(true, true, 0f, 8f, 0.125f, 3f)]
    [SerializeField] private Color color;
    [SerializeField] private float timeLoop=1.5f;
    private MeshRenderer triggeRenderer, lGripRend, rGripRend, trackpad, button;
    private bool initialized;
    private int emisId = Shader.PropertyToID("_EmissionColor");
    private void init() {
        var model = GetComponent<SteamVR_RenderModel>();
        try
        {
            triggeRenderer = model.FindComponent("trigger").GetComponent<MeshRenderer>();
            lGripRend = model.FindComponent("lgrip").GetComponent<MeshRenderer>();
            rGripRend = model.FindComponent("rgrip").GetComponent<MeshRenderer>();
            trackpad = model.FindComponent("trackpad").GetComponent<MeshRenderer>();
            button = model.FindComponent("button").GetComponent<MeshRenderer>();
            initialized = true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message, this);
            initialized = true;
        }

    }

    private EmissionState prevState;
    public override void SetState(EmissionState state)
    {
        if (!isActiveAndEnabled) return;
        if (!initialized)
            init();
        TurnOffPrevState();
        switch (state) {
            case EmissionState.Default:
                break;
            case EmissionState.BlinkStick:
                StartCoroutine(EmisTrigger(new[] {trackpad}));
                break;
            case EmissionState.Stick:
                trackpad?.material.EnableKeyword("_EMISSION");
                trackpad?.material.SetColor(emisId, color);
                break;
            case EmissionState.BlinkGrab:
                StartCoroutine(EmisTrigger(new[] { lGripRend,rGripRend }));
                break;
            case EmissionState.Grab:
                lGripRend?.material.EnableKeyword("_EMISSION");
                lGripRend?.material.SetColor(emisId, color);
                rGripRend?.material.EnableKeyword("_EMISSION");
                rGripRend?.material.SetColor(emisId, color);
                break;
            case EmissionState.BlinkMenu:
                StartCoroutine(EmisTrigger(new[] { button }));
                break;
            case EmissionState.XyButtons:
                StartCoroutine(EmisTrigger(new[] { trackpad }));
                break;
            case EmissionState.Trigger:
                StartCoroutine(EmisTrigger(new[] { triggeRenderer }));
                break;
        }

        prevState = state;
    }

    private void TurnOffPrevState() {
        StopAllCoroutines();
        switch (prevState) {
            case EmissionState.Default:
                break;
            case EmissionState.BlinkStick:
            case EmissionState.Stick:
            case EmissionState.XyButtons:
                trackpad?.material.DisableKeyword("_EMISSION");
                break;
            case EmissionState.BlinkGrab:
            case EmissionState.Grab:
                lGripRend?.material.DisableKeyword("_EMISSION");
                rGripRend?.material.DisableKeyword("_EMISSION");
                break;
            case EmissionState.BlinkMenu:
                button?.material.DisableKeyword("_EMISSION");
                break;
            case EmissionState.Trigger:
                triggeRenderer?.material.DisableKeyword("_EMISSION");
                break;
        }
    }

    private IEnumerator EmisTrigger(MeshRenderer[] mrs) {
        yield return null;
        foreach (var mr in mrs) {
            mr?.material.EnableKeyword("_EMISSION");
            mr?.material.SetColor(emisId, Color.black);
        }

        float startTime = Time.time;
        while (true) {
            var k = (Time.time - startTime) - ((int) ((Time.time - startTime) / timeLoop)) * timeLoop;
            var currentColor = k < timeLoop / 2
                ? Color.Lerp(color, Color.black, (k * 2 / timeLoop))
                : Color.Lerp(Color.black, color, ((k - timeLoop / 2) * 2 / timeLoop));
            foreach (var mr in mrs) {
                mr?.material.SetColor(emisId, currentColor);
            }

            
            yield return null;
        }

    }

    



}
