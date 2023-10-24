using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.AnimationPackz {
  public class ToyPlayPack {
    public AnimationClip beachball1;
    public AnimationClip beachball2;

    public ToyPlayPack () {
      LoadToyPlayAnimations();
    }

    private void LoadToyPlayAnimations() {
      Addressables.LoadAssetAsync<AnimationClip>("BeachballGrunt_01.anim").Completed += handle => {
        beachball1 = handle.Result;
      };
      Addressables.LoadAssetAsync<AnimationClip>("BeachballGrunt_02.anim").Completed += handle => {
        beachball2 = handle.Result;
      };
    }
  }
}
