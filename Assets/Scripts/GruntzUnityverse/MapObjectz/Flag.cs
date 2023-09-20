using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz {
  public class Flag : MapObject {
    private AnimationClip _wavingClip;
    public FlagType flagType;

    protected override void Start() {
      base.Start();

      Addressables.LoadAssetAsync<AnimationClip>($"{flagType}_Waving.anim").Completed += handle => {
        _wavingClip = handle.Result;

        if (flagType != FlagType.CheckpointFlag) {
          PlayAnim();
        }
      };

    }

    public void PlayAnim() {
      animancer.Play(_wavingClip);
    }
  }
}
