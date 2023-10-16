using Animancer;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.Actorz {
  public class SelectedCircle : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public AnimancerComponent animancer;
    private Animator _animator;

    private void Start() {
      spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      animancer.Animator = _animator;
      spriteRenderer.enabled = false;

      Addressables.LoadAssetAsync<AnimationClip>("SelectedCircle.anim").Completed += handle => {
        animancer.Play(handle.Result);
      };
    }
  }
}
