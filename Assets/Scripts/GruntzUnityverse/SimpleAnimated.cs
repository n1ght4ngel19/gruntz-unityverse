using Animancer;
using UnityEngine;

namespace GruntzUnityverse {
  public class SimpleAnimated : MonoBehaviour {
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
