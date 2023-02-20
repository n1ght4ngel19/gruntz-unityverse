using System.Collections;
using System.Linq;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public abstract class Grunt : MonoBehaviour {
    [field: SerializeField] public Equipment Equipment { get; set; }
    [field: SerializeField] public NavComponent NavComponent { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public bool IsSelected { get; set; }
    [field: SerializeField] public bool IsMovementInterrupted { get; set; }
    [field: SerializeField] public Owner Owner { get; set; }


    protected void Start() {
      NavComponent = gameObject.AddComponent<NavComponent>();
      Equipment = gameObject.AddComponent<Equipment>();
      Animator = gameObject.GetComponent<Animator>();
    }

    private void Update() {
      // Save new target for Grunt when it gets a command while moving to another tile
      if (Input.GetMouseButtonDown(1) && IsSelected && NavComponent.IsMoving) {
        NavComponent.SavedTargetLocation = SelectorCircle.Instance.OwnLocation;
        NavComponent.HaveSavedTarget = true;

        return;
      }

      // Set target to previously saved target, if there's one
      if (!NavComponent.IsMoving) {
        if (NavComponent.HaveSavedTarget) {
          NavComponent.TargetLocation = NavComponent.SavedTargetLocation;
          NavComponent.HaveSavedTarget = false;

          return;
        }
      }

      // Set target with the mouse if nothing interrupts
      if (Input.GetMouseButtonDown(1)
        && IsSelected
        && SelectorCircle.Instance.OwnLocation != NavComponent.OwnLocation) {
        NavComponent.TargetLocation = SelectorCircle.Instance.OwnLocation;
      }

      if (LevelManager.Instance.nodeList.Any(
        node => node.isBlocked && node.GridLocation.Equals(NavComponent.OwnLocation)
      )) {
        StartCoroutine(GetSquashed());
      }

      if (!IsMovementInterrupted) {
        HandleMovement();
      }
    }

    private void HandleMovement() {
      if (!NavComponent.TargetLocation.Equals(NavComponent.OwnLocation)) {
        Animator.Play($"Walk_{NavComponent.FacingDirection}");
        NavComponent.MoveTowardsTarget();
      } else {
        Animator.Play($"Idle_{NavComponent.FacingDirection}");
      }
    }

    public IEnumerator PickupItem(GameObject item) {
      // Get appropriate Animation Clip from AnimationManager and set it to 
      Animator.Play("Pickup_Item");
      IsMovementInterrupted = true;

      yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);

      IsMovementInterrupted = false;
      Destroy(item);
    }

    public IEnumerator GetSquashed() {
      // Get appropriate Animation Clip from AnimationManager and set it to 
      Animator.Play("Death_Squash");
      IsMovementInterrupted = true;

      yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);

      NavComponent.OwnLocation = new Vector2Int(int.MaxValue, int.MaxValue);
      Destroy(gameObject);
    }
    
    public IEnumerator FallInHole() {
      Animator.Play("Death_Hole");
      IsMovementInterrupted = true;

      yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);

      NavComponent.OwnLocation = new Vector2Int(int.MaxValue, int.MaxValue);
      Destroy(gameObject);
    }

    protected void OnMouseDown() {
      IsSelected = true;

      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(grunt => grunt != this)) {
        grunt.IsSelected = false;
      }
    }
  }
}
