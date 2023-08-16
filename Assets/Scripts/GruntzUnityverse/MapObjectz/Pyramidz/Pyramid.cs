using GruntzUnityverse.Managerz;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Pyramidz {
  public class Pyramid : MapObject {
    [field: SerializeField] public bool IsDown { get; set; }
    private AnimationClip _downAnim;
    private AnimationClip _upAnim;


    protected override void Start() {
      base.Start();

      LevelManager.Instance.SetBlockedAt(location, !IsDown);
    }

    protected override void LoadAnimationz() {
      Addressables.LoadAssetAsync<AnimationClip>($"{GetType().Name}_Down.anim").Completed +=
        (handle) => {
          _downAnim = handle.Result;
        };

      Addressables.LoadAssetAsync<AnimationClip>($"{GetType().Name}_Down.anim").Completed +=
        (handle) => {
          _upAnim = handle.Result;
        };
    }

    public void Toggle() {
      animancer.Play(IsDown ? _upAnim : _downAnim);

      IsDown = !IsDown;
      LevelManager.Instance.SetBlockedAt(location, !IsDown);
      LevelManager.Instance.SetHardTurnAt(location, !IsDown);
    }
  }
}
