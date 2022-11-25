
using TsSDK;
using UnityEngine;
using UnityEngine.UI;

public class TsHapticAnimationPlayback : MonoBehaviour
{
    [SerializeField] private Button m_playButton;
    [SerializeField] private Button m_pauseButton;
    [SerializeField] private Button m_stopButton;
    [SerializeField] private Slider m_progressSlider;


    [SerializeField]
    private TsHapticPlayer m_hapticPlayer;

    [SerializeField]
    private TsHapticAnimationAsset m_animationAsset;


    private void Start()
    {
        m_playButton.onClick.AddListener(Play);
        m_pauseButton.onClick.AddListener(Pause);
        m_stopButton.onClick.AddListener(Stop);
    }

    private void Update()
    {
        if (m_hapticPlayer.IsPlaying(m_animationAsset.Instance))
        {
            var duration = GetDuration();
            var time = GetTime();

            var progress = ((float) time) / duration;
            m_progressSlider.value = progress;
        }
    }

    public void Play()
    {
        m_hapticPlayer.Play(m_animationAsset.Instance);
    }

    public void Stop()
    {
        m_hapticPlayer.Stop(m_animationAsset.Instance);
    }

    public void Pause()
    {
        m_hapticPlayer.SetPaused(m_animationAsset.Instance, true);
    }

    public ulong GetTime()
    {
        return m_hapticPlayer.GetTime(m_animationAsset.Instance);
    }

    public ulong GetDuration()
    {
        return m_hapticPlayer.GetDuration(m_animationAsset.Instance);
    }
}
