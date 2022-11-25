using System.Collections;
using System.Collections.Generic;
using Antilatency;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AltDirectHMDTracking))]
public class AltDirectHMDTrackingEditor : Editor {
    public override void OnInspectorGUI() {
        var directHmdTracking = (AltDirectHMDTracking)target;
        if (directHmdTracking != null) {
            if (!directHmdTracking.CheckEnvironment(false)) {
                EditorGUILayout.HelpBox("AltEnvironment component must be placed on one of the parent gameobjects", MessageType.Error);
            }
        }

        base.OnInspectorGUI();
    }
}
