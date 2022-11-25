using System;
using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using ExitGames.SportShooting;
using UnityEngine;

public class WhiteBoardController : MonoBehaviour {

    public Camera decalCamera;
    private PhotonView _view;
    [SerializeField] private Renderer buferRenderer;
    private void Awake() {
        _view = GetComponent<PhotonView>();
        NetworkController.OnGameConnected += OnConnect;
    }


    private void OnConnect() {
        if (!PhotonNetwork.isMasterClient){
            _view.RPC("SendCurImg", PhotonTargets.MasterClient,PhotonNetwork.player);
        }
    }


    public Renderer stamp;
    private int colorId = Shader.PropertyToID("_Color");
    public void Paint(Vector2 uvCoord, Color clr) {
        paint(uvCoord,clr);
        if(_view&&PhotonNetwork.inRoom)
            _view.RPC("PaintRemote",PhotonTargets.Others,uvCoord,clr.r,clr.g,clr.b,clr.a);
    }

    private void paint(Vector2 uvCoord, Color clr) {
        //Debug.Log(uvCoord - new Vector2(0.5f, 0.5f));

        stamp.material.SetColor(colorId, clr);// = clr;

        stamp.transform.localPosition =uvCoord -new Vector2(0.5f,0.5f);
        stamp.transform.localPosition +=new Vector3(0f,0f,0.1f);
        stamp.gameObject.SetActive(true);


        //spriteBuffer.material.mainTexture = renderBuffer[prevRenderBufferIndex];
        //Debug.LogError("2");
        //decalImage.material.mainTexture = renderBuffer[currRenderBufferIndex];
       // decalImage.texture = renderBuffer[currRenderBufferIndex];

        //Debug.LogError("3");
        //decalCamera.targetTexture = renderBuffer[currRenderBufferIndex];
        decalCamera.Render();
        
        //Debug.LogError("4");
        //int temp = prevRenderBufferIndex;
        //prevRenderBufferIndex = currRenderBufferIndex;
        //currRenderBufferIndex = temp;

        stamp.gameObject.SetActive(false);
    }
    [PunRPC]
    private void PaintRemote(Vector2 uvCoord, float clrR, float clrG, float clrB, float clrA) {
        paint(uvCoord, new Color(clrR,clrG,clrB,clrA));
    }


#region Clear

      [Button]
    private void clearBoard()
    {
        decalCamera.clearFlags = CameraClearFlags.SolidColor;
        decalCamera.Render();
        decalCamera.clearFlags = CameraClearFlags.Nothing;

    }
    public void ClearBoard()
    {
        clearBoard();
        if (_view && PhotonNetwork.inRoom)
            _view.RPC("ClearBoardRemote", PhotonTargets.Others);
    }
    [PunRPC]
    private void ClearBoardRemote() {
        clearBoard();
    }

#endregion

    //[SerializeField] private Texture2D text;
    [SerializeField] private RenderTexture fromRT;




    [PunRPC]
    private void SendCurImg(PhotonPlayer newPlayer) {
        var oldRT = RenderTexture.active;
        var tex = new Texture2D(fromRT.width, fromRT.height);
        RenderTexture.active = fromRT;
        tex.ReadPixels(new Rect(0, 0, fromRT.width, fromRT.height), 0, 0);
        tex.Apply();
        RenderTexture.active = oldRT;
        _view.RPC("ReceiveImg",newPlayer,tex.EncodeToPNG());
        
    }

    [PunRPC]
    private void ReceiveImg(byte[] textureInBytes) {
        Texture2D tex = new Texture2D(512, 512);
        tex.LoadImage(textureInBytes);
        buferRenderer.material.mainTexture = tex;
        buferRenderer.gameObject.SetActive(true);
        decalCamera.Render();
        buferRenderer.gameObject.SetActive(false);
    }
}
