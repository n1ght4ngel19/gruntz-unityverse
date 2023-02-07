using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class Pyramid : MonoBehaviour {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public bool IsDown { get; set; }

    private void Start() {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Animator = gameObject.GetComponentInChildren<Animator>();
    }

    public void ChangeState() {
      Animator.Play(
        IsDown
          ? "Pyramid_Up"
          : "Pyramid_Down"
      );

      IsDown = !IsDown;
      LevelManager.Instance.SetBlockedAt(OwnLocation, !IsDown);
    }
  }
}
