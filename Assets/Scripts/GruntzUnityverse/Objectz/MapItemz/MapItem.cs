using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class MapItem : MapObject {
    [field: SerializeField] public AnimationClip RotationAnimation { get; set; }

    protected override void Start() {
      base.Start();

      animancer.Play(RotationAnimation);
    }

    protected override void LoadAnimationz() {
      Addressables.LoadAssetAsync<AnimationClip>($"{GetType()}_Rotating.anim")
        .Completed += (handle) => {
        RotationAnimation = handle.Result;
      };
    }
  }
}
