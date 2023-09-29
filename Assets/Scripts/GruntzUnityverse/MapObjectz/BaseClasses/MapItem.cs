using System.Collections;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Itemz.Misc;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.BaseClasses {
  public class MapItem : MapObject {
    public Item pickupItem;
    private AnimationClip _rotationAnimation;

    private void Update() {
      HandlePickup();
    }

    // ------------------------------------------------------------ //
    // OVERRIDES
    // ------------------------------------------------------------ //
    public override void Setup() {
      base.Setup();

      pickupItem = GetComponent<Item>();

      StartCoroutine(LoadAndPlayAnimation());
    }

    protected override IEnumerator LoadAndPlayAnimation() {
      yield return new WaitUntil(() => pickupItem.mapItemName.Length > 0);

      string animKey = pickupItem.mapItemName == nameof(Warpletter)
        ? $"{nameof(Warpletter)}{((Warpletter)pickupItem).warpletterType}_Rotating.anim"
        : $"{pickupItem.mapItemName}_Rotating.anim";

      // Addressables.LoadAssetAsync<AnimationClip>($"{nameof(Warpletter)}{((Warpletter)pickupItem).warpletterType}_Rotating.anim")
      //   .Completed += handle => {
      //   _rotationAnimation = handle.Result;
      //
      //   animancer.Play(_rotationAnimation);
      // };
      // Addressables.LoadAssetAsync<AnimationClip>($"{pickupItem.mapItemName}_Rotating.anim")
      //   .Completed += handle => {
      //   _rotationAnimation = handle.Result;
      //
      //   animancer.Play(_rotationAnimation);
      // };
      Addressables.LoadAssetAsync<AnimationClip>(animKey).Completed += handle => {
        _rotationAnimation = handle.Result;

        animancer.Play(_rotationAnimation);
      };
    }

    // ------------------------------------------------------------ //
    // CLASS METHODS
    // ------------------------------------------------------------ //
    private void HandlePickup() {
      foreach (Grunt grunt in GameManager.Instance.currentLevelManager.playerGruntz.Where(grunt => grunt.navigator.ownNode == ownNode)) {
        SetEnabled(false);

        StartCoroutine(grunt.PickupItem(pickupItem));

        break;
      }
    }
  }
}
