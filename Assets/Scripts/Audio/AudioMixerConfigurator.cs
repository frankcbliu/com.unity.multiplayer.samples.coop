using UnityEngine;
using UnityEngine.Audio;

namespace Unity.Multiplayer.Samples.BossRoom.Client {
  /// <summary>
  /// Initializes the game's AudioMixer to use volumes stored in preferences. Provides
  /// a public function that can be called when these values change.
  /// </summary>
  public class AudioMixerConfigurator : MonoBehaviour {
    [SerializeField]
    private AudioMixer m_Mixer;

    [SerializeField]
    private string m_MixerVarMainVolume = "OverallVolume";

    [SerializeField]
    private string m_MixerVarMusicVolume = "MusicVolume";

    public static AudioMixerConfigurator Instance { get; private set; }

    /// <summary>
    /// The audio sliders use a value between 0.0001 and 1, but the mixer works in decibels -- by default, -80 to 0.
    /// To convert, we use log10(slider) multiplied by 20. Why 20? because log10(.0001)*20=-80, which is the
    /// bottom range for our mixer, meaning it's disabled.
    /// </summary>
    private const float k_VolumeLog10Multiplier = 20;

    private void Awake() {
      Instance = this;
      DontDestroyOnLoad(gameObject); // 这个 gameObject 指向的是这个脚本绑定的对象吗?
    }

    private void Start() {
      // note that trying to configure the AudioMixer during Awake does not work, must be initialized in Start
      Configure(); // 尝试在 Awake中初始化AudioMixer, 但失败了，必须在 Start中进行初始化
    }

    public void Configure() {
      m_Mixer.SetFloat(m_MixerVarMainVolume, GetVolumeInDecibels(ClientPrefs.GetMasterVolume()));
      m_Mixer.SetFloat(m_MixerVarMusicVolume, GetVolumeInDecibels(ClientPrefs.GetMusicVolume()));
    }

    private float GetVolumeInDecibels(float volume) {
      // 音量滑动条的值 volume 的范围是 0.0001 ~ 1
      if (volume <= 0) // sanity-check in case we have bad prefs data
      {
        volume = 0.0001f;
      }
      // 而 AudioMixer 中使用的范围是 -80~0, 因此需要做一层转换
      // lg(0.0001)*20 = -80, lg(1)*20 = 0
      return Mathf.Log10(volume) * k_VolumeLog10Multiplier;
    }
  }
}
