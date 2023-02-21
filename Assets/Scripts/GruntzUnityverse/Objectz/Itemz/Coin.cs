using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  public class Coin : MapObject {
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public bool HasBeenTouched { get; set; }

    protected override void Start() {
      base.Start();

      Animator = gameObject.GetComponentInChildren<Animator>();
      Animator.Play("Collectible_Spinning");
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz) {
        if (grunt.IsOnLocation(OwnLocation) && !HasBeenTouched) {
          HasBeenTouched = true;

          StatzManager.Instance.acquiredCoinz++;

          StartCoroutine(grunt.PickupItem(gameObject));
        }
      }
    }
  }
}
