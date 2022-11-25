using System.Collections;
using System.Collections.Generic;
using Antilatency;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AltOculusHMDTracking))]
public class AltOculusHMDTrackingEditor : Editor {

    public override void OnInspectorGUI() {
        var oculusTracking = (AltOculusHMDTracking) target;
        if (oculusTracking != null) {
            if (!oculusTracking.RigCheck(false)) {
                EditorGUILayout.HelpBox("This component must be placed on OVRCameraRig gameobject", MessageType.Error);
            }
            if (!oculusTracking.CheckEnvironment(false)) {
                EditorGUILayout.HelpBox("AltEnvironment component must be placed on one of the parent gameobjects", MessageType.Error);
            }
        }

        base.OnInspectorGUI();
    }
}
