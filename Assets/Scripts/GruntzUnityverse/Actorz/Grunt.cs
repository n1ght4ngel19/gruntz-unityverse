using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public abstract class Grunt : MonoBehaviour {
    [field: SerializeField] public Equipment Equipment { get; set; }
    [field: SerializeField] public NavComponent NavComponent { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public bool IsSelected { get; set; }


    protected void Start() {
      NavComponent = gameObject.AddComponent<NavComponent>();
      Equipment = gameObject.AddComponent<Equipment>();
      Animator = gameObject.GetComponent<Animator>();
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
        NavComponent.MoveToTarget();
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

    public void PlayPickupAnimation(string pickupObject) { Animator.Play($"Pickup_{pickupObject}"); }

    protected void OnMouseDown() {
      IsSelected = true;

      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(grunt => grunt != this)) {
        grunt.IsSelected = false;
      }
    }
  }
}
