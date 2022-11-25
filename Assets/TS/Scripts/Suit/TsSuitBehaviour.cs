using TsSDK;
using UnityEngine;

public class TsSuitBehaviour : TsDeviceBehaviour
{
    public SuitIndex TargetSuitIndex { get { return m_suitIndex; }  set { m_suitIndex = value;}}

    public ISuit Suit { get { return (ISuit)Device; } }
    
    [SerializeField]
    private SuitIndex m_suitIndex = SuitIndex.Suit0;
    
    private void Start()
    {
        var suitManager = TsManager.Root.SuitManager;
        suitManager.OnSuitConnected += OnSuitConnected; ;
        suitManager.OnSuitDisconnected += OnSuitDisconnected;

        foreach (var suit in suitManager.Suits)
        {
            OnSuitConnected(suit);
        }
    }

    private void OnSuitConnected(ISuit obj)
    {
        if (obj.Index != TargetSuitIndex)
        {
            return;
        }
        UpdateState(obj, true);
    }

    private void OnSuitDisconnected(ISuit obj)
    {
        if (obj.Index != TargetSuitIndex)
        {
            return;
        }
        UpdateState(null, false);
    }
}
