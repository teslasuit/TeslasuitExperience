using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class PlayingGameState : BaseGameState
    {


        public override void InitState()
        {
            base.InitState();

            NetworkController.OnSomePlayerConnected += SomeoneConnectedMessage;
            NetworkController.OnSomePlayerDisconnected += SomeoneDisconnectedMessage;
            NetworkController.OnPlayerTrapHit += ShowTrapHitMessage;
        }

        public override void FinishState()
        {
            base.FinishState();

            NetworkController.OnSomePlayerConnected -= SomeoneConnectedMessage;
            NetworkController.OnSomePlayerDisconnected -= SomeoneDisconnectedMessage;
            NetworkController.OnPlayerTrapHit -= ShowTrapHitMessage;
        }

        public override void ExecuteState()
        {
            base.ExecuteState();

            if (!PhotonNetwork.isMasterClient)
            {
                return;
            }            
            
        }
        
        void SomeoneConnectedMessage(PhotonPlayer somePlayer)
        {
            GameView.Instance.ShowGamePopupPanel("NEW PLAYER", "Player ID: " + somePlayer.ID + " connected");
        }

        void SomeoneDisconnectedMessage(PhotonPlayer somePlayer)
        {
            GameView.Instance.ShowGamePopupPanel("PLAYER DISCONNECTED", "Player ID: " + somePlayer.ID + " disconnected");
        }

        void ShowTrapHitMessage(int playerID)
        {
            PhotonPlayer[] players = PhotonNetwork.playerList;
            string playerName = playerID.ToString();

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].ID == playerID)
                {
                    playerName = players[i].CustomProperties["name"].ToString();
                    break;
                }
            }

            GameView.Instance.ShowStatusPopupPanel("Player: '" + playerName + "' have hit the trap");
        }
    }
}
