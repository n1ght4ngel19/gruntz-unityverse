using System;
using System.Linq;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz;
using UnityEngine;

namespace _Test {
  public class TGrunt : MonoBehaviour {
    [field: SerializeField] public TNavComponent NavComponent { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public RuntimeAnimatorController LocomotionAnimatorController { get; set; }
    [field: SerializeField] public RuntimeAnimatorController PickupAnimatorController { get; set; }
    [field: SerializeField] public bool IsSelected { get; set; }


    private void Start() {
      Animator = gameObject.GetComponent<Animator>();
      NavComponent = gameObject.AddComponent<TNavComponent>();
      NavComponent.OwnLocation = Vector2Int.FloorToInt(transform.position);
      NavComponent.TargetLocation = NavComponent.OwnLocation;
      NavComponent.FacingDirection = CompassDirection.South;
      Animator.runtimeAnimatorController = LocomotionAnimatorController;
    }

    private void Update() {
      // PlayLocomotionAnimation();

      if (Input.GetMouseButtonDown(1) && IsSelected && NavComponent.IsMoving) {
        NavComponent.SavedTargetLocation = SelectorCircle.Instance.OwnLocation;
        NavComponent.HaveSavedTarget = true;

        return;
      }

      if (NavComponent.HaveSavedTarget && !NavComponent.IsMoving) {
        NavComponent.TargetLocation = NavComponent.SavedTargetLocation;
        NavComponent.HaveSavedTarget = false;

        return;
      }

      if (Input.GetMouseButtonDown(1)
        && IsSelected
        && SelectorCircle.Instance.OwnLocation != NavComponent.OwnLocation) {
        NavComponent.TargetLocation = SelectorCircle.Instance.OwnLocation;
      }

      if (!NavComponent.TargetLocation.Equals(NavComponent.OwnLocation)) {
        Animator.Play($"BareHandzGrunt_Walk_{NavComponent.FacingDirection}");
        NavComponent.MoveTowardsTarget();
      } else {
        Animator.Play($"BareHandzGrunt_Idle_{NavComponent.FacingDirection}");
      }
    }

    private void PlayLocomotionAnimation() {
      string animationType = NavComponent.IsMoving
        ? "Walk"
        : "Idle";

      Animator.Play($"BareHandzGrunt_{animationType}_{NavComponent.FacingDirection}");
    }

    public void PlayPickupAnimation(string pickupObject) {
      Animator.runtimeAnimatorController = PickupAnimatorController;
      Animator.Play($"Pickup_{pickupObject}");
    }

    protected void OnMouseDown() {
      IsSelected = true;

      foreach (TGrunt grunt in LevelManager.Instance.PlayerGruntz.Where(grunt => grunt != this)) {
        grunt.IsSelected = false;
      }
    }
  }
}
