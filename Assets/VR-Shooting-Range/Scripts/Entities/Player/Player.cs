using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;
using TsSDK;
using UnityEngine.Events;
using UnityEngine.XR;
using SuitIndex = TsSDK.SuitIndex;
using System;

namespace ExitGames.SportShooting
{
    /// <summary>
    /// Component's container for Player
    /// </summary>
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform _uiRoot;
        
        
        public Transform UIRoot{
            get{
                return _uiRoot;
            }
        }

        [SerializeField] private Transform _sideUIRoot;
        public Transform SideUIRoot {
            get {
                return _sideUIRoot;
            }
        }

        [SerializeField] GameObject _cameraRig;
        public GameObject CameraRig {
            get {
                return _cameraRig;
            }
        }

        [SerializeField] private UnityEvent onLocalPlayer;
#if UNITY_EDITOR
        [SerializeField] private UnityEvent onEditorPlayer;
#endif



        private void OnDestroy() {
            Debug.Log("[Player] OnDestroy call");
        }

        public void BuildForGame()
        {
            Debug.Log("[Player] BuildForGame call");
            _cameraRig.SetActive(true);



            if (_uiRoot)
                _uiRoot.gameObject.SetActive(true);
            if (_sideUIRoot)
                _sideUIRoot.gameObject.SetActive(true);
            
            if (GameManager.Instance == null) {
                new GameObject().AddComponent<GameManager>();
            }

            //GameManager.Instance.localPlayerSceleton = sceleton;
            //GameManager.Instance.LocalHapticMesh = haptic;
            GameManager.Instance.CalibrationAnimateScale = CalibrationUiAnimate;
            GameManager.Instance.LocalIk = vrik;
            GameManager.Instance.CurrentSuitBehaviour = GetComponent<TsSuitBehaviour>();
            GameManager.Instance.LocalCameraTransform = XRSettings.loadedDeviceName == "OpenVR" ? viveCamera : ovrCamera;
            onLocalPlayer.Invoke();
#if UNITY_EDITOR
            onEditorPlayer.Invoke();
#endif

            if (GameManager.Instance && GameManager.Instance.CurrentSuitBehaviour != null)
            {
                GetComponent<TsSuitBehaviour>().TargetSuitIndex = SuitIndex.Suit0;//TODO
                GameManager.Instance.CurrentSuitBehaviour.ConnectionStateChanged += CurrentSuitBehaviour_ConnectionStateChanged;
            }

        }

        private void CurrentSuitBehaviour_ConnectionStateChanged(TsDeviceBehaviour beh, bool connected)
        {
            try
            {
                if (connected)
                {
                    var playerName = beh.Device.Ssid;

                    Client.Photon.Hashtable playerInfo = new Client.Photon.Hashtable();
                    playerInfo.Add("name", playerName);
                    PhotonNetwork.player.SetCustomProperties(playerInfo);
                }
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }

        //[SerializeField] private HapticMesh haptic;
        [SerializeField] private AnimateScale CalibrationUiAnimate;
        [SerializeField] private VRIK vrik;
        [SerializeField] private Transform ovrCamera;
        [SerializeField] private Transform viveCamera;

    }
}
