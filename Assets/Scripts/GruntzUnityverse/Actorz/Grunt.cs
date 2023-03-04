using System.Collections;
using System.Linq;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public class Grunt : MonoBehaviour {
    [field: SerializeField] public Equipment Equipment { get; set; }
    [field: SerializeField] public NavComponent NavComponent { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public bool IsSelected { get; set; }
    [field: SerializeField] public bool IsMovementInterrupted { get; set; }
    [field: SerializeField] public Owner Owner { get; set; }


    protected void Start() { Animator = gameObject.GetComponent<Animator>(); }

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

      if (LevelManager.Instance.nodeList.Any(node => node.isBlocked && IsOnLocation(node.GridLocation))) {
        // Play drowning animation when on a Lake (Water or Death) tile
        if (LevelManager.Instance.LakeLayer.HasTile(
          new Vector3Int(NavComponent.OwnLocation.x, NavComponent.OwnLocation.y, 0)
        )) {
          StartCoroutine(Sink());
        } else {
          StartCoroutine(GetSquashed());
        }
      }

      if (!IsMovementInterrupted) {
        HandleMovement();
      }
    }

    public bool IsOnLocation(Vector2Int location) { return NavComponent.OwnLocation.Equals(location); }

    public bool HasTool(string tool) {
      return Equipment.Tool.Type.ToString()
        .Equals(tool);
    }

    public bool HasToy(string toy) {
      return Equipment.Toy.Type.ToString()
        .Equals(toy);
    }

    public bool HasPowerup(string powerup) {
      return Equipment.Powerup.Type.ToString()
        .Equals(powerup);
    }

    public bool HasItem(string item) { return HasTool(item) || HasToy(item) || HasPowerup(item); }

    private void HandleMovement() {
      if (IsOnLocation(NavComponent.TargetLocation)) {
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController();

        // Debug.Log(
        //   Animator.runtimeAnimatorController.animationClips[0]
        //     .name
        // );

        Animator.Play($"Idle_{NavComponent.FacingDirection}");
      } else {
        Animator.Play($"Walk_{NavComponent.FacingDirection}");
        NavComponent.MoveTowardsTarget();
      }
    }

    public IEnumerator PickupItem(GameObject item) {
      // Todo: Play corresponding Animation Clip
      Animator.Play("Pickup_Item");
      IsMovementInterrupted = true;

      yield return new WaitForSeconds(
        Animator.GetCurrentAnimatorStateInfo(0)
          .length
      );

      IsMovementInterrupted = false;
    }

    public IEnumerator GetSquashed() {
      enabled = false;
      Animator.Play("Death_Squash");
      IsMovementInterrupted = true;

      yield return new WaitForSeconds(
        Animator.GetCurrentAnimatorStateInfo(0)
          .length
      );

      NavComponent.OwnLocation = Vector2IntCustom.Max;
      Destroy(gameObject);
    }

    public IEnumerator Sink() {
      enabled = false;
      // Get appropriate Animation Clip from AnimationManager and set it to 
      Animator.Play("Death_Sink");
      IsMovementInterrupted = true;

      yield return new WaitForSeconds(
        Animator.GetCurrentAnimatorStateInfo(0)
          .length
      );

      NavComponent.OwnLocation = Vector2IntCustom.Max;
      Destroy(gameObject);
    }

    public IEnumerator FallInHole() {
      enabled = false;
      Animator.Play("Death_Hole");
      IsMovementInterrupted = true;

      yield return new WaitForSeconds(
        Animator.GetCurrentAnimatorStateInfo(0)
          .length
      );

      NavComponent.OwnLocation = Vector2IntCustom.Max;
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
