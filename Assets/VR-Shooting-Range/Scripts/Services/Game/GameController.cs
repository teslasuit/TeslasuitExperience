using UnityEngine;
using System.Collections;

namespace ExitGames.SportShooting
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        void Start() {
            StartMultiplayerGame();
        }
        

        public void StartMultiplayerGame()
        {
            if (PhotonNetwork.connected) {
                GameModel.Instance.ChangeGameState(new InitializingGameState());
            }
            else
                GameModel.Instance.ChangeGameState(new ConnectingGameState());
            GameView.Instance.ShowNetworkPanel();
            NetworkController.Instance.StartMultiplayerGame();
        }
    }
}
