using GruntzUnityverse.Enumz;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using System.Linq;
using UnityEngine;

namespace GruntzUnityverse.Objectz.MapItemz.Misc {
  public class Warpletter : MapItem {
    public WarpletterType warpletterType;

    protected override void Start() {
      base.Start();

      RotationAnimation = Resources.Load<AnimationClip>($"Animationz/MapItemz/Misc/Clipz/{nameof(Warpletter)}{warpletterType}_Rotating");
      Animancer.Play(RotationAnimation);
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(grunt => grunt.AtNode(ownNode))) {
        SetEnabled(false);

        StatzManager.Instance.acquiredWarpletterz++;

        StartCoroutine(grunt.PickupMiscItem($"{nameof(Warpletter)}{warpletterType}"));

        break;
      }
    }
  }
}
