using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Collectiblez {
  public class Coin : MonoBehaviour {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public bool HasBeenTouched { get; set; }

    private void Start() {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Animator = gameObject.GetComponentInChildren<Animator>();
      Animator.Play("Coin_Spinning");
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz) {
        if (grunt.NavComponent.OwnLocation.Equals(OwnLocation) && !HasBeenTouched) {
          HasBeenTouched = true;

          StatzManager.Instance.acquiredCoinz++;

          StartCoroutine(grunt.PickupItem(gameObject));
        }
      }
    }
  }
}
