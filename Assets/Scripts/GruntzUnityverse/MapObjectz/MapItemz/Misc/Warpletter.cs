using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.MapItemz.Misc {
  public class Warpletter : MapItem {
    public WarpletterType warpletterType;

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(grunt => grunt.AtNode(ownNode))) {
        SetEnabled(false);

        StatzManager.Instance.acquiredWarpletterz++;

        StartCoroutine(grunt.PickupMiscItem($"{nameof(Warpletter)}{warpletterType}"));

        break;
      }
    }

    protected override void LoadAnimationz() {
      Addressables.LoadAssetAsync<AnimationClip>($"{nameof(Warpletter)}{warpletterType}_Rotating.anim")
        .Completed += (handle) => {
        RotationAnimation = handle.Result;
      };
    }
  }
}
