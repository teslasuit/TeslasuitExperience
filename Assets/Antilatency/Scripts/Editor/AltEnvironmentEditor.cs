using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AltEnvironment))]
public class AltEnvironmentEditor : Editor {
    private SerializedProperty _drawBars;
    private SerializedProperty _drawContours;
    private SerializedProperty _enableContourOffset;
    private SerializedProperty _contourOffset;

    private void OnEnable() {
        _drawBars = serializedObject.FindProperty("DrawBars");
        _drawContours = serializedObject.FindProperty("DrawContours");
        _enableContourOffset = serializedObject.FindProperty("EnableContourOffset");
        _contourOffset = serializedObject.FindProperty("ContourOffset");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_drawBars);
        EditorGUILayout.PropertyField(_drawContours);
        EditorGUILayout.PropertyField(_enableContourOffset);
        if (_enableContourOffset.boolValue) {
            EditorGUILayout.PropertyField(_contourOffset);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
