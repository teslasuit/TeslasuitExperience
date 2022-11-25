using System;
using UnityEngine;
using System.Collections;
using Photon;
using UnityEngine.SceneManagement;

namespace ExitGames.SportShooting
{
    public class Connection : PunBehaviour {
        public static event Action OnLeftRoomAction;
        public static event Action OnDisconnectAction;
        //[SerializeField] private string roomName;
        public void Init()
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.INITIALIZING);
            PhotonNetwork.autoJoinLobby = false;
            //PhotonNetwork.Set
        }

        public void Connect()
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.CONNECTING_TO_SERVER);
            if (PhotonNetwork.connected)
            {
                OnConnectedToMaster();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings(NetworkController.NETCODE_VERSION);
            }
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        #region PUN Callbacks
        public override void OnConnectedToMaster()
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.JOINING_ROOM);
            if (PhotonNetwork.inRoom)
            {
                OnJoinedRoom();
            }
            else
            {
                NetworkController.Instance.ChangeNetworkState(NetworkState.JOINING_ROOM);
                PhotonNetwork.JoinOrCreateRoom(SceneManager.GetActiveScene().name, new RoomOptions() { MaxPlayers = NetworkController.MAX_PLAYERS }, null);
            }
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.CREATING_ROOM);
            PhotonNetwork.CreateRoom(SceneManager.GetActiveScene().name, new RoomOptions() { MaxPlayers = NetworkController.MAX_PLAYERS }, null);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("[Connection] OnJoinedRoom call");
            NetworkController.Instance.ChangeNetworkState(NetworkState.ROOM_JOINED);
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("[Connection] OnCreatedRoom call");
            NetworkController.Instance.ChangeNetworkState(NetworkState.ROOM_CREATED);            
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            Debug.Log("[Connection] OnPhotonPlayerConnected call");
            NetworkController.Instance.ChangeNetworkState(NetworkState.SOME_PLAYER_CONNECTED, newPlayer);
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            NetworkController.Instance.ChangeNetworkState(NetworkState.SOME_PLAYER_DISCONNECTED, otherPlayer);
        }
        
        public override void OnDisconnectedFromPhoton() {
            if (OnDisconnectAction != null) OnDisconnectAction();
            NetworkController.Instance.ChangeNetworkState(NetworkState.DISCONNECTED);
        }

        public override void OnLeftRoom() {
            if (OnLeftRoomAction!=null) OnLeftRoomAction();
        }
        #endregion
    }
}
