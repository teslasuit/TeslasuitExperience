using System.Linq;
using TsSDK;
using UnityEngine;

public class TsHapticCollisionHandler : MonoBehaviour
{
    [SerializeField]
    public TsHapticPlayer HapticPlayer;

    [SerializeField]
    public TsHapticSimplifiedChannel Channel;

    public IMapping2dElectricChannel[] GetChannels()
    {
        var device = HapticPlayer.Device;
        if (device == null)
        {
            return null;
        }
        return device.Mapping2d.ElectricChannels.Where((item) =>
            item.BoneIndex == Channel.BoneIndex && item.BoneSide == Channel.BoneSide).ToArray();
    }

    public void AddImpact(IHapticMaterialPlayable material, float impact, int duration)
    {
        if (!HapticPlayer.IsAvailable)
        {
            return;
        }

        var channels = GetChannels();
        if (channels == null)
        {
            return;
        }
        material.Play();
        foreach (var channel in channels)
        {
            HapticPlayer.PlayerHandle.AddImpact(material, channel, impact, duration);
        }
    }

    public void RemoveImpact(IHapticMaterialPlayable material)
    {
        if (!HapticPlayer.IsAvailable)
        {
            return;
        }
        var channels = GetChannels();
        if (channels == null)
        {
            return;
        }

        foreach (var channel in channels)
        {
            HapticPlayer.PlayerHandle.RemoveImpact(material, channel);
        }
    }
}
