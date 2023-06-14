using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class Pyramid : MapObject {
    [field: SerializeField] public bool IsDown { get; set; }
    private AnimationClip DownAnim { get; set; }
    private AnimationClip UpAnim { get; set; }


    protected override void Start() {
      base.Start();

      LevelManager.Instance.SetInaccessibleAt(Location, !IsDown);

      DownAnim = Resources.Load<AnimationClip>($"Animationz/MapObjectz/Pyramidz/Clipz/{GetType().Name}_Down");
      UpAnim = Resources.Load<AnimationClip>($"Animationz/MapObjectz/Pyramidz/Clipz/{GetType().Name}_Up");
    }

    public void TogglePyramid() {
      Animancer.Play(IsDown ? UpAnim : DownAnim);

      IsDown = !IsDown;
      IsHardTurn = !IsDown;
      LevelManager.Instance.SetInaccessibleAt(Location, !IsDown);
      LevelManager.Instance.SetHardTurnAt(Location, IsHardTurn);
    }
  }
}
