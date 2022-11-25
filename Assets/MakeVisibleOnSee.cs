using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeVisibleOnSee : MonoBehaviour {
    private bool isVisible;
    [SerializeField] private Transform objectForView;
    [SerializeField] private float timeBeforeStartOpenening=1f;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float timeForOpen=1f;
    [SerializeField] private HumanHand hand=HumanHand.Left;
    public void Update() {
        switch (currentState) {
            case UiState.Closed:
                if (isVisible&&Time.time-timeBecomeVisible> timeBeforeStartOpenening) {
                    currentState = UiState.Opening;
                    if (ChartRenderer.Instance) { 
                        if (hand == HumanHand.Left) 
                            ChartRenderer.Instance.renderLeftHand = true;
                        else 
                            ChartRenderer.Instance.renderRightHand = true;
                        
                    }
                   
                }
                break;
            case UiState.Opening:
                objectForView.localScale=Vector3.LerpUnclamped(Vector3.zero, Vector3.one, curve.Evaluate((Time.time - timeBecomeVisible- timeBeforeStartOpenening)/timeForOpen));
                if (Time.time - timeBecomeVisible - timeBeforeStartOpenening > timeForOpen) {
                    objectForView.localScale = Vector3.one;
                    currentState = UiState.Opened;
                }
                break;
            case UiState.Opened:
                if (!isVisible && Time.time - timeBecomeInvisible > timeBeforeStartOpenening/2) {
                    currentState = UiState.Closing;
                }
                break;
            case UiState.Closing:
                objectForView.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one, curve.Evaluate(1f-(Time.time - timeBecomeInvisible - timeBeforeStartOpenening/2) / (timeForOpen/2)));
                if (Time.time - timeBecomeInvisible - timeBeforeStartOpenening/2 > timeForOpen/2) {
                    objectForView.localScale = Vector3.zero;
                    currentState = UiState.Closed;

                    if (ChartRenderer.Instance)
                    {
                        if (hand == HumanHand.Left)
                            ChartRenderer.Instance.renderLeftHand = false;
                        else
                            ChartRenderer.Instance.renderRightHand = false;

                    }
                }
                break;
        }
    }

    private float timeBecomeVisible,timeBecomeInvisible;
    void OnBecameVisible() {
        timeBecomeVisible = Time.time;
        isVisible = true;
    }

    private UiState currentState = UiState.Closed;
    void OnBecameInvisible() {
        timeBecomeInvisible = Time.time;
        isVisible = false;
    }
    private enum UiState {
        Closed,
        Opening,
        Opened,
        Closing
    }
}
