using ExitGames.SportShooting;
using TeslasuitAPI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class ExitFromRoom : MonoBehaviour {

    public void Exit() {
        ExitToSetupScene();
        
    }

    public static void ExitToSetupScene() {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("SetupUI");
        if (ChartRenderer.Instance)
            ChartRenderer.Instance.StopListening();
    }

    public void ToTirRoom() {
        Debug.Log("[ExitFromRoom] ToTirRoom call");
        Connection.OnDisconnectAction += onLeave;
        PhotonNetwork.Disconnect();
        if (ChartRenderer.Instance)
            ChartRenderer.Instance.StopListening();

    }

    private void onLeave() {
        SceneManager.LoadScene("RoomTir");
        Connection.OnDisconnectAction -= onLeave;
    }

    public void ToMainRoomFromIntro() {
        SceneManager.LoadScene("NewEnvironment");
    }



    public void ToMainRoom() {
        Debug.Log("[ExitFromRoom] ToMainRoom call");
        Connection.OnDisconnectAction += onLeaveTir;
        PhotonNetwork.Disconnect();
        if (ChartRenderer.Instance)
            ChartRenderer.Instance.StopListening();
    }

    private void onLeaveTir() {
        SceneManager.LoadScene("NewEnvironment");
        Connection.OnDisconnectAction -= onLeaveTir;

    }

    public void ExitGame() {
        StopGame();
    }

    public static void StopGame() {
        Connection.OnDisconnectAction += Connection_OnDisconnectAction;
        
        PhotonNetwork.Disconnect();
        if (ChartRenderer.Instance)
            ChartRenderer.Instance.StopListening();
        try {
            Teslasuit.Unload();
        } catch {

        }
        
        
    }

    private static void Connection_OnDisconnectAction() {
        Connection.OnDisconnectAction -= Connection_OnDisconnectAction;
        Application.Quit();
    }
}
