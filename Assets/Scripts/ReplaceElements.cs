using EasyButtons;
using UnityEngine;

public class ReplaceElements : MonoBehaviour {
    [SerializeField] private Transform[] elementsForReplace;
    [SerializeField] private GameObject prefab;
    [Button]
    private void replace() {
        for (int i = 0; i < elementsForReplace.Length; i++) {
            var obj=Instantiate(prefab, elementsForReplace[i].parent);
            obj.transform.position = elementsForReplace[i].position;
            obj.transform.rotation = elementsForReplace[i].rotation;
            DestroyImmediate(elementsForReplace[i].gameObject);
        }
    }
}
