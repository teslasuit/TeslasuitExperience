using UnityEngine;
using UnityEngine.VR;

namespace ExitGames.SportShooting
{
    public class PlayerFactory : MonoBehaviour
    {

        [SerializeField]
        GameObject _ovrPlayerPrefab;
        
        [SerializeField]
        Transform _playerSpawnPoints;

        private GameObject _playerPrefab;

        private void Awake()
        {
                       
            _playerPrefab = _ovrPlayerPrefab;
            
        }

        public Transform PlayerSpawnPoints
        {
            get
            {
                return _playerSpawnPoints;
            }
        }

        public void Build()
        {
                BuildPlayerForGame();
        }

        public void BuildPlayerForGame()
        {
            if (GameModel.Instance.CurrentPlayer != null)
            {
                GameObject.DestroyImmediate(GameModel.Instance.CurrentPlayer.gameObject);
            }

            int positionIndex = (int)PhotonNetwork.player.CustomProperties["position"];
            Vector3 spawnPoint = PlayerSpawnPoints.GetChild(positionIndex).position;
            
            

            GameObject go = PhotonNetwork.Instantiate(_playerPrefab.name, spawnPoint, PlayerSpawnPoints.GetChild(positionIndex).rotation, 0) as GameObject;
            GameModel.Instance.CurrentPlayer = go.GetComponent<Player>();
            Debug.Log("[PlayerFactory] BuildPlayerForGame " + go.name);
            //Initialize UI
            //GameModel.Instance.CurrentPlayer.CameraRig.SetActive(true);
            GameModel.Instance.CurrentPlayer.BuildForGame();
            GameView.Instance.UIRoot = GameModel.Instance.CurrentPlayer.UIRoot;
            GameView.Instance.SideUIRoot = GameModel.Instance.CurrentPlayer.SideUIRoot;
            
        }
        

        public static Color GetColor(int position)
        {
            switch (position)
            {
                case 0: return Color.red;
                case 1: return Color.blue;
                case 2: return Color.yellow;
                case 3: return Color.green;
                case 4: return Color.black;
                default: return Color.grey;
            }
        }
    }
}
