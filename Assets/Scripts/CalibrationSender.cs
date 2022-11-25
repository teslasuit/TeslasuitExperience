using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class CalibrationSender : MonoBehaviour {

    public void ChangeValue(bool value) {
        CalibrationMenu.Instance.ChangeValue(value);
    }

    public void RestartButton() {
        CalibrationMenu.Instance.StopCalibrationAnimation();
        //SceneManager.LoadScene("SetupUI");
        teleport.MakeTeleport();
        CalibrationMenu.Instance.CloseUI();
    }

    [SerializeField] private TeleportOnWall teleport;
}
