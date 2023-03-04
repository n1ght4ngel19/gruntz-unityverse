using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class Coin : MapObject {
    protected override void Start() {
      base.Start();

      Animator = gameObject.GetComponentInChildren<Animator>();
      Animator.Play("MapItem_Spinning");
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(grunt => grunt.IsOnLocation(OwnLocation))) {
        enabled = false;
        StatzManager.Instance.acquiredCoinz++;

        StartCoroutine(grunt.PickupItem(gameObject));
      }
    }
  }
}
