using TeslasuitAPI;
using UnityEngine;
using UnityEngine.UI;
using ISuit = TsSDK.ISuit;

public class ChooseSuitUiElement : MonoBehaviour {
    private GameObject thisObject;
    [SerializeField] private Text nameText;
    public ISuit Handle
    {
        get { return _handle; }
    }
    private ISuit _handle=null;
    [SerializeField] private Text connectButtonText;
    [SerializeField] private Text connectedText;
    //[SerializeField] private SuitAPIObject suit;
    [SerializeField] private TsSuitBehaviour suit;
    private GameObject _gameObject;

    private void Awake() {
        _gameObject = gameObject;
    }

    public void Connect() {
        //if (_handle == GameManager.Instance.CurrentHandle) {
        //    return;
        //}


        connectedText.gameObject.SetActive(true);
        connectedText.text = "Connecting";
        connectedText.enabled = true;
        //GameManager.Instance.OnConnect += OnConnect;
        try {
            
            // _handle.Open();
        } catch {

        }
        //GameManager.Instance.CurrentHandle = _handle;
        //connectedText.text = "Connected";
        //Debug.Log("[ChooseSuitUiElement]: Connect call handle is open");
        //Debug.Log("[ChooseSuitUiElement]: Connect call handle.index="+_handle.SuitIndex);
        //GameManager.Instance.TryConnectTo(_handle);
        suit.TargetSuitIndex = _handle.Index;
        //GameManager.Instance.CurrentHandle = _handle;
        connectedText.gameObject.SetActive(true);
        connectedText.text = "Connected";
        connectedText.enabled = true;
        connected = true;
        
    }

    private void _handle_Disconnected() {
        connected = false;
        MainThreadDispatcher.Execute(() => {
            connectedText.gameObject.SetActive(false);
            connectedText.text = "";
            connectedText.enabled = false;
        });
    }


    /*private void OnConnect() {
        MainThreadDispatcher.Execute(() =>{
            connectedText.gameObject.SetActive(true);
            connectedText.text = "Connected";
            connectedText.enabled = true;
        });
        connected = true;
        GameManager.Instance.OnConnect -= OnConnect;
        GameManager.Instance.OnDisconnect += OnDisconnect;
    }

    private void OnDisconnect() {
        connected = false;
        MainThreadDispatcher.Execute(() => {
            connectedText.gameObject.SetActive(false);
            connectedText.text = "Connected";
            connectedText.enabled = false;
        });
        GameManager.Instance.OnDisconnect -= OnDisconnect;
    }*/

    public void Initialize(ISuit handle) {
        _handle = null;

        MainThreadDispatcher.Execute(() =>
       {
           if(nameText)nameText.text = handle.Ssid;
           if(_gameObject) _gameObject.SetActive(true);
       });
        
        _handle = handle;
    }

    public void Deinit(ISuit handle)
    {
        _handle_Disconnected();
    }

    private bool connected;
    /*private void OnDisable() {
        if(connected)
            GameManager.Instance.OnDisconnect -= OnDisconnect;
    }*/
    
    

    public void Disable() {
        MainThreadDispatcher.Execute(() =>
        {
            if(_gameObject) _gameObject.SetActive(false);
        });
    }
    
    
}
