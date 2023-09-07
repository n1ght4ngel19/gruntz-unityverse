using System.Collections;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.MapObjectz.Itemz;
using GruntzUnityverse.MapObjectz.MapItemz.Misc;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.MapItemz {
  public class MapItem : MapObject {
    [field: SerializeField] public AnimationClip RotationAnimation { get; set; }
    public Item pickupItem;
    // -------------------------------------------------------------------------------- //

    protected override void Start() {
      pickupItem = GetComponent<Item>();

      base.Start();
    }
    // -------------------------------------------------------------------------------- //

    private void Update() {
      HandlePickup();
    }
    // -------------------------------------------------------------------------------- //

    protected override IEnumerator LoadAndPlayAnimation() {
      yield return new WaitUntil(() => pickupItem.mapItemName.Length > 0);

      if (pickupItem.mapItemName == nameof(Warpletter)) {
        Addressables.LoadAssetAsync<AnimationClip>($"{nameof(Warpletter)}{((Warpletter)pickupItem).warpletterType}_Rotating.anim")
          .Completed += (handle) => {
          RotationAnimation = handle.Result;

          animancer.Play(RotationAnimation);
        };
      } else {
        Addressables.LoadAssetAsync<AnimationClip>($"{pickupItem.mapItemName}_Rotating.anim")
          .Completed += (handle) => {
          RotationAnimation = handle.Result;

          animancer.Play(RotationAnimation);
        };
      }
    }

    protected virtual void HandlePickup() {
      foreach (Grunt grunt in GameManager.Instance.currentLevelManager.playerGruntz.Where(grunt => grunt.AtNode(ownNode))) {
        SetEnabled(false);

        StartCoroutine(grunt.PickupItem(pickupItem));

        break;
      }
    }
  }
}
