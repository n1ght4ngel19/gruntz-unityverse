using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace _Test
{
  public class TGrunt : MonoBehaviour
  {
    [field: SerializeField] public TNavComponent NavComponent { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public bool IsSelected { get; set; }


    private void Start()
    {
      Animator = gameObject.GetComponent<Animator>();
      NavComponent = gameObject.AddComponent<TNavComponent>();
      NavComponent.OwnLocation = Vector2Int.FloorToInt(transform.position);
      NavComponent.TargetLocation = NavComponent.OwnLocation;
      NavComponent.FacingDirection = CompassDirection.South;
    }

    private void Update()
    {
      PlayAnimation();

      if (Input.GetMouseButtonDown(1)
        && IsSelected
        && NavComponent.IsMoving)
      {
        NavComponent.SavedTargetLocation = SelectorCircle.Instance.GridLocation;
        NavComponent.HaveSavedTarget = true;

        return;
      }

      if (NavComponent.HaveSavedTarget
        && !NavComponent.IsMoving)
      {
        NavComponent.TargetLocation = NavComponent.SavedTargetLocation;
        NavComponent.HaveSavedTarget = false;

        return;
      }

      if (Input.GetMouseButtonDown(1)
        && IsSelected
        && SelectorCircle.Instance.GridLocation != NavComponent.OwnLocation
      )
      {
        NavComponent.TargetLocation = SelectorCircle.Instance.GridLocation;
      }

      // This gets called every time, since it only needs the target, which is always provided
      NavComponent.MoveTowardsTarget();
    }

    public void PlayAnimation()
    {
      string animationType = NavComponent.IsMoving ? "Walk" : "Idle";
      Animator.Play($"BareHandzGrunt_{animationType}_{NavComponent.FacingDirection}");
    }

    protected void OnMouseDown()
    {
      IsSelected = true;

      foreach (TGrunt grunt in LevelManager.Instance.testGruntz.Where(grunt => grunt != this))
      {
        grunt.IsSelected = false;
      }
    }
  }
}
