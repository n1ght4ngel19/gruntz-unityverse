using System;
using System.Linq;
using _Test;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Collectiblez {
  public class Coin : MonoBehaviour {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }

    private void Start() {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Animator = gameObject.GetComponentInChildren<Animator>();
      Animator.Play("Coin_Spinning");
    }

    private void Update() {
      foreach (TGrunt grunt in LevelManager.Instance.PlayerGruntz) {
        if (grunt.NavComponent.OwnLocation.Equals(OwnLocation)) {
          grunt.PlayPickupAnimation("Coin");

          RemoveSelfFromGame();
        }
      }

      if (LevelManager.Instance.PlayerGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))) {}
    }

    private void RemoveSelfFromGame() {
      StatzManager.Instance.acquiredCoinz++;

      Destroy(gameObject);
    }
  }
}
