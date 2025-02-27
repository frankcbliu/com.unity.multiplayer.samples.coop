using Unity.Netcode;
using UnityEngine;

namespace Unity.Multiplayer.Samples.BossRoom {
  /// <summary>
  /// MonoBehaviour containing only one NetworkVariable of type LifeState which represents this object's life state.
  /// </summary>
  public class NetworkLifeState : NetworkBehaviour {
    [SerializeField]
    NetworkVariable<LifeState> m_LifeState = new NetworkVariable<LifeState>(BossRoom.LifeState.Alive); // 初始化为 Alive

    public NetworkVariable<LifeState> LifeState => m_LifeState;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    /// <summary>
    /// Indicates whether this character is in "god mode" (cannot be damaged).
    // 可以在 editor 中开启上帝模式，上帝模式不会掉血
    /// </summary>
    public NetworkVariable<bool> IsGodMode { get; } = new NetworkVariable<bool>(false);
#endif
  }
}
