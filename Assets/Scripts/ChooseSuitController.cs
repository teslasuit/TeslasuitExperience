using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac.Core;
using TsSDK;
using UnityEngine;
using ISuit = TsSDK.ISuit;

public class ChooseSuitController : MonoBehaviour {

    //[SerializeField] private Transform content;
    //[SerializeField] private GameObject uiElementPrefab;
    [SerializeField] private List<ChooseSuitUiElement> uiElements;
    [SerializeField] private UiSyncPosition sync;
    [SerializeField] private UiButton[] colliderButtons;
    private void Start() {
        //uiElementPrefab.gameObject.SetActive(false);
        UpdateInfo(null);
        TsManager.Root.SuitManager.OnSuitConnected += UpdateInfo;
        TsManager.Root.SuitManager.OnSuitDisconnected += UpdateInfo;
        TsManager.Root.SuitManager.OnSuitDisconnected += OnDisconnected;

    }

    private void OnDisable() {
        TsManager.Root.SuitManager.OnSuitConnected -= UpdateInfo;
        TsManager.Root.SuitManager.OnSuitDisconnected -= UpdateInfo;
        TsManager.Root.SuitManager.OnSuitDisconnected -= OnDisconnected;
    }

    private void UpdateInfo(ISuit handle) {
        Debug.Log("[ChooseSuitController] UpdateInfo call. Handles:"+ TsManager.Root.SuitManager.Suits.Count());

        var suits = TsManager.Root.SuitManager.Suits.ToArray();
        for (var index = 0; index < suits.Length; index++) {
            uiElements[index].Initialize(suits[index]);
            colliderButtons[index].onClick.RemoveAllListeners();
            colliderButtons[index].onClick.AddListener(uiElements[index].Connect);
        }

        for (int i = suits.Length; i < uiElements.Count; i++) {
            uiElements[i].Disable();
        }
        sync.maxEntries = suits.Length;
        
    }

    private void OnDisconnected(ISuit handle)
    {
        foreach (var element in uiElements)
        {
            if (element.Handle == handle)
            {
                element.Deinit(handle);
            }
        }
    }
}
