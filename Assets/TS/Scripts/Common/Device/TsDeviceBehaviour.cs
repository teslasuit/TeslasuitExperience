using System;
using TsSDK;
using UnityEngine;

public abstract class TsDeviceBehaviour : MonoBehaviour
{
    public IDevice Device { get; protected set; }
    public event Action<TsDeviceBehaviour, bool> ConnectionStateChanged = delegate { };
    public bool IsConnected { get; private set; }

    protected void UpdateState(IDevice device, bool isConnected)
    {
        if (isConnected == IsConnected)
        {
            return;
        }
        this.IsConnected = isConnected;
        this.Device = device;
        ConnectionStateChanged(this, isConnected);
        if (!isConnected)
        {
            this.Device = null;
        }
    }
}
