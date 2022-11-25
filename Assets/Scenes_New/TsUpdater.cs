using System.Collections;
using System.Collections.Generic;
using System.IO;
using TeslasuitAPI;
using TsAPI.Types;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR


[ExecuteInEditMode]
public class TsUpdater : MonoBehaviour
{
    [SerializeField] 
    private bool m_run = false;
    // Update is called once per frame
    void Update()
    {
        if (m_run)
        {
            Run();
            m_run = false;
        }
    }

    private void Run()
    {
        //UpdateMagicController();
        //UpdateVibrationHandler();
        //UpdateOverheatableShootOnClick();
        //UpdateHapticTriggerObject();
        //UpdateFanController();
        //UpdateFogCollisionHaptic();
        //UpdateShootOnClick();
        //UpdateIntroTeleportController();
        //UpdateHapticUIController();
        //CheckComponentsHasTypeProperty<HapticAnimationAsset>();
        //CheckComponentsHasTypeProperty<HapticMaterialAsset>();
        CheckComponentsHasNullProperty<TsHapticAnimationAsset>();
        CheckComponentsHasNullProperty<TsHapticMaterialAsset>();
        //CheckComponentsHasntBothTypeProperty<HapticAnimationAsset, TsHapticAnimationAsset>();
        //CheckComponentsHasntBothTypeProperty<HapticMaterialAsset, TsHapticMaterialAsset>();
    }

    private void CheckComponentsHasTypeProperty<T>()
    {
        var allComponents = GetAllComponentsOnlyInScene<MonoBehaviour>();

        foreach (var component in allComponents)
        {
            var so = new SerializedObject(component);
            var it = so.GetIterator();
            if (it.Next(true))
            {
                do
                {
                    var type = it.type;
                    if (type.Contains(typeof(T).Name))
                    {   
                        Debug.Log($"{component.name} | {component.GetType()} has property with type: {typeof(T).ToString()} | Property name: {it.name}", component);
                    }
                } while (it.Next(false));
            }
        }
    }

    private void CheckComponentsHasNullProperty<T>()
    {
        var allComponents = GetAllComponentsOnlyInScene<MonoBehaviour>();

        foreach (var component in allComponents)
        {
            var so = new SerializedObject(component);
            var it = so.GetIterator();
            if (it.Next(true))
            {
                do
                {
                    var type = it.type;
                    if (type.Contains(typeof(T).Name) && it.objectReferenceValue == null)
                    {   
                        Debug.Log($"{component.name} | {component.GetType()} has NULL property with type: {typeof(T).ToString()} | Property name: {it.name}", component);
                    }
                } while (it.Next(false));
            }
        }
    }

    private void CheckComponentsHasntBothTypeProperty<T1, T2>()
    {
        var allComponents = GetAllComponentsOnlyInScene<MonoBehaviour>();

        foreach (var component in allComponents)
        {
            var so = new SerializedObject(component);
            var it = so.GetIterator();
            bool hasT1 = false;
            bool hasT2 = false;
            if (it.Next(true))
            {
                do
                {
                    var type = it.type;
                    if (type.Contains(typeof(T1).Name))
                    {
                        hasT1 = true;
                    }

                    if (type.Contains(typeof(T2).Name))
                    {
                        hasT2 = true;
                    }
                } while (it.Next(false));
            }

            if (hasT1 && !hasT2)
            {
                Debug.Log($"{component.name} | {component.GetType()} has property with type: {typeof(T1)} But no property found with the type {typeof(T2)}", component);
            }
        }
    }
    /*
    private void UpdateIntroTeleportController()
    {
        var components = GetAllComponentsOnlyInScene<IntroTeleportController>();

        foreach (var comp in components)
        {
            var animationName = UpdateName(comp.animationAsset.name);
            if (comp.animationAssetNew == null)
            {
                comp.animationAssetNew = GetAnimationAsset(animationName);
            }
        }
    }

    private void UpdateShootOnClick()
    {
        var components = GetAllComponentsOnlyInScene<ShootOnClick>();

        foreach (var comp in components)
        {
            var assetName = UpdateName(comp.HapticMaterial.name);
            var materialAsset = GetMaterialAsset(assetName);
            comp.HapticMaterialNew = materialAsset;
        }
    }

    private void UpdateFogCollisionHaptic()
    {
        var components = GetAllComponentsOnlyInScene<FogCollisionHaptic>();

        foreach (var comp in components)
        {
            var assetName = UpdateName(comp.material.name);
            var materialAsset = GetMaterialAsset(assetName);
            comp.materialNew = materialAsset;
        }
    }

    private void UpdateHapticTriggerObject()
    {
        var components = GetAllComponentsOnlyInScene<HapticTriggerObject>();

        foreach (var comp in components)
        {
            var assetName = UpdateName(comp.material.name);
            var materialAsset = GetMaterialAsset(assetName);
            comp.materialNew = materialAsset;
        }
    }

    private void UpdateFanController()
    {
        var components = GetAllComponentsOnlyInScene<FanController>();

        foreach (var comp in components)
        {
            var assetName = UpdateName(comp.hapticAsset.name);
            var materialAsset = GetMaterialAsset(assetName);
            comp.hapticAssetNew = materialAsset;
        }
    }

    private void UpdateMagicController()
    {
        var components = GetAllComponentsOnlyInScene<MagicController>();

        foreach (var comp in components)
        {
            var nameLeft = UpdateName(comp.animationLeftHandAsset.name);
            var nameRight = UpdateName(comp.animationRightHandAsset.name);

            if (comp.animationLeftHandAssetNew == null)
            {
                comp.animationLeftHandAssetNew = GetAnimationAsset(nameLeft);
            }

            if (comp.animationRightHandAssetNew == null)
            {
                comp.animationRightHandAssetNew = GetAnimationAsset(nameRight);
            }
        }
    }

    private void UpdateHapticUIController()
    {
        var components = GetAllComponentsOnlyInScene<HapticUiController>();

        foreach (var comp in components)
        {
            foreach (var clip in comp.clips)
            {
                var assetName = UpdateName(clip.animationAsset.name);
                if (clip.animationAssetNew == null)
                {
                    clip.animationAssetNew = GetAnimationAsset(assetName);
                }
            }
        }
    }

    private void UpdateOverheatableShootOnClick()
    {
        var components = GetAllComponentsOnlyInScene<OverheatableShootOnClick>();

        foreach (var comp in components)
        {
            var assetName = UpdateName(comp.HapticMaterial.name);
            var materialAsset = GetMaterialAsset(assetName);
            comp.HapticMaterialNew = materialAsset;
        }
    }

    private TsHapticEffectAsset GetEffectAsset(string name)
    {
        var fullPathEffect = "HapticAnimations_New/Effects/" + name + ".asset";
        var asset = AssetDatabase.LoadAssetAtPath<TsHapticEffectManualAsset>("Assets/" +fullPathEffect);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<TsHapticEffectManualAsset>();
            var curves = new TsHapticEffectManualAsset.ParamCurve[3];
            curves[0].paramType = TsHapticParamType.Amplitude;
            curves[0].curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
            curves[1].paramType = TsHapticParamType.PulseWidth;
            curves[1].curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
            curves[2].paramType = TsHapticParamType.Period;
            curves[2].curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
            asset.curves = curves;

            
            CreateAssetInFolder(asset, fullPathEffect);
            AssetDatabase.SaveAssets();
        }

        return asset;
    }



    public void CreateAssetInFolder(Object newAsset, string path)
    {
        var dirInfo = new DirectoryInfo(Path.GetDirectoryName($"{Application.dataPath}/{path})"));
       
        if (!dirInfo.Exists)
        {
            dirInfo.Create();
        }

        AssetDatabase.CreateAsset(newAsset, $"Assets/{path}");
    }

    private TsTouchSequenceAsset GetSampleAsset(string name)
    {
        var fullPathSample = "HapticAnimations_New/Samples/" + name + ".asset";
        var asset = AssetDatabase.LoadAssetAtPath<TsTouchSequenceManualAsset>("Assets/" + fullPathSample);

        if (asset == null)
        {
            TsTouchPlacement[] touchPlacements = new TsTouchPlacement[1];
            touchPlacements[0].parameters = new TsHapticParam[3];
            touchPlacements[0].parameters[0].type = TsHapticParamType.Amplitude;
            touchPlacements[0].parameters[0].value = 50;
            touchPlacements[0].parameters[1].type = TsHapticParamType.PulseWidth;
            touchPlacements[0].parameters[1].value = 50;
            touchPlacements[0].parameters[2].type = TsHapticParamType.Period;
            touchPlacements[0].parameters[2].value = 1000000 / 150;
            touchPlacements[0].durationMs = 100;
            touchPlacements[0].paramsSize = (uint)touchPlacements[0].parameters.Length;
            asset = ScriptableObject.CreateInstance<TsTouchSequenceManualAsset>();
            asset.touchPlacements  = touchPlacements;
            CreateAssetInFolder(asset, fullPathSample);
            AssetDatabase.SaveAssets();
        }

        return asset;
    }

    private TsHapticMaterialAsset GetMaterialAsset(string name)
    {
        var fullPathMaterial = "HapticAnimations_New/Materials/" + name + ".asset";

        var material = AssetDatabase.LoadAssetAtPath<TsHapticMaterialAsset>("Assets/" +fullPathMaterial);
        if (material == null)
        {
            var effect = GetEffectAsset(name);
            var sample = GetSampleAsset(name);
            material = ScriptableObject.CreateInstance<TsHapticMaterialAsset>();
            SerializedObject so = new SerializedObject(material);
            so.FindProperty("m_hapticEffect").objectReferenceValue = effect;
            so.FindProperty("m_touchSequence").objectReferenceValue = sample;
            so.ApplyModifiedProperties();
            CreateAssetInFolder(material, fullPathMaterial);
            AssetDatabase.SaveAssets();
        }

        return material;
    }

    private TsHapticAnimationAsset GetAnimationAsset(string name)
    {
        return AssetDatabase.LoadAssetAtPath<TsHapticAnimationAsset>("Assets/HapticAnimations_New/" + name + ".ts_asset");
    }

    private void UpdateVibrationHandler()
    {
        var components = GetAllComponentsOnlyInScene<VibrationHandler>();

        foreach (var comp in components)
        {
            var nameLeft = UpdateName(comp.animationLeftAsset.name);
            var nameRight = UpdateName(comp.animationRightAsset.name);

            if (comp.animationLeftAssetNew == null)
            {
                comp.animationLeftAssetNew = GetAnimationAsset(nameLeft);
            }

            if (comp.animationRightAssetNew == null)
            {
                comp.animationRightAssetNew = GetAnimationAsset(nameRight);
            }
        }
    }
    */
    private string UpdateName(string src)
    {
        if (src[0] == '_')
        {
            return src.Substring(1);
        }

        return src;
    }

    List<T> GetAllComponentsOnlyInScene<T>() where T : MonoBehaviour
    {
        List<T> objectsInScene = new List<T>();

        foreach (T comp in Resources.FindObjectsOfTypeAll(typeof(T)) as T[])
        {
            if (!EditorUtility.IsPersistent(comp.transform.root.gameObject) && !(comp.hideFlags == HideFlags.NotEditable || comp.hideFlags == HideFlags.HideAndDontSave))
                objectsInScene.Add(comp);
        }

        return objectsInScene;
    }
}
#endif