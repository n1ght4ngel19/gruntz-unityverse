using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz {
  public class CheckpointFlag : MapObject {
    private AnimationClip _wavingClip;

    protected override void Start() {
      base.Start();

      Addressables.LoadAssetAsync<AnimationClip>("CheckpointFlag_Waving.anim").Completed += handle => {
        _wavingClip = handle.Result;
      };
    }
    public IEnumerator PlayAnim() {
      animancer.Play(_wavingClip);

      yield return null;
    }
  }
}
