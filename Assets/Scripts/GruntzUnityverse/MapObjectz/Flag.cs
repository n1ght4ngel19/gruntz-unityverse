using System.Collections;
using GruntzUnityverse.Enumz;
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
      };
    }

    public IEnumerator PlayAnim() {
      animancer.Play(_wavingClip);

      yield return null;
    }
  }
}
