using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ExitGames.SportShooting
{
    public class GameModel : MonoBehaviour
    {

        #region Player Properties
        [SerializeField]
        PlayerFactory _playerFactory;
        #endregion

        #region Gameplay Properties
        
        public Dictionary<int, int> ScoreBoard { get; set; }

        public Dictionary<int, int> PlayersPositions { get; set; }

        public PhotonPlayer WinningPlayer
        {
            get
            {
                PhotonPlayer winningPlayer = PhotonNetwork.player;

                foreach(PhotonPlayer player in PhotonNetwork.playerList)
                {
                    if((int)player.CustomProperties["roundScore"] > (int)winningPlayer.CustomProperties["roundScore"])
                    {
                        winningPlayer = player;
                    }
                }

                return winningPlayer;
            }
        }

        BaseGameState _activeGameState; 
        public BaseGameState ActiveGameState { get { return _activeGameState; }  }

        public Player CurrentPlayer;

        public static GameModel Instance { get; set; }
        #endregion

        [SerializeField] private bool tir;
        void Awake()
        {
            Instance = this;
            CurrentPlayer = null;
            if (tir) {
                NetworkController.OnRoomJoined += BuildPlayer;
            }
        }

        void Update()
        {
            // Execute current game state each frame
            if(_activeGameState != null)
            {
                _activeGameState.ExecuteState();
            }
        }

        public void ChangeGameState(BaseGameState newState)
        {
            if(_activeGameState != null)
            {
                _activeGameState.FinishState();
            }
            _activeGameState = newState;
            _activeGameState.InitState();
        }

        public void CountScoreToPlayer(int playerID, int scorePrice)
        {
            foreach(PhotonPlayer player in PhotonNetwork.playerList)
            {
                if(player.ID == playerID)
                {
                    var updatedProperties = player.CustomProperties;
                    updatedProperties["roundScore"] = (int)updatedProperties["roundScore"] + scorePrice;
                    player.SetCustomProperties(updatedProperties);
                }
            }

            NetworkController.Instance.NotifyTrapHit(playerID);
            NetworkController.Instance.NotifyToUpdateScore();
        }

        public void ResetRoundData()
        {
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                var updatedProperties = player.CustomProperties;
                updatedProperties["roundScore"] = 0;
                player.SetCustomProperties(updatedProperties);                
            }
            
        }
        

        public void BuildPlayer()
        {
            _playerFactory.Build();
            if(tir) NetworkController.OnRoomJoined -= BuildPlayer;
        }

    }
}
