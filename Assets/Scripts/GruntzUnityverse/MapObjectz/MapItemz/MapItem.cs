using System.Collections;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.Itemz;
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

      Addressables.LoadAssetAsync<AnimationClip>($"{pickupItem.mapItemName}_Rotating.anim")
        .Completed += (handle) => {
        RotationAnimation = handle.Result;

        animancer.Play(RotationAnimation);
      };
    }

    protected virtual void HandlePickup() {
      foreach (Grunt grunt in LevelManager.Instance.playerGruntz.Where(grunt => grunt.AtNode(ownNode))) {
        SetEnabled(false);

        StatzManager.Instance.acquiredCoinz++;

        StartCoroutine(grunt.PickupItem(pickupItem));

        // StartCoroutine(grunt.PickupMiscItem($"Pickup_{pickupItem.mapItemName}.anim"));

        break;
      }
    }
  }
}
