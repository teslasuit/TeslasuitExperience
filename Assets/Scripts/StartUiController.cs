using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUiController : MonoBehaviour {
    public Button singleButton;
    public Button multButton;
    public Button localButton;
    public Button StartButton;

    public GameObject networkGroup;
    public GameObject playGroup;

    public Button TutorialButton;
    public Button MainRoomButton;
    public Button ArmyButton;
    public Button playButton;
    public InputField inputField;

    private void Awake() {
        singleButton.onClick.AddListener(singleModeClick);
        multButton.onClick.AddListener(multButtonClick);
        localButton.onClick.AddListener(localButtonClick);
        StartButton.onClick.AddListener(startButtonClick);
        currentMode = GameMode.NotDefined;

        TutorialButton.onClick.AddListener(TutorialButtonClick);
        MainRoomButton.onClick.AddListener(mainButtonClick);
        ArmyButton.onClick.AddListener(armyButtonClick);

        playButton.onClick.AddListener(playButtonClick);

        StartButton.interactable = false;
        inputField.text = PhotonNetwork.PhotonServerSettings.ServerAddress;
    }

    private void Start() {

        //asyncLoad=SceneManager.LoadSceneAsync("Intro");
        //asyncLoad.allowSceneActivation = false;
    }

    private AsyncOperation asyncLoad;

    private GameMode currentMode;
    private Level currentlevel;
    private void singleModeClick() {
        
        turnPrevIntaractable();
        currentMode = GameMode.Offline;
        singleButton.interactable = false;
        StartButton.interactable = true;
        currentlevel = Level.NotDefined;
    }

    private void turnPrevIntaractable() {
        switch (currentMode) {
            case GameMode.NotDefined:
                break;
            case GameMode.Offline:
                singleButton.interactable = true;
                break;
            case GameMode.Multiplayer:
                multButton.interactable = true;
                break;
            case GameMode.LocalMode:
                localButton.interactable = true;
                break;
        }
    }

    private void multButtonClick() {
        turnPrevIntaractable();
        currentMode = GameMode.Multiplayer;
        multButton.interactable = false;
        StartButton.interactable = true;
    }

    private void localButtonClick() {
        turnPrevIntaractable();
        currentMode = GameMode.LocalMode;
        localButton.interactable = false;
        StartButton.interactable = isValidIp(inputField.text);
    }

    private bool started=false;
    private void startButtonClick() {
        
        Debug.Log("Started with mode"+currentMode+(currentMode==GameMode.LocalMode?" with ip:"+ inputField.text : ""));
        
        switch (currentMode) {
            case GameMode.NotDefined:
                break;
            case GameMode.Offline:
                PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.OfflineMode;
                Debug.Log("Changing settings to offline");
                break;
            case GameMode.Multiplayer:
                PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.PhotonCloud;
                break;
            case GameMode.LocalMode:
                Debug.Log("Changing settings to selfhosted ip='" + inputField.text + "'");
                PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.SelfHosted;
                PhotonNetwork.PhotonServerSettings.ServerAddress = inputField.text;
                break;
        }
        networkGroup.SetActive(false);
        playGroup.SetActive(true);
    }

    private void armyButtonClick() {
        turnPrevIntaractableLevel();
        currentlevel = Level.Army;
        ArmyButton.interactable = false;
        playButton.interactable = true;
        playButtonClick();
    }
    private void mainButtonClick() {
        turnPrevIntaractableLevel();
        currentlevel = Level.MainRoom;
        MainRoomButton.interactable = false;
        playButton.interactable = true;
        playButtonClick();
    }
    private void TutorialButtonClick() {
        turnPrevIntaractableLevel();
        currentlevel = Level.Tutorial;
        TutorialButton.interactable = false;
        playButton.interactable = true;
        playButtonClick();
    }


    private void turnPrevIntaractableLevel() {
        switch (currentlevel){
            case Level.Tutorial:
                TutorialButton.interactable = true;
                break;
            case Level.MainRoom:
                MainRoomButton.interactable = true;
                break;
            case Level.Army:
                ArmyButton.interactable = true;
                break;
        }
    }

    private void playButtonClick() {
        if (started)
            return;
        switch (currentlevel) {
            case Level.Tutorial:
                SceneManager.LoadScene("Intro");
                break;
            case Level.MainRoom:
                SceneManager.LoadScene("NewEnvironment");
                break;
            case Level.Army:
                SceneManager.LoadScene("RoomTir");
                break;
        }



        //asyncLoad.allowSceneActivation = true;
        started = true;
    }


    private string ip;
    public void CheckIp(string str) {
        ip = inputField.text;
        Debug.Log(ip+"  "+ isValidIp(ip));
        if (currentMode == GameMode.LocalMode)
            StartButton.interactable = isValidIp(ip);
    }

    
    public static bool isValidIp(string ipString)
    {
        if (String.IsNullOrWhiteSpace(ipString))
        {
            return false;
        }

        string[] splitValues = ipString.Split('.');
        if (splitValues.Length != 4)
        {
            return false;
        }

        return splitValues.All(r => byte.TryParse(r, out _));
    }
}

public enum GameMode {
    NotDefined,
    Offline,
    Multiplayer,
    LocalMode
}

public enum Level {
    NotDefined,
    Tutorial,
    MainRoom,
    Army
}