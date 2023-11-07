using Animancer;
using UnityEngine;

namespace GruntzUnityverse {
  /// <summary>
  /// Helper script for displaying animated GameObjects in purely UI Scenes (e.g. Main Menu).
  /// </summary>
  public class SimpleAnimated : MonoBehaviour {
    /// <summary>
    /// The AnimationClip to be played by the GameObject.
    /// </summary>
    public AnimationClip clipToPlay;

    private AnimancerComponent _animancer;
    private Animator _animator;

    private void Start() {
      _animator = GetComponent<Animator>();
      _animancer = GetComponent<AnimancerComponent>();
      _animancer.Animator = _animator;
      _animancer.Play(clipToPlay);
    }
  }
}
