using UnityEngine;
using TMPro;
using Unity.Multiplayer.Samples.BossRoom.ApplicationLifecycle.Messages;
using Unity.Multiplayer.Samples.BossRoom.Shared;
using Unity.Multiplayer.Samples.BossRoom.Shared.Infrastructure;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;

namespace Unity.Multiplayer.Samples.BossRoom.Visual {
  /// <summary>
  /// Provides backing logic for all of the UI that runs in the PostGame stage.
  /// </summary>
  public class PostGameUI : MonoBehaviour {
    [SerializeField]
    private Light m_SceneLight;

    [SerializeField]
    private TextMeshProUGUI m_WinEndMessage;

    [SerializeField]
    private TextMeshProUGUI m_LoseGameMessage;

    [SerializeField]
    private GameObject m_ReplayButton;

    [SerializeField]
    private GameObject m_WaitOnHostMsg;

    [SerializeField]
    TransformVariable m_NetworkGameStateTransform;

    [SerializeField]
    private Color m_WinLightColor;

    [SerializeField]
    private Color m_LoseLightColor;

    IPublisher<QuitGameSessionMessage> m_QuitGameSessionPub;

    [Inject]
    void InjectDependencies(IPublisher<QuitGameSessionMessage> quitGameSessionPub) {
      m_QuitGameSessionPub = quitGameSessionPub;
    }

    void Start() {
      // only hosts can restart the game, other players see a wait message
      if (NetworkManager.Singleton.IsHost) {
        m_ReplayButton.SetActive(true);
        m_WaitOnHostMsg.SetActive(false);
      } else {
        m_ReplayButton.SetActive(false);
        m_WaitOnHostMsg.SetActive(true);
      }

      if (m_NetworkGameStateTransform && m_NetworkGameStateTransform.Value &&
          m_NetworkGameStateTransform.Value.TryGetComponent(out NetworkGameState networkGameState)) {
        SetPostGameUI(networkGameState.NetworkWinState.winState.Value);
      }
    }

    void SetPostGameUI(WinState winState) {
      // Set end message and background color based last game outcome
      if (winState == WinState.Win) {
        m_SceneLight.color = m_WinLightColor;
        m_WinEndMessage.gameObject.SetActive(true);
        m_LoseGameMessage.gameObject.SetActive(false);
      } else if (winState == WinState.Loss) {
        m_SceneLight.color = m_LoseLightColor;
        m_WinEndMessage.gameObject.SetActive(false);
        m_LoseGameMessage.gameObject.SetActive(true);
      }
    }

    public void OnPlayAgainClicked() {
      // this should only ever be called by the Host - so just go ahead and switch scenes
      SceneLoaderWrapper.Instance.LoadScene("CharSelect", useNetworkSceneManager: true);

      // FUTURE: could be improved to better support a dedicated server architecture
    }

    public void OnMainMenuClicked() {
      m_QuitGameSessionPub.Publish(new QuitGameSessionMessage() { UserRequested = true });
    }
  }
}

