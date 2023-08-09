using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.MapItemz.Misc {
  public class Coin : MapItem {
    protected override void Start() {
      base.Start();
      RotationAnimation = Resources.Load<AnimationClip>("Animationz/MapItemz/Misc/Clipz/Coin_Rotating");
      Animancer.Play(RotationAnimation);
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(grunt => grunt.AtNode(ownNode))) {
        SetEnabled(false);

        StatzManager.Instance.acquiredCoinz++;

        StartCoroutine(grunt.PickupMiscItem(nameof(Coin)));

        break;
      }
    }
  }
}
