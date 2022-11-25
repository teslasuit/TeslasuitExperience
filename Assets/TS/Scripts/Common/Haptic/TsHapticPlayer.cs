using System;
using System.Collections.Generic;
using TsAPI.Types;
using TsSDK;
using UnityEngine;

[RequireComponent(typeof(TsDeviceBehaviour))]
public class TsHapticPlayer : MonoBehaviour
{
    private TsDeviceBehaviour m_deviceBehaviour;

    public IHapticPlayer PlayerHandle
    {
        get { return m_hapticPlayer; }
    }
    private IHapticPlayer m_hapticPlayer;
    public IDevice Device
    {
        get { return m_deviceBehaviour.Device; }
    }
    public bool IsAvailable
    {
        get { return m_deviceBehaviour.IsConnected; }
    }

    private Dictionary<IntPtr, IHapticPlayable> m_playables = new Dictionary<IntPtr, IHapticPlayable>();

    private void Start()
    {
        m_deviceBehaviour = GetComponent<TsDeviceBehaviour>();
        m_deviceBehaviour.ConnectionStateChanged += DeviceBehaviour_ConnectionStateChanged;
        if (m_deviceBehaviour.IsConnected)
        {
            DeviceBehaviour_ConnectionStateChanged(m_deviceBehaviour, true);
        }
    }

    private void DeviceBehaviour_ConnectionStateChanged(TsDeviceBehaviour devicetBehaviour, bool connected)
    {
        if(connected)
        {
            m_hapticPlayer = devicetBehaviour.Device.HapticPlayer;
        }
        else
        {
            m_hapticPlayer = null;
            m_playables.Clear();
        }
    }

    public void Play(IHapticAsset asset)
    {
        IHapticPlayable hapticPlayable = TryGetPlayable(asset);

        if (hapticPlayable == null)
        {
            hapticPlayable = m_hapticPlayer.CreatePlayable(asset, false);
            m_playables.Add(asset.Handle, hapticPlayable);
        }

        m_hapticPlayer.Play(hapticPlayable);
    }

    public void Add(IHapticAsset asset)
    {
        IHapticPlayable hapticPlayable = TryGetPlayable(asset);

        if (hapticPlayable == null)
        {
            hapticPlayable = m_hapticPlayer.CreatePlayable(asset, false);
            m_playables.Add(asset.Handle, hapticPlayable);
        }
    }

    public void Stop(IAsset asset)
    {
        var hapticPlayable = TryGetPlayable(asset);

        if (hapticPlayable != null)
        {
            m_hapticPlayer.Stop(hapticPlayable);
        }
    }

    public void SetPaused(IAsset asset, bool value)
    {
        var hapticPlayable = TryGetPlayable(asset);

        if (hapticPlayable != null)
        {
            hapticPlayable.IsPaused = value;
        }
    }

    public void SetLooped(IAsset asset, bool value)
    {
        var hapticPlayable = TryGetPlayable(asset);

        if (hapticPlayable != null)
        {
            hapticPlayable.IsLooped = value;
        }
    }

    public ulong GetTime(IAsset asset)
    {
        var hapticPlayable = TryGetPlayable(asset);

        if (hapticPlayable != null)
        {
            return hapticPlayable.TimeMs;
        }

        return 0;
    }

    public ulong GetDuration(IAsset asset)
    {
        var hapticPlayable = TryGetPlayable(asset);

        if (hapticPlayable != null)
        {
            return hapticPlayable.DurationMs;
        }

        return 0;
    }

    public bool IsPlaying(IAsset asset)
    {
        var hapticPlayable = TryGetPlayable(asset);

        if (hapticPlayable != null)
        {
            return hapticPlayable.IsPlaying;
        }

        return false;
    }

    private IHapticPlayable TryGetPlayable(IAsset asset)
    {
        if (m_playables.ContainsKey(asset.Handle))
        {
            return m_playables[asset.Handle];
        }

        return null;
    }

    public IHapticDynamicPlayable CreateTouch(int frequency, int amplitude, int pulseWidth, long duration)
    {
        TsHapticParam[] touchParams = new TsHapticParam[3];
        touchParams[0] = new TsHapticParam(TsHapticParamType.Period, (ulong) (1000000 / frequency));
        touchParams[1] = new TsHapticParam(TsHapticParamType.Amplitude, (ulong) amplitude);
        touchParams[2] = new TsHapticParam(TsHapticParamType.PulseWidth, (ulong) pulseWidth);
        return (IHapticDynamicPlayable)m_hapticPlayer.CreateTouch(touchParams, new IntPtr[0], (ulong)duration);
    }

}
