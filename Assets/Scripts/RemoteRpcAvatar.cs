using System;
using System.Collections;
using System.IO;
using ExitGames.SportShooting;
using Oculus.Avatar;
using UnityEngine;
using VRTK;

public class RemoteRpcAvatar : MonoBehaviour {

    public OvrAvatar Avatar;
    public bool remote;
    [Header("Photon Params")]
    [SerializeField]
    private PhotonView _photonView;
    private int PacketSequence = 0;

    public void OnEnable() {
        StartCoroutine(startRecord());
    }

    public void LoadLikeLocal() {
        StopAllCoroutines();
        remote = false;
        Avatar.ShowThirdPerson = false;
        Avatar.ShowFirstPerson = true;
        Avatar.RecordPackets = true;
        Avatar.PacketRecorded += OnLocalAvatarPacketRecorded;
        var k=gameObject.GetComponent<OvrAvatarRemoteDriver>();
        if(k)Destroy(k);
        Avatar.Driver=gameObject.AddComponent<OvrAvatarLocalDriver>();
        //Avatar.Driver.Mode = Avatar.UseSDKPackets ?OvrAvatarDriver.PacketMode.SDK: OvrAvatarDriver.PacketMode.Unity;
        //GetComponent<VRTK_ObjectFollow>().enabled = true;
    }
    
    private IEnumerator startRecord() {
        yield return null;
        yield return null;
        if (!remote) {
            
        } else {
            Avatar.ShowFirstPerson = false;
            Avatar.ShowThirdPerson = true;
            Avatar.Driver = gameObject.AddComponent<OvrAvatarRemoteDriver>();
        }
            
    }
    void OnLocalAvatarPacketRecorded(object sender, OvrAvatar.PacketEventArgs args)
    {
        using (MemoryStream outputStream = new MemoryStream())
        {
            BinaryWriter writer = new BinaryWriter(outputStream);

            var size = CAPI.ovrAvatarPacket_GetSize(args.Packet.ovrNativePacket);
            byte[] data = new byte[size];
            CAPI.ovrAvatarPacket_Write(args.Packet.ovrNativePacket, size, data);

            writer.Write(PacketSequence++);
            writer.Write(size);
            writer.Write(data);
            _photonView.RPC("SendAvatarPacketData", PhotonTargets.Others, outputStream.ToArray());
            //SendPacketData(outputStream.ToArray());
        }
    }

    [PunRPC]
    public void SendAvatarPacketData(byte[] data)
    {
        using (MemoryStream inputStream = new MemoryStream(data))
        {
            BinaryReader reader = new BinaryReader(inputStream);
            int sequence = reader.ReadInt32();

            int size = reader.ReadInt32();
            byte[] sdkData = reader.ReadBytes(size);

            IntPtr packet = CAPI.ovrAvatarPacket_Read((UInt32)data.Length, sdkData);
            var driver= Avatar.GetComponent<OvrAvatarRemoteDriver>();
            if(driver)driver.QueuePacket(sequence, new OvrAvatarPacket { ovrNativePacket = packet });
        }
    }
}
