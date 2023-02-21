using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class Pyramid : MapObject {
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public bool IsDown { get; set; }
    [field: SerializeField] public bool IsInitialized { get; set; }


    protected override void Start() {
      base.Start();

      Animator = gameObject.GetComponentInChildren<Animator>();
    }

    protected virtual void Update() {
      InitializeNodeAtOwnLocation();
    }

    protected void InitializeNodeAtOwnLocation() {
      IsInitialized = true;
      LevelManager.Instance.SetBlockedAt(OwnLocation, !IsDown);
    }

    public void TogglePyramid() {
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
